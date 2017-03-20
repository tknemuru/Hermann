using Hermann.Contexts;
using Hermann.Helpers;
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
        /// 左に動かす際の速度
        /// </summary>
        private const int LeftSpeed = 1;

        /// <summary>
        /// 右に動かす際の速度
        /// </summary>
        private const int RightSpeed = -1;

        /// <summary>
        /// 下に動かす際の速度
        /// </summary>
        private const int DownSpeed = 4 * 8;

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
                    Move(context, ModifyDownShift(context, DownSpeed));
                    break;
                case Command.Direction.Left:
                    if (IsEnabledLeftMove(context))
                    {
                        Move(context, LeftSpeed);
                    }
                    break;
                case Command.Direction.Right:
                    if (IsEnabledRightMove(context))
                    {
                        Move(context, RightSpeed);
                    }
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
            // １．移動前スライムを消す
            foreach (var movable in context.MovableInfos)
            {
                context.SlimeFields[movable.Slime][movable.Index] &= ~(1u << movable.Position);
            }

            // ２．スライムを移動させる
            foreach (var movable in context.MovableInfos)
            {
                var position = movable.Position + shift;
                movable.Index += (position / FieldContextConfig.FieldUnitBitCount);
                movable.Position = position % FieldContextConfig.FieldUnitBitCount;
                context.SlimeFields[movable.Slime][movable.Index] |= 1u << movable.Position;
            }
        }

        /// <summary>
        /// 右に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>右に移動可能かどうか</returns>
        private static bool IsEnabledRightMove(FieldContext context)
        {
            // 判定対象は最右である1つめの移動可能スライムが対象
            var first = context.MovableInfos[(int)MovableUnit.First];

            // 壁を越えないか？
            if (!(((1u << first.Position) & 0xf8f8f8f8u) > 0))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            if (FieldContextHelper.ExistsSlime(context, first.Index, first.Position + RightSpeed))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 左に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>左に移動可能かどうか</returns>
        private static bool IsEnabledLeftMove(FieldContext context)
        {
            // 判定対象は最左である2つめの移動可能スライムが対象
            var shift = context.MovableInfos[(int)MovableUnit.Second].Position;

            return (((1u << shift) & 0x7f7f7f7fu) > 0);
        }

        /// <summary>
        /// 下に移動するシフト量の調整を行います。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="shift">シフト量</param>
        /// <returns></returns>
        private static int ModifyDownShift(FieldContext context, int shift)
        {
            // 判定対象は最下である2つめの移動可能スライムが対象
            var second = context.MovableInfos[(int)MovableUnit.Second];
            var position = second.Position + shift;
            var index = second.Index + (position / FieldContextConfig.FieldUnitBitCount);

            if (index > FieldContextConfig.FieldUnitCount - 1)
            {
                // 底辺を越えている場合は、底辺に着地させる
                shift -= (((position - FieldContextConfig.FieldUnitBitCount) / FieldContextConfig.OneLineBitCount) + 1) * FieldContextConfig.OneLineBitCount;
            }

            return shift;
        }
    }
}
