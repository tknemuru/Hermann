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
            sb.AppendLine(context.OperationPlayer.ToString());

            // 操作方向
            sb.AppendLine(SimpleText.ConvertDirection(context.OperationDirection));

            // フィールド
            var firstFieldList = CreateFieldList(context, Player.First);
            var secondFieldList = CreateFieldList(context, Player.Second);
            var field = MergeFieldList(firstFieldList, secondFieldList);

            return sb.ToString() + field.ToString();
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
                possibilityOfExistsMovableUnit = isExistsMovableUnit(context, player, unitIndex);
                for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
                {
                    if (possibilityOfExistsMovableUnit)
                    {
                        possibilityOfExistsMovablePosition = isExistsMovablePosition(context, player, i);
                    }

                    // フィールド外は除外
                    if ((i % 8) < 2) { continue; }

                    bool isExists = ExtensionSlime.Slimes.Any(slime =>
                    {
                        var exists = (slimeFields[slime][unitIndex] & (1ul << i)) > 0u;
                        if (exists)
                        {
                            line = (possibilityOfExistsMovablePosition && isExsitsMovableColor(context, player, slime, unitIndex, i)) ? SimpleText.ConvertMovableSlime(slime) + line : SimpleText.ConvertSlime(slime) + line;
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
                        fieldList.Add(line);
                        line = string.Empty;
                    }
                }
            }

            return fieldList;
        }

        /// <summary>
        /// 指定されたフィールドのユニットに移動可能なスライムが存在するかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="unitIndex">判定対象のフィールドユニットのインデックス</param>
        /// <returns>指定されたフィールドのユニットに移動可能なスライムが存在するかどうか</returns>
        private static bool isExistsMovableUnit(FieldContext context, int player, int unitIndex)
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
        private static bool isExistsMovablePosition(FieldContext context, int player, int position)
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
        private static bool isExsitsMovableColor(FieldContext context, int player, Slime slime, int unitIndex, int position)
        {
            return context.MovableSlimes[player].Any(m => m.Slime == slime && m.Index == unitIndex && m.Position == position);
        }

        /// <summary>
        /// フィールド情報の文字列リストをマージします。
        /// </summary>
        /// <param name="first">1Pのフィールド情報文字列リスト</param>
        /// <param name="second">2Pのフィールド情報文字列リスト</param>
        /// <returns>マージしたフィールド情報文字列リスト</returns>
        private static StringBuilder MergeFieldList(List<string> first, List<string> second)
        {
            Debug.Assert((first.Count == second.Count), string.Format("firstとsecondの要素数が等しくありません。first:{0} second:{1}", first.Count, second.Count));
            var sb = new StringBuilder();
            for (var i = 0; i < first.Count; i++)
            {
                sb.AppendLine(first[i] + SimpleText.FieldSeparator + second[i]);
            }
            return sb;
        }
    }
}
