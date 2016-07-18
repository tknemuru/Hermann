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
            var direction = context[(int)FieldContext.Command] & Command.DirectionMask;
            var upperCollection = FieldContextExtension.GetCollection(FieldContextExtension.Position.Upper);
            var lowerCollection = FieldContextExtension.GetCollection(FieldContextExtension.Position.Lower);

            switch (direction)
            {
                case Command.DirectionNone :
                    break;
                case Command.DirectionUp :
                    // TODO:あとで実装
                    throw new NotSupportedException();
                case Command.DirectionDown :
                    Move(context, upperCollection, 8);
                    Move(context, lowerCollection, 8);
                    break;
                case Command.DirectionLeft :
                    Move(context, upperCollection, 1);
                    Move(context, lowerCollection, 1);
                    break;
                case Command.DirectionRight :
                    if (IsEnabledRightMove(context[(int)upperCollection.MovableField])) { Move(context, upperCollection, -1); }
                    if (IsEnabledRightMove(context[(int)lowerCollection.MovableField])) { Move(context, lowerCollection, -1); }
                    break;
                default :
                    throw new ApplicationException(string.Format("不正な方向です。{0}", direction));
            }
            return context;
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">フィールドコンテキスト</param>
        /// <param name="collection">コレクション</param>
        /// <param name="shift">シフト量</param>
        private static void Move(ulong[] context, FieldContextCollection collection, int shift)
        {
            foreach (var color in collection.ColorFields)
            {
                Move(context, collection.MovableField, color.Value, shift);
            }

            // 最後に移動
            if (shift > 0)
            {
                context[(int)collection.MovableField] <<= shift;
                context[(int)collection.OccupiedField] <<= shift;
            }
            else
            {
                context[(int)collection.MovableField] >>= (shift * -1);
                context[(int)collection.OccupiedField] >>= (shift * -1);
            }
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">状態</param>
        /// <param name="movableField">操作可能スライムの状態</param>
        /// <param name="colorField">対象色スライムの状態</param>
        /// <param name="shift">シフト量</param>
        private static void Move(ulong[] context, FieldContext movableField, FieldContext colorField, int shift)
        {
            // 対象の色が移動対象外なら処理終了
            if ((context[(int)movableField] & context[(int)colorField]) <= 0ul) { return; }

            var movedColorContext = context[(int)colorField];
            // １．移動前スライムを消す
            movedColorContext = (context[(int)colorField] & ~(context[(int)movableField] & context[(int)colorField]));

            // ２．スライムを移動させる
            if (shift > 0)
            {
                movedColorContext |= ((context[(int)movableField] & context[(int)colorField]) << shift);
            }
            else
            {
                movedColorContext |= ((context[(int)movableField] & context[(int)colorField]) >> (shift * -1));
            }

            // ３．処理が終わった後にセットする
            context[(int)colorField] = movedColorContext;
        }

        /// <summary>
        /// 右に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="field">フィールドの状態</param>
        /// <returns>右に移動可能かどうか</returns>
        private static bool IsEnabledRightMove(ulong field)
        {
            return !((field & 0x0c0c0c0c0c0c0c0cul) > 0);
        }
    }
}
