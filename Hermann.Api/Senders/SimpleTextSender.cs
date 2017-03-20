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
            var command = context.Command;
            var player = Command.GetPlayer(command);
            sb.AppendLine(player.ToString());
            var direction = Command.GetDirection(command);
            sb.AppendLine(SimpleText.ConvertDirection(direction));

            // フィールド
            AddField(sb, context);

            return sb.ToString();
        }

        /// <summary>
        /// フィールド情報をStringBuilderに追加します。
        /// </summary>
        /// <param name="sb">コンテキストを表すStringBuilder</param>
        /// <param name="field">フィールド</param>
        /// <param name="isBreak">繰り返し完了を判定するメソッド</param>
        private static void AddField(StringBuilder sb, FieldContext context)
        {
            string line = string.Empty;
            var possibilityOfExistsMovableUnit = true;
            var possibilityOfExistsMovablePosition = true;
            for (var unitIndex = 0; unitIndex < FieldContextConfig.FieldUnitCount; unitIndex++)
            {
                possibilityOfExistsMovableUnit = isExistsMovableUnit(context, unitIndex);
                for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
                {
                    if (possibilityOfExistsMovableUnit)
                    {
                        possibilityOfExistsMovablePosition = isExistsMovablePosition(context, i);
                    }

                    // フィールド外は除外
                    if ((i % 8) < 2) { continue; }

                    bool isExists = ExtensionSlime.Slimes.Any(slime =>
                    {
                        var exists = (context.SlimeFields[slime][unitIndex] & (1ul << i)) > 0u;
                        if (exists)
                        {
                            line = (possibilityOfExistsMovablePosition && isExsitsMovableColor(context, slime, unitIndex, i)) ? SimpleText.ConvertMovableSlime(slime) + line : SimpleText.ConvertSlime(slime) + line;
                        }
                        return exists;
                    });

                    if (!isExists)
                    {
                        line = SimpleText.SlimeNone.ToString() + line;
                    }

                    // 改行
                    if ((i % 8) == 7)
                    {
                        sb.AppendLine(line);
                        line = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 指定されたフィールドのユニットに移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="unitIndex">判定対象のフィールドユニットのインデックス</param>
        /// <returns>指定されたフィールドのユニットに移動可能なスライムが存在するかどうか</returns>
        private static bool isExistsMovableUnit(FieldContext context, int unitIndex)
        {
            return context.MovableInfos.Any(m => m.Index == unitIndex);
        }

        /// <summary>
        /// 指定されたポジションに移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="position">判定対象のポジション</param>
        /// <returns>指定されたフィールドのポジションに移動可能なスライムが存在するかどうか</returns>
        private static bool isExistsMovablePosition(FieldContext context, int position)
        {
            return context.MovableInfos.Any(m => m.Position == position);
        }

        /// <summary>
        /// 指定された場所に移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="slime">スライム</param>
        /// <param name="unitIndex">フィールド単位のインデックス</param>
        /// <param name="position">ポジション</param>
        /// <returns>指定された場所に移動可能なスライムが存在するかどうか</returns>
        private static bool isExsitsMovableColor(FieldContext context, Slime slime, int unitIndex, int position)
        {
            return context.MovableInfos.Any(m => m.Slime == slime && m.Index == unitIndex && m.Position == position);
        }
    }
}
