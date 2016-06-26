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
        public const int First = 0;

        /// <summary>
        /// 2P
        /// </summary>
        public const int Second = 1;

        /// <summary>
        /// プレイヤ数
        /// </summary>
        public const int PlayerCount = 2;

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        public static ulong[] Move(ulong[] context)
        {
            var direction = context[FieldContext.IndexCommand] & Command.DirectionMask;
            switch (direction)
            {
                case Command.DirectionNone :
                    break;
                case Command.DirectionUp :
                    // TODO:あとで実装
                    throw new NotSupportedException();
                case Command.DirectionDown :
                    // 上部
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexBlueFieldUpper, FieldContext.IndexOccupiedFieldUpper, 8);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexRedFieldUpper, FieldContext.IndexOccupiedFieldUpper, 8);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexGreenFieldUpper, FieldContext.IndexOccupiedFieldUpper, 8);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexYellowFieldUpper, FieldContext.IndexOccupiedFieldUpper, 8);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexPurpleFieldUpper, FieldContext.IndexOccupiedFieldUpper, 8);
                    // 下部
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexBlueFieldLower, FieldContext.IndexOccupiedFieldLower, 8);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexRedFieldLower, FieldContext.IndexOccupiedFieldLower, 8);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexGreenFieldLower, FieldContext.IndexOccupiedFieldLower, 8);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexYellowFieldLower, FieldContext.IndexOccupiedFieldLower, 8);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexPurpleFieldLower, FieldContext.IndexOccupiedFieldLower, 8);
                    // 最後に移動
                    context[FieldContext.IndexMovableFieldUpper] <<= 8;
                    break;
                case Command.DirectionLeft :
                    // 上部
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexBlueFieldUpper, FieldContext.IndexOccupiedFieldUpper, 1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexRedFieldUpper, FieldContext.IndexOccupiedFieldUpper, 1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexGreenFieldUpper, FieldContext.IndexOccupiedFieldUpper, 1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexYellowFieldUpper, FieldContext.IndexOccupiedFieldUpper, 1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexPurpleFieldUpper, FieldContext.IndexOccupiedFieldUpper, 1);
                    // 下部
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexBlueFieldLower, FieldContext.IndexOccupiedFieldLower, 1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexRedFieldLower, FieldContext.IndexOccupiedFieldLower, 1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexGreenFieldLower, FieldContext.IndexOccupiedFieldLower, 1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexYellowFieldLower, FieldContext.IndexOccupiedFieldLower, 1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexPurpleFieldLower, FieldContext.IndexOccupiedFieldLower, 1);
                    // 最後に移動
                    context[FieldContext.IndexMovableFieldUpper] <<= 1;
                    break;
                case Command.DirectionRight :
                    // 上部
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexBlueFieldUpper, FieldContext.IndexOccupiedFieldUpper, -1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexRedFieldUpper, FieldContext.IndexOccupiedFieldUpper, -1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexGreenFieldUpper, FieldContext.IndexOccupiedFieldUpper, -1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexYellowFieldUpper, FieldContext.IndexOccupiedFieldUpper, -1);
                    Move(context, FieldContext.IndexMovableFieldUpper, FieldContext.IndexPurpleFieldUpper, FieldContext.IndexOccupiedFieldUpper, -1);
                    // 下部
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexBlueFieldLower, FieldContext.IndexOccupiedFieldLower, -1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexRedFieldLower, FieldContext.IndexOccupiedFieldLower, -1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexGreenFieldLower, FieldContext.IndexOccupiedFieldLower, -1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexYellowFieldLower, FieldContext.IndexOccupiedFieldLower, -1);
                    Move(context, FieldContext.IndexMovableFieldLower, FieldContext.IndexPurpleFieldLower, FieldContext.IndexOccupiedFieldLower, -1);
                    // 最後に移動
                    context[FieldContext.IndexMovableFieldUpper] >>= 1;
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
        private static void Move(ulong[] context, int movableField, int colorField, int occupiedField, int shift)
        {
            if ((context[movableField] & context[colorField]) > 0ul)
            {
                var movedColorContext = context[colorField];
                // １．移動前スライムを消す
                movedColorContext = (context[colorField] & ~(context[movableField] & context[colorField]));
                context[occupiedField] &= ~(context[movableField] & context[colorField]);

                // ２．スライムを移動させる
                if (shift > 0)
                {
                    movedColorContext |= ((context[movableField] & context[colorField]) << shift);
                    context[occupiedField] |= ((context[movableField] & context[colorField]) << shift);
                }
                else
                {
                    movedColorContext |= ((context[movableField] & context[colorField]) >> (shift * -1));
                    context[occupiedField] |= ((context[movableField] & context[colorField]) >> (shift * -1));
                }
                
                // ３．処理が終わった後にセットする
                context[colorField] = movedColorContext;
            }
        }
    }
}
