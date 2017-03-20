using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Contexts;
using Hermann.Collections;

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
            Debug.Assert((lines.Length == SimpleText.LineCount), string.Format("テキストファイルの行数が不正です。{0}", lines.Length));

            // プレイヤ
            context.OperationPlayer = int.Parse(lines[(int)SimpleText.Lines.Player]);

            // 操作方向
            context.OperationDirection = (Direction)SimpleText.ConvertDirection(lines[(int)SimpleText.Lines.Direction]);

            // 状態
            for (var player = Player.First; player < Player.Length; player++)
            {
                var slimeFiels = context.SlimeFields[player];
                foreach (Slime slime in ExtensionSlime.Slimes)
                {
                    var shift = 0;
                    var state = 0x0u;
                    var field = new uint[FieldContextConfig.FieldUnitCount];
                    var fieldIndex = 0;
                    for (var i = (int)SimpleText.Lines.FieldStart; i < SimpleText.LineCount; i++)
                    {
                        var line = lines[i];
                        state |= ConvertStateToUint(slime, line, player) << (shift * 8);
                        shift++;

                        // フィールドの境目か？
                        if ((i - (int)SimpleText.Lines.FieldStart) % FieldContextConfig.FieldLineCount >= (FieldContextConfig.FieldLineCount - 1))
                        {
                            field[fieldIndex] = state;
                            shift = 0;
                            state = 0x0u;
                            fieldIndex++;
                        }
                    }
                    slimeFiels.Add(slime, field);
                }

                // 移動可能なスライム
                var movableSlimes = context.MovableSlimes[player];
                var movableUnit = MovableSlimeUnit.Index.First;
                var isEnd = false;
                foreach (Slime slime in ExtensionSlime.Slimes)
                {
                    var baseShift = 0;
                    var field = new uint[FieldContextConfig.FieldUnitCount];
                    var fieldIndex = 0;
                    for (var i = (int)SimpleText.Lines.FieldStart; i < SimpleText.LineCount; i++)
                    {
                        var line = lines[i].Split(SimpleText.FieldSeparator)[player];
                        var blocks = line.ToCharArray();
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
                                movableSlimes[(int)movableUnit] = movable;

                                if (movableUnit == MovableSlimeUnit.Index.Second)
                                {
                                    isEnd = true;
                                    break;
                                }

                                movableUnit = MovableSlimeUnit.Index.Second;
                            }
                            shift--;
                        }

                        if (isEnd) { break; }
                        baseShift++;

                        // フィールドの境目か？
                        if ((i - (int)SimpleText.Lines.FieldStart) % FieldContextConfig.FieldLineCount >= (FieldContextConfig.FieldLineCount - 1))
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
                context.MovableSlimes[player] = movableSlimes
                    .OrderBy(m => m.Index)
                    .ThenBy(m => m.Position)
                    .ToArray();
            }

            return context;
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
        /// <param name="player">変換対象のプレイヤ</param>
        /// <returns>uintに変換した状態</returns>
        private static uint ConvertStateToUint(Slime slime, string stateStr, int player)
        {
            var state = 0x0u;
            var stateStrs = stateStr.Split(SimpleText.FieldSeparator);
            Debug.Assert(stateStrs.Length == Player.Length, string.Format("フィールドの数が不正です。{0}", stateStrs.Length));
            var blocks = stateStrs[player].ToCharArray();
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
    }
}
