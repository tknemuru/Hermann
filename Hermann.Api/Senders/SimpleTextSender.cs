using Hermann.Contexts;
using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hermann.Api.Senders
{
    /// <summary>
    /// 標準テキスト形式文字列の送信機能を提供します。
    /// </summary>
    public class SimpleTextSender : ISendable<string>
    {
        /// <summary>
        /// 状態を文字列に変換し送信します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>フィールドの状態を表した文字列</returns>
        public string Send(FieldContext context)
        {
            var sb = new StringBuilder();

            // プレイヤ
            sb.AppendLine(ToString(SimpleText.Keys.Player, context.OperationPlayer));

            // 操作方向
            sb.AppendLine(ToString(SimpleText.Keys.Direction, SimpleText.ConvertDirection(context.OperationDirection)));

            // 経過時間
            sb.AppendLine(ToString(SimpleText.Keys.Time, context.Time));

            // 接地
            sb.AppendLine(ArrayToString(SimpleText.Keys.Ground, context.Ground));

            // 設置残タイム
            sb.AppendLine(ArrayToString(SimpleText.Keys.BuiltRemainingTime, context.BuiltRemainingTime));

            // 得点
            sb.AppendLine(ArrayToString(SimpleText.Keys.Score, context.Score));

            // 連鎖
            sb.AppendLine(ArrayToString(SimpleText.Keys.Chain, context.Chain));

            // 相殺
            sb.AppendLine(ArrayToString(SimpleText.Keys.Offset, context.Offset));

            // 全消
            sb.AppendLine(ArrayToString(SimpleText.Keys.AllErase, context.AllErase));

            // 勝数
            sb.AppendLine(ArrayToString(SimpleText.Keys.WinCount, context.WinCount));

            // 使用スライム
            sb.AppendLine(UsingSlimesToString(context.UsingSlimes));

            // おじゃまスライム
            sb.AppendLine(ObstructionSlimesToString(context.ObstructionSlimes));

            // フィールド
            sb.Append(FieldToString(context));

            return sb.ToString();
        }

        /// <summary>
        /// 情報を文字列に変換します。
        /// </summary>
        /// <param name="key">情報のキー</param>
        /// <param name="value">情報の値</param>
        /// <returns>情報の文字列</returns>
        private static string ToString(string key, object value)
        {
            return string.Format("{0}{1}{2}", key, SimpleText.Separator.KeyValue, value.ToString().ToLower());
        }

        /// <summary>
        /// 情報の配列を文字列に変換します。
        /// </summary>
        /// <param name="key">情報のキー</param>
        /// <param name="value">情報の値（配列）</param>
        /// <returns>情報の文字列</returns>
        private static string ArrayToString<T>(string key, T[] value)
        {
            Debug.Assert(value.Length == Player.Length, "値の要素数が不正です。要素数：" + value.Length);
            return string.Format("{0}{1}{2}{3}{4}", key, SimpleText.Separator.KeyValue, value[Player.First].ToString().ToLower(), SimpleText.Separator.Player, value[Player.Second].ToString().ToLower());
        }

        /// <summary>
        /// 使用スライムの情報を文字列に変換します。
        /// </summary>
        /// <param name="value">使用スライムの情報</param>
        /// <returns>使用スライムの情報の文字列</returns>
        private static string UsingSlimesToString(Slime[] value)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("{0}{1}", SimpleText.Keys.UsingSlimes, SimpleText.Separator.KeyValue));

            for(var i = 0; i < value.Count(); i++)
            {
                sb.Append(SimpleText.ConvertSlime(value[i]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// おじゃまスライムの情報を文字列に変換します。
        /// </summary>
        /// <param name="value">おじゃまスライムの情報</param>
        /// <returns>おじゃまスライムの情報の文字列</returns>
        private static string ObstructionSlimesToString(Dictionary<ObstructionSlime, int>[] value)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("{0}{1}", SimpleText.Keys.ObstructionSlime, SimpleText.Separator.KeyValue));

            Action<int> toString = (player) =>
            {
                foreach (var ob in value[player])
                {
                    for (var i = 0; i < ob.Value; i++)
                    {
                        sb.Append(SimpleText.ConvertObstructionSlime(ob.Key));
                    }
                }
            };

            toString(Player.First);
            sb.Append(SimpleText.Separator.Player);
            toString(Player.Second);

            return sb.ToString();
        }

        /// <summary>
        /// フィールド情報を文字列に変換します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>フィールド情報の文字列</returns>
        private static string FieldToString(FieldContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("{0}{1}", SimpleText.Keys.Field, SimpleText.Separator.KeyValue));

            var firstFieldList = CreateFieldList(context, Player.First);
            var secondFieldList = CreateFieldList(context, Player.Second);
            var nextSlimeList = CreateNextSlimeList(context.NextSlimes);
            var field = MergeFieldList(firstFieldList, secondFieldList, nextSlimeList);
            sb.Append(field.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// フィールド情報の文字列リストを作成します。
        /// </summary>
        /// <param name="field">フィールド</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="isBreak">繰り返し完了を判定するメソッド</param>
        private static List<string> CreateFieldList(FieldContext context, int player)
        {
            var fieldList = new List<string>();
            string line = string.Empty;
            var possibilityOfExistsMovableUnit = true;
            var possibilityOfExistsMovablePosition = true;
            var slimeFields = context.SlimeFields[player];
            for (var unitIndex = 0; unitIndex < FieldContextConfig.FieldUnitCount; unitIndex++)
            {
                possibilityOfExistsMovableUnit = IsExistsMovableUnit(context, player, unitIndex);
                for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
                {
                    if (possibilityOfExistsMovableUnit)
                    {
                        possibilityOfExistsMovablePosition = IsExistsMovablePosition(context, player, i);
                    }

                    // フィールド外は除外
                    if ((i % 8) < 2) { continue; }

                    bool isExists = ExtensionSlime.Slimes.Any(slime =>
                    {
                        var exists = (slimeFields[slime][unitIndex] & (1ul << i)) > 0u;
                        if (exists)
                        {
                            line = (possibilityOfExistsMovablePosition && IsExsitsMovableColor(context, player, slime, unitIndex, i)) ? SimpleText.ConvertMovableSlime(slime) + line : SimpleText.ConvertSlime(slime) + line;
                        }
                        return exists;
                    });

                    if (!isExists)
                    {
                        line = ((FieldContextConfig.HiddenUnitIndex == unitIndex) ? SimpleText.SlimeSymbol.NoneHidden : SimpleText.SlimeSymbol.None) + line;
                    }

                    // 改行
                    if ((i % 8) == 7)
                    {
                        fieldList.Add(line);
                        line = string.Empty;
                    }
                }
            }

            return fieldList;
        }

        /// <summary>
        /// NEXTスライム情報の文字列リストを作成します。
        /// </summary>
        /// <param name="nextSlimes">NEXTスライム情報</param>
        /// <returns>NEXTスライム情報の文字列リスト</returns>
        private static List<string> CreateNextSlimeList(Slime[][][] nextSlimes)
        {
            var list = new List<string>();
            for (var i = 0; i < FieldContextConfig.FieldLineCount; i++)
            {
                if (!SimpleText.ContainsNextSlimeInfo(i))
                {
                    // NEXTスライムの情報が存在しない行
                    list.Add("　　");
                    continue;
                }

                var nextSlimeIndex = SimpleText.ConvertNextSlimeIndex(i);
                var movableSlimeUnitIndex = SimpleText.ConvertMovableSlimeUnitIndex(i);
                var first = SimpleText.ConvertSlime(nextSlimes[Player.First][(int)nextSlimeIndex][(int)movableSlimeUnitIndex]);
                var second = SimpleText.ConvertSlime(nextSlimes[Player.Second][(int)nextSlimeIndex][(int)movableSlimeUnitIndex]);
                list.Add(string.Format("{0}{1}", first, second));
            }

            return list;
        }

        /// <summary>
        /// 指定されたフィールドのユニットに移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="unitIndex">判定対象のフィールドユニットのインデックス</param>
        /// <returns>指定されたフィールドのユニットに移動可能なスライムが存在するかどうか</returns>
        private static bool IsExistsMovableUnit(FieldContext context, int player, int unitIndex)
        {
            return context.MovableSlimes[player].Any(m => m.Index == unitIndex);
        }

        /// <summary>
        /// 指定されたポジションに移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="position">判定対象のポジション</param>
        /// <returns>指定されたフィールドのポジションに移動可能なスライムが存在するかどうか</returns>
        private static bool IsExistsMovablePosition(FieldContext context, int player, int position)
        {
            return context.MovableSlimes[player].Any(m => m.Position == position);
        }

        /// <summary>
        /// 指定された場所に移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="slime">スライム</param>
        /// <param name="unitIndex">フィールド単位のインデックス</param>
        /// <param name="position">ポジション</param>
        /// <returns>指定された場所に移動可能なスライムが存在するかどうか</returns>
        private static bool IsExsitsMovableColor(FieldContext context, int player, Slime slime, int unitIndex, int position)
        {
            return context.MovableSlimes[player].Any(m => m.Slime == slime && m.Index == unitIndex && m.Position == position);
        }

        /// <summary>
        /// フィールド情報の文字列リストをマージします。
        /// </summary>
        /// <param name="first">1Pのフィールド情報文字列リスト</param>
        /// <param name="second">2Pのフィールド情報文字列リスト</param>
        /// <param name="nextSlime">NEXTスライム情報文字列リスト</param>
        /// <returns>マージしたフィールド情報文字列リスト</returns>
        private static StringBuilder MergeFieldList(List<string> first, List<string> second, List<string> nextSlime)
        {
            Debug.Assert((first.Count == FieldContextConfig.FieldLineCount), string.Format("firstの要素数が正しくありません。first:{0}", first.Count()));
            Debug.Assert((first.Count == second.Count), string.Format("firstとsecondの要素数が等しくありません。first:{0} second:{1}", first.Count(), second.Count()));
            Debug.Assert((first.Count == nextSlime.Count), string.Format("firstとnextSlimeの要素数が等しくありません。first:{0} nextSlime:{1}", first.Count(), nextSlime.Count()));
            var sb = new StringBuilder();
            for (var i = 0; i < first.Count; i++)
            {
                sb.AppendLine(string.Format("{0}{1}{2}{3}{4}", first[i], SimpleText.Separator.Player, nextSlime[i], SimpleText.Separator.Player, second[i]));
            }
            return sb;
        }
    }
}
