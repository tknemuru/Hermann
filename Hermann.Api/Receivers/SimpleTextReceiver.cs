using Hermann.Contexts;
using Hermann.Collections;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// Hermannにおける標準テキスト形式文字列の受信機能を提供します。
    /// </summary>
    public class SimpleTextReceiver : IReceivable<string>
    {
        /// <summary>
        /// 標準テキストファイルを受信しフィールドの状態に変換します。
        /// </summary>
        /// <param name="source">標準テキストファイルパス</param>
        /// <returns>フィールドの状態</returns>
        public FieldContext Receive(string source)
        {
            var context = new FieldContext();
            var lines = FileHelper.ReadTextLines(source).ToArray();
            Debug.Assert((lines.Length == SimpleText.Length.Sum), string.Format("テキストファイルの行数が不正です。{0}", lines.Length));

            // 辞書の作成
            var dic = buildInfoDictionary(lines);

            // プレイヤ
            context.OperationPlayer = int.Parse(dic[SimpleText.Keys.Player]);

            // 操作方向
            context.OperationDirection = SimpleText.ConvertDirection(dic[SimpleText.Keys.Direction]);

            // 経過時間
            context.Time = long.Parse(dic[SimpleText.Keys.Time]);

            // 接地
            context.Ground = Parse(dic[SimpleText.Keys.Ground], bool.Parse);

            // 設置残タイム
            context.BuiltRemainingTime = Parse<int>(dic[SimpleText.Keys.BuiltRemainingTime], int.Parse);

            // 得点
            context.Score = Parse(dic[SimpleText.Keys.Score], long.Parse);

            // 連鎖
            context.Chain = Parse(dic[SimpleText.Keys.Chain], int.Parse);

            // 相殺
            context.Offset = Parse(dic[SimpleText.Keys.Offset], bool.Parse);

            // 全消
            context.AllErase = Parse(dic[SimpleText.Keys.AllErase], bool.Parse);

            // 勝数
            context.WinCount = Parse(dic[SimpleText.Keys.WinCount], int.Parse);

            // 使用スライム
            context.UsingSlimes = ParseUsingSlimes(dic[SimpleText.Keys.UsingSlimes]);

            // おじゃまスライム
            context.ObstructionSlimes = ParseObstructionSlime(dic[SimpleText.Keys.ObstructionSlime]);

            // フィールド
            context.SlimeFields = ParseSlimeFields(dic[SimpleText.Keys.Field]);

            // 移動可能なスライム
            context.MovableSlimes = ParseMovableSlime(dic[SimpleText.Keys.Field]);

            // NEXTスライム
            context.NextSlimes = ParseNextSlimes(dic[SimpleText.Keys.Field]);

            return context;
        }

        /// <summary>
        /// フィールドの情報を示すインデックス
        /// </summary>
        private enum FieldIndex
        {
            /// <summary>
            /// 1Pのフィールド
            /// </summary>
            FirstPlayer = 0,

            /// <summary>
            /// NEXTスライム
            /// </summary>
            NextSlime = 1,

            /// <summary>
            /// 2Pのフィールド
            /// </summary>
            SecondPlayer = 2,
        }

        /// <summary>
        /// 両プレイヤの値を変換します
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="value">値</param>
        /// <param name="parse">変換メソッド</param>
        /// <returns>変換した値の配列</returns>
        private static T[] Parse<T>(string value, Func<string, T> parse)
        {
            var values = value.Split(SimpleText.Separator.Player);
            T[] results = new T[Player.Length];
            results[Player.First] = parse(values[Player.First]);
            results[Player.Second] = parse(values[Player.Second]);
            return results;
        }

        /// <summary>
        /// 使用スライムの変換を行います。
        /// </summary>
        /// <param name="value">使用スライム情報の文字列</param>
        /// <returns>使用スライム</returns>
        private static Slime[] ParseUsingSlimes(string value)
        {
            var result = new Slime[FieldContextConfig.UsingSlimeCount];
            var slimes = value.ToCharArray();
            Debug.Assert(slimes.Count() == FieldContextConfig.UsingSlimeCount, "使用スライムの数が不正です。数：" + slimes.Count());

            for (var i = 0; i < slimes.Count(); i++)
            {
                result[i] = SimpleText.ConvertSlime(slimes[i]);
            }

            return result;
        }

        /// <summary>
        /// スライムごとの配置状態の変換を行います。
        /// </summary>
        /// <param name="value">スライムごとの配置状態の文字列</param>
        /// <returns>スライムごとの配置状態</returns>
        private static Dictionary<Slime, uint[]>[] ParseSlimeFields(string value)
        {
            // 変換を行う
            Func<List<string>, Dictionary<Slime, uint[]>> parse = (lines) =>
            {
                var slimeField = new Dictionary<Slime, uint[]>();
                foreach (Slime slime in ExtensionSlime.Slimes)
                {
                    var shift = 0;
                    var state = 0x0u;
                    var field = new uint[FieldContextConfig.FieldUnitCount];
                    var fieldIndex = 0;
                    for (var i = 0; i < lines.Count(); i++)
                    {
                        var line = lines[i];
                        state |= ConvertStateToUint(slime, line) << (shift * 8);
                        shift++;

                        // フィールドの境目か？
                        if (i % FieldContextConfig.FieldUnitLineCount >= (FieldContextConfig.FieldUnitLineCount - 1))
                        {
                            field[fieldIndex] = state;
                            shift = 0;
                            state = 0x0u;
                            fieldIndex++;
                        }
                    }
                    slimeField.Add(slime, field);
                }

                return slimeField;
            };

            var values = SplitNewLine(value);
            var slimeFields = new Dictionary<Slime, uint[]>[Player.Length];
            for (var player = 0; player < Player.Length; player++)
            {
                var lines = ExtractOnePlayerFieldLines(values, player);
                slimeFields[player] = parse(lines);
            }

            return slimeFields;
        }

        /// <summary>
        /// 移動可能なスライムの変換を行います。
        /// </summary>
        /// <param name="value">移動可能なスライムの文字列</param>
        /// <returns>移動可能なスライムの情報</returns>
        private static MovableSlime[][] ParseMovableSlime(string value)
        {
            // 変換を行う
            Func<List<string>, MovableSlime[]> parse = (lines) =>
            {
                var slimes = new MovableSlime[MovableSlime.Length];
                var movableUnit = MovableSlime.UnitIndex.First;
                var isEnd = false;
                foreach (Slime slime in ExtensionSlime.Slimes)
                {
                    var baseShift = 0;
                    var field = new uint[FieldContextConfig.FieldUnitCount];
                    var fieldIndex = 0;
                    for (var i = 0; i < SimpleText.Length.Field - 1; i++)
                    {
                        var blocks = lines[i].ToCharArray();
                        Debug.Assert((blocks.Length == 6), string.Format("状態の長さが不正です。{0}", blocks.Length));
                        var shift = blocks.Length + 1;
                        for (var j = 0; j < blocks.Length; j++)
                        {
                            if (IsExistsMovableColor(slime, blocks[j]))
                            {
                                var movable = new MovableSlime();
                                movable.Slime = slime;
                                movable.Index = fieldIndex;
                                movable.Position = (baseShift * 8) + shift;
                                slimes[(int)movableUnit] = movable;

                                if (movableUnit == MovableSlime.UnitIndex.Second)
                                {
                                    isEnd = true;
                                    break;
                                }

                                movableUnit = MovableSlime.UnitIndex.Second;
                            }
                            shift--;
                        }

                        if (isEnd) { break; }
                        baseShift++;

                        // フィールドの境目か？
                        if (i % FieldContextConfig.FieldUnitLineCount >= (FieldContextConfig.FieldUnitLineCount - 1))
                        {
                            baseShift = 0;
                            fieldIndex++;
                        }
                    }
                    if (isEnd) { break; }
                }

                // 移動可能スライムの順番整形
                // 横の場合は、2←1とする
                // 縦の場合は、
                // 1
                // ↓
                // 2
                // とする
                var orderdSlimes = slimes
                    .OrderBy(m => m.Index)
                    .ThenBy(m => m.Position)
                    .ToArray();

                return orderdSlimes;
            };

            var values = SplitNewLine(value);
            var movableSlimes = new MovableSlime[Player.Length][];
            for (var player = 0; player < Player.Length; player++)
            {
                var lines = ExtractOnePlayerFieldLines(values, player);
                movableSlimes[player] = parse(lines);
            }

            return movableSlimes;
        }

        /// <summary>
        /// NEXTスライムの変換を行います。
        /// </summary>
        /// <param name="value">フィールドを示す文字列</param>
        /// <returns>NEXTスライムの情報</returns>
        private static Slime[][][] ParseNextSlimes(string value)
        {
            var nextSlimes = new Slime[Player.Length][][];
            Action<int> initialize = (player) =>
            {
                nextSlimes[player] = new Slime[NextSlime.Count][];
                nextSlimes[player][(int)NextSlime.Index.First] = new Slime[MovableSlime.Length];
                nextSlimes[player][(int)NextSlime.Index.Second] = new Slime[MovableSlime.Length];
            };
            initialize(Player.First);
            initialize(Player.Second);

            var values = SplitNewLine(value);
            for (var i = 0; i < values.Length; i++)
            {
                if (!SimpleText.ContainsNextSlimeInfo(i))
                {
                    // NEXTスライム情報が存在しない行なのでスキップ
                    continue;
                }

                var nextSlimeLine = values[i].Split(SimpleText.Separator.Player)[(int)FieldIndex.NextSlime].ToCharArray();
                Debug.Assert(nextSlimeLine.Length == NextSlime.Count, "NEXTスライムの数が不正です。数：" + nextSlimeLine.Length);
                var nextSlimeIndex = SimpleText.ConvertNextSlimeIndex(i);
                var movableSlimeUnitIndex = SimpleText.ConvertMovableSlimeUnitIndex(i);
                nextSlimes[Player.First][(int)nextSlimeIndex][(int)movableSlimeUnitIndex] = SimpleText.ConvertSlime(nextSlimeLine[Player.First]);
                nextSlimes[Player.Second][(int)nextSlimeIndex][(int)movableSlimeUnitIndex] = SimpleText.ConvertSlime(nextSlimeLine[Player.Second]);
            }

            return nextSlimes;
        }

        /// <summary>
        /// 2プレイヤ分のフィールド情報 + NEXTスライム情報の文字列から対象プレイヤのフィールド文字列のみを抽出します。
        /// </summary>
        /// <param name="orgLines">2プレイヤ分のフィールド情報 + NEXTスライム情報の文字列</param>
        /// <param name="player">対象プレイヤ</param>
        /// <returns>対象プレイヤのフィールド文字列</returns>
        private static List<string> ExtractOnePlayerFieldLines(string[] orgLines, int player)
        {
            var targetIndex = player == Player.First ? FieldIndex.FirstPlayer : FieldIndex.SecondPlayer;
            var lines = new List<string>();

            foreach (var orgLine in orgLines)
            {
                // 対象プレイヤのフィールド情報を取得
                var line = orgLine.Split(SimpleText.Separator.Player);
                lines.Add(line[(int)targetIndex]);
            }

            Debug.Assert(lines.Count() == SimpleText.Length.Field - 1, "フィールドの行数が不正です。行数：" + lines.Count());
            return lines;
        }

        /// <summary>
        /// おじゃまスライムの変換を行います。
        /// </summary>
        /// <param name="value">おじゃまスライムの文字列</param>
        /// <returns>変換したおじゃまスライム情報</returns>
        private static Dictionary<ObstructionSlime, int>[] ParseObstructionSlime(string value)
        {
            var result = new Dictionary<ObstructionSlime, int>[Player.Length];

            Func<char[], Dictionary<ObstructionSlime, int>> parse = (vals) =>
            {
                var dic = new Dictionary<ObstructionSlime, int>();
                foreach (var val in vals)
                {
                    var key = SimpleText.ConvertObstructionSlime(val);
                    var count = dic.ContainsKey(key) ? dic[key] + 1 : 1;
                    dic.Add(key, count);
                }
                return dic;
            };

            var values = value.Split(SimpleText.Separator.Player);
            result[Player.First] = parse(values[Player.First].ToCharArray());
            result[Player.Second] = parse(values[Player.Second].ToCharArray());

            return result;
        }

        /// <summary>
        /// 情報の辞書を作成します。
        /// </summary>
        /// <param name="lines">標準テキストファイルの行配列</param>
        /// <returns>情報の辞書</returns>
        private static Dictionary<string, string> buildInfoDictionary(string[] lines)
        {
            var dic = new Dictionary<string, string>();
            var infoIndex = 0;
            var infoLength = -1;
            var isInfoReading = false;
            var sb = new StringBuilder();
            string[] keyValue = null;

            foreach (var line in lines)
            {
                // 情報行読み取り中ではないか？
                if (!isInfoReading)
                {
                    keyValue = line.Split(SimpleText.Separator.KeyValue);
                    infoLength = SimpleText.GetInfoLength(keyValue[0]);

                    if (infoLength == SimpleText.Length.Default)
                    {
                        // 通常は1行にKeyValueがセットされている
                        dic.Add(keyValue[0], keyValue[1]);
                    }
                    else
                    {
                        // 複数行情報
                        isInfoReading = true;
                        infoIndex = 1;
                        sb = new StringBuilder();
                    }
                }
                else
                {
                    if (infoIndex >= infoLength - 1)
                    {
                        // 最終行
                        sb.Append(line);
                    }
                    else
                    {
                        // 最終行以外
                        sb.AppendLine(line);
                    }

                    infoIndex++;
                    if (infoIndex >= infoLength)
                    {
                        // 情報を終端まで読み込んだので、辞書に追加
                        dic.Add(keyValue[0], sb.ToString());
                        isInfoReading = false;
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 指定した状態において対象の色のスライムが存在しているかどうかを判定します。
        /// </summary>
        /// <param name="slime">色</param>
        /// <param name="state">状態</param>
        /// <returns>指定した状態において対象の色のスライムが存在しているかどうか</returns>
        private static bool IsExistsColor(Slime slime, char state)
        {
            return (state == SimpleText.ConvertSlime(slime) || state == SimpleText.ConvertMovableSlime(slime));
        }

        /// <summary>
        /// 指定した状態において対象の色の移動可能スライムが存在しているかどうかを判定します。
        /// </summary>
        /// <param name="slime">色</param>
        /// <param name="state">状態</param>
        /// <returns>指定した状態において対象の色の移動可能スライムが存在しているかどうか</returns>
        private static bool IsExistsMovableColor(Slime slime, char state)
        {
            return (state == SimpleText.ConvertMovableSlime(slime));
        }

        /// <summary>
        /// 状態をuintに変換します。
        /// </summary>
        /// <param name="slime">色</param>
        /// <param name="stateStr">状態を表す文字列</param>
        /// <returns>uintに変換した状態</returns>
        private static uint ConvertStateToUint(Slime slime, string stateStr)
        {
            var state = 0x0u;
            var blocks = stateStr.ToCharArray();
            Debug.Assert((blocks.Length == 6), string.Format("状態の長さが不正です。{0}", blocks.Length));
            var shift = blocks.Length + 1;
            for (var i = 0; i < blocks.Length; i++)
            {
                if (IsExistsColor(slime, blocks[i]))
                {
                    state |= (1u << shift);
                }
                shift--;
            }

            return state;
        }

        /// <summary>
        /// 改行コードで分割した文字列を取得します。
        /// </summary>
        /// <param name="str">分割対象の文字列</param>
        /// <returns>改行コードで分割した文字列</returns>
        private static string[] SplitNewLine(string str)
        {
            return str.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
