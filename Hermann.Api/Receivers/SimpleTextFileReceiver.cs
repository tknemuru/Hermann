using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Collections;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// Hermannにおける標準テキストファイルの受信機能を提供します。
    /// </summary>
    public class SimpleTextFileReceiver : Receiver
    {
        /// <summary>
        /// 標準テキストファイルを受信しフィールドの状態に変換します。
        /// </summary>
        /// <param name="source">標準テキストファイルパス</param>
        /// <returns>フィールドの状態</returns>
        public override ulong[] Receive(string source)
        {
            ulong[] context = new ulong[3];
            var lines = FileHelper.ReadTextLines(source).ToArray();
            Debug.Assert((lines.Length == 14), string.Format("テキストファイルの行数が不正です。{0}", lines.Length));
            ulong command = 0x0;

            // プレイヤ
            command |= ulong.Parse(lines[0]);

            // 操作
            switch (lines[1])
            {
                case SimpleText.DirectionNone :
                    command |= Command.DirectionNone;
                    break;
                case SimpleText.DirectionUp :
                    command |= Command.DirectionUp;
                    break;
                case SimpleText.DirectionDown :
                    command |= Command.DirectionDown;
                    break;
                case SimpleText.DirectionLeft :
                    command |= Command.DirectionLeft;
                    break;
                case SimpleText.DirectionRight :
                    command |= Command.DirectionRight;
                    break;
                default :
                    throw new ApplicationException(string.Format("不正な操作コマンドです。{0}", lines[1]));
            }
            context[0] = command;

            // 状態
            ulong upper = 0x0;
            int shift = 0;
            for (var i = 2; i < 11; i++)
            {
                upper |= (ConvertStateToUlong(lines[i]) << (shift * 8));
                shift++;
            }
            context[1] = upper;

            ulong lower = 0x0;
            shift = 0;
            for (var i = 12; i < lines.Length; i++)
            {
                lower |= (ConvertStateToUlong(lines[i]) << (shift * 8));
                shift++;
            }
            context[2] = lower;

            return context;
        }

        /// <summary>
        /// 状態をulongに変換します。
        /// </summary>
        /// <param name="stateStr">状態を表す文字列</param>
        /// <returns>ulongに変換した状態</returns>
        private static ulong ConvertStateToUlong(string stateStr)
        {
            ulong state = 0x0;
            var blocks = stateStr.ToCharArray();
            Debug.Assert((blocks.Length == 6), string.Format("状態の長さが不正です。{0}", blocks.Length));
            var shift = blocks.Length + 1;
            for (var i = 0; i < blocks.Length; i++)
            {
                switch (blocks[i])
                {
                    case SimpleText.SlimeNone :
                        break;
                    case SimpleText.SlimeBlue :
                    case SimpleText.SlimeGreen :
                    case SimpleText.SlimePurple :
                    case SimpleText.SlimeRed :
                    case SimpleText.SlimeYellow :
                    case SimpleText.SlimeObstruction :
                        state |= (1ul << shift);
                        break;
                    default :
                        throw new ApplicationException(string.Format("不正な状態です。{0}", blocks[i]));
                }
                shift--;
            }

            return state;
        }
    }
}
