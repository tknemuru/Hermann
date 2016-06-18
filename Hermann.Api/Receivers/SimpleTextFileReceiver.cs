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
                case "無" :
                    command |= 0x0;
                    break;
                case "上" :
                    command |= 0x2;
                    break;
                case "下":
                    command |= 0x3;
                    break;
                case "左":
                    command |= 0x4;
                    break;
                case "右":
                    command |= 0x5;
                    break;
                default :
                    throw new ApplicationException(string.Format("不正な操作コマンドです。{0}", lines[1]));
            }
            context[0] = command;

            // 状態
            ulong upper = 0x0;
            for (var i = 2; i < 11; i++)
            {
                upper |= ConvertStateToUlong(lines[i]);
            }
            context[1] = upper;

            ulong lower = 0x0;
            for (var i = 12; i < lines.Length; i++)
            {
                lower |= ConvertStateToUlong(lines[i]);
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
            for(var i = 0; i < blocks.Length; i++)
            {
                switch (blocks[i])
                {
                    case 'ロ' :
                        break;
                    case '赤' :
                    case '青' :
                    case '黄' :
                    case '紫' :
                    case '緑' :
                    case 'お' :
                        state |= (1ul << i);
                        break;
                    default :
                        throw new ApplicationException(string.Format("不正な状態です。{0}", blocks[i]));
                }
            }

            return state;
        }
    }
}
