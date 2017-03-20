using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// プレイヤ
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// 1P
        /// </summary>
        public const uint First = 0;

        /// <summary>
        /// 2P
        /// </summary>
        public const uint Second = 1;

        /// <summary>
        /// プレイヤ数
        /// </summary>
        public const int PlayerCount = 2;

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        public static FieldContext Move(FieldContext context)
        {
            var direction = Command.GetDirection(context.Command);

            switch (direction)
            {
                case Command.Direction.None :
                    break;
                case Command.Direction.Up:
                    // TODO:あとで実装
                    throw new NotSupportedException();
                case Command.Direction.Down:
                    Move(context, 8);
                    break;
                case Command.Direction.Left:
                    Move(context, 1);
                    break;
                case Command.Direction.Right:
                    Move(context, -1);
                    break;
                default :
                    throw new ApplicationException(string.Format("不正な方向です。{0}", direction));
            }
            return context;
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">状態</param>
        /// <param name="movableField">操作可能スライムの状態</param>
        /// <param name="colorField">対象色スライムの状態</param>
        /// <param name="shift">シフト量</param>
        private static void Move(FieldContext context, int shift)
        {
            foreach (var movable in context.MovableInfos)
            {
                // １．移動可能か判定
                if (!IsEnabledRightMove(context.SlimeFields[movable.Slime][movable.Index]))
                {
                    continue;
                }

                // ２．スライムを移動させる
                var index = movable.Index;
                var position = movable.Position;
                if((movable.Position + shift) > 0)
                {
                    context.SlimeFields[movable.Slime][index] |= 1u << (position + shift);
                    movable.Position = (position + shift);
                }
                
                // ３．移動前スライムを消す
                context.SlimeFields[movable.Slime][index] &= ~(1u << position);
            }
        }

        /// <summary>
        /// 右に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="field">フィールドの状態</param>
        /// <returns>右に移動可能かどうか</returns>
        private static bool IsEnabledRightMove(uint field)
        {
            return !((field & 0x0c0c0c0c0c0c0c0cul) > 0);
        }
    }
}
