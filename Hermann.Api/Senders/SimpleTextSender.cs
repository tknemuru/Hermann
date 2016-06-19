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
    public class SimpleTextSender : Sender
    {
        /// <summary>
        /// 状態を文字列に変換し送信します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string Send(ulong[] context)
        {
            var sb = new StringBuilder();
            var command = context[0];
            var player = command & Command.PlayerMask;
            sb.AppendLine(player.ToString());
            var direction = command & Command.DirectionMask;
            sb.AppendLine(ConvertToDirection(direction));

            // フィールド上部
            AddField(sb, context[1], (index => false));

            // フィールド下部
            AddField(sb, context[2], (index => index > 3));

            return sb.ToString();
        }

        /// <summary>
        /// ulongから方向の文字列に変換します。
        /// </summary>
        /// <param name="direction">ulongの方向</param>
        /// <returns>方向の文字列</returns>
        private static string ConvertToDirection(ulong direction)
        {
            switch (direction)
            {
                case Command.DirectionNone :
                    return SimpleText.DirectionNone;
                case Command.DirectionUp :
                    return SimpleText.DirectionUp;
                case Command.DirectionDown:
                    return SimpleText.DirectionDown;
                case Command.DirectionLeft:
                    return SimpleText.DirectionLeft;
                case Command.DirectionRight:
                    return SimpleText.DirectionRight;
                default :
                    throw new ApplicationException(string.Format("不正な方向です。{0}", direction));
            }
        }

        /// <summary>
        /// フィールド情報を与えられたStringBuilderに追加します。
        /// </summary>
        /// <param name="sb">コンテキストを表すStringBuilder</param>
        /// <param name="field">フィールド</param>
        /// <param name="isBreak">繰り返し完了を判定するメソッド</param>
        private static void AddField(StringBuilder sb, ulong field, Func<int, bool> isBreak)
        {
            string line = string.Empty;
            for (var i = 0; i < 8; i++)
            {
                // 条件に合致したら終了
                if (isBreak(i)) { break; }

                var lineByte = (Byte)((field >> (i * 8)) & 0xff);
                var digits = Convert.ToString(lineByte, 2).PadLeft(8, '0').Reverse().ToArray();
                Debug.Assert((digits.Length == 8), string.Format("digitsの桁数が不正です。{0},{1}", digits, digits.Length));

                for (var s = 0; s < digits.Length; s++)
                {
                    // フィールド外は除外
                    if ((s % 8) < 2) { continue; }

                    if (digits[s] == '1')
                    {
                        line = SimpleText.SlimeBlue.ToString() + line;
                    }
                    else
                    {
                        line = SimpleText.SlimeNone.ToString() + line;
                    }

                    // 改行
                    if ((s % 8) == 7)
                    {
                        sb.AppendLine(line);
                        line = string.Empty;
                    }
                }
            }
        }
    }
}
