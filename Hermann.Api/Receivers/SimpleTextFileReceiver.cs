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
            ulong[] context = new ulong[FieldContext.ElementCount];
            var lines = FileHelper.ReadTextLines(source).ToArray();
            Debug.Assert((lines.Length == SimpleText.LineCount), string.Format("テキストファイルの行数が不正です。{0}", lines.Length));
            ulong command = 0x0;

            // プレイヤ
            command |= ulong.Parse(lines[SimpleText.LineIndexPlayer]);

            // 操作方向
            switch (lines[SimpleText.LineIndexDirection])
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
            context[FieldContext.IndexCommand] = command;

            // 状態
            // 上部
            var occupiedUpper = 0ul;
            var movableUpper = 0ul;
            int shift = 0;
            for (var i = 2; i < 11; i++)
            {
                occupiedUpper |= (ConvertStateToUlong(lines[i], IsOccupied) << (shift * 8));
                movableUpper |= (ConvertStateToUlong(lines[i], IsExistsMovableSlime) << (shift * 8));
                shift++;
            }
            context[FieldContext.IndexOccupiedFieldUpper] = occupiedUpper;
            context[FieldContext.IndexMovableFieldUpper] = movableUpper;

            // 下部
            var occupiedLower = 0ul;
            var movableLower = 0ul;
            shift = 0;
            for (var i = 12; i < lines.Length; i++)
            {
                occupiedLower |= (ConvertStateToUlong(lines[i], IsOccupied) << (shift * 8));
                movableLower |= (ConvertStateToUlong(lines[i], IsExistsMovableSlime) << (shift * 8));
                shift++;
            }
            context[FieldContext.IndexOccupiedFieldLower] = occupiedLower;
            context[FieldContext.IndexMovableFieldLower] = movableLower;

            return context;
        }

        /// <summary>
        /// 状態をulongに変換します。
        /// </summary>
        /// <param name="stateStr">状態を表す文字列</param>
        /// <param name="isStateOn">状態がOnかどうかを判定するメソッド</param>
        /// <returns>ulongに変換した状態</returns>
        private static ulong ConvertStateToUlong(string stateStr, Func<char, bool> isStateOn)
        {
            var state = 0x0ul;
            var blocks = stateStr.ToCharArray();
            Debug.Assert((blocks.Length == 6), string.Format("状態の長さが不正です。{0}", blocks.Length));
            var shift = blocks.Length + 1;
            for (var i = 0; i < blocks.Length; i++)
            {
                if(isStateOn(blocks[i]))
                {
                    state |= (1ul << shift);
                }
                shift--;
            }

            return state;
        }

        /// <summary>
        /// 占有状態になっているかを判定します。
        /// </summary>
        /// <param name="state">状態</param>
        /// <returns>占有状態になっているかどうか</returns>
        private static bool IsOccupied(char state)
        {
            var isOccupied = false;
            switch (state)
            {
                case SimpleText.SlimeNone:
                    break;
                case SimpleText.SlimeBlue:
                case SimpleText.SlimeGreen:
                case SimpleText.SlimePurple:
                case SimpleText.SlimeRed:
                case SimpleText.SlimeYellow:
                case SimpleText.SlimeObstruction:
                case SimpleText.MovableSlimeBlue:
                case SimpleText.MovableSlimeGreen:
                case SimpleText.MovableSlimePurple:
                case SimpleText.MovableSlimeRed:
                case SimpleText.MovableSlimeYellow:
                    isOccupied = true;
                    break;
                default:
                    throw new ApplicationException(string.Format("不正な状態です(占有状態)。{0}", state));
            }

            return isOccupied;
        }

        /// <summary>
        /// 操作可能スライムが存在しているかを判定します。
        /// </summary>
        /// <param name="state">状態</param>
        /// <returns>操作可能スライムが存在しているかどうか</returns>
        private static bool IsExistsMovableSlime(char state)
        {
            var isExistsMovableSlime = false;
            switch (state)
            {
                case SimpleText.SlimeNone:
                case SimpleText.SlimeBlue:
                case SimpleText.SlimeGreen:
                case SimpleText.SlimePurple:
                case SimpleText.SlimeRed:
                case SimpleText.SlimeYellow:
                case SimpleText.SlimeObstruction:
                    break;
                case SimpleText.MovableSlimeBlue:
                case SimpleText.MovableSlimeGreen:
                case SimpleText.MovableSlimePurple:
                case SimpleText.MovableSlimeRed:
                case SimpleText.MovableSlimeYellow:
                    isExistsMovableSlime = true;
                    break;
                default:
                    throw new ApplicationException(string.Format("不正な状態です(操作対象状態)。{0}", state));
            }

            return isExistsMovableSlime;
        }
    }
}
