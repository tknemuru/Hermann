﻿using Hermann.Contexts;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            foreach (var movable in context.MovableSlimes)
            {
                context.SlimeFields[movable.Slime][movable.Index] &= ~(1u << movable.Position);
            }

            // ２．スライムを移動させる
            foreach (var movable in context.MovableSlimes)
            {
                var position = movable.Position + shift;
                movable.Index += (position / FieldContextConfig.FieldUnitBitCount);
                movable.Position = position % FieldContextConfig.FieldUnitBitCount;
                Debug.Assert(!FieldContextHelper.ExistsSlime(context, movable.Index, movable.Position), string.Format("他のスライムが移動場所に存在しています。 Index : {0} Position : {1}", movable.Index, movable.Position));
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
            var first = context.MovableSlimes[(int)MovableSlimeUnit.Index.First];
            var second = context.MovableSlimes[(int)MovableSlimeUnit.Index.Second];

            // 壁を越えないか？
            // 壁越えチェック対象は最右である1つめの移動可能スライムが対象
            if (!(((1u << first.Position) & 0xf8f8f8f8u) > 0))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            // 縦向きの場合は最下である2つめの移動可能スライムが対象
            var form = MovableSlimeUnit.GetForm(context.MovableSlimes);
            if (form == MovableSlimeUnit.Form.Vertical &&
                FieldContextHelper.ExistsSlime(context, second.Index, second.Position + RightSpeed))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            // 横向きの場合は最右である1つめの移動可能スライムが対象
            if (form == MovableSlimeUnit.Form.Horizontal &&
                FieldContextHelper.ExistsSlime(context, first.Index, first.Position + RightSpeed))
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
            // 判定対象は最左・最下である2つめの移動可能スライムが対象
            var second = context.MovableSlimes[(int)MovableSlimeUnit.Index.Second];

            // 壁を越えないか？
            if (!(((1u << second.Position) & 0x7f7f7f7fu) > 0))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            if (FieldContextHelper.ExistsSlime(context, second.Index, second.Position + LeftSpeed))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 下に移動するシフト量の調整を行います。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="shift">シフト量</param>
        /// <returns></returns>
        private static int ModifyDownShift(FieldContext context, int shift)
        {
            // 底辺越えの判定対象は最下である2つめの移動可能スライムが対象
            var second = context.MovableSlimes[(int)MovableSlimeUnit.Index.Second];
            var position = second.Position + shift;
            var index = second.Index + (position / FieldContextConfig.FieldUnitBitCount);

            if (index > FieldContextConfig.FieldUnitCount - 1)
            {
                // 底辺を越えている場合は、底辺に着地させる
                shift -= (((position - FieldContextConfig.FieldUnitBitCount) / FieldContextConfig.OneLineBitCount) + 1) * FieldContextConfig.OneLineBitCount;
            }

            // 移動先に他スライムが存在している場合は、それ以上下に移動させない
            var first = context.MovableSlimes[(int)MovableSlimeUnit.Index.First];
            var maxShiftLine = shift / FieldContextConfig.OneLineBitCount;
            var shiftLine = maxShiftLine;

            // 1行ずつ移動して移動場所に他スライムが存在するか確認していく
            for (var line = 1; line <= maxShiftLine; line++)
            {
                // 2つめは縦横どちらでも検証が必要
                position = second.Position + (line * FieldContextConfig.OneLineBitCount);
                index = second.Index + (position / FieldContextConfig.FieldUnitBitCount);
                if (FieldContextHelper.ExistsSlime(context, index, position))
                {
                    // 他スライムのひとつ上まで移動させる
                    shiftLine = line - 1;
                    break;
                }

                // 1つめは横向きの場合のみ検証が必要
                if (MovableSlimeUnit.GetForm(context.MovableSlimes) == MovableSlimeUnit.Form.Horizontal)
                {
                    position = first.Position + (line * FieldContextConfig.OneLineBitCount);
                    index = first.Index + (position / FieldContextConfig.FieldUnitBitCount);
                    if (FieldContextHelper.ExistsSlime(context, index, position))
                    {
                        // 他スライムのひとつ上まで移動させる
                        shiftLine = line - 1;
                        break;
                    }
                }
            }

            return shiftLine * FieldContextConfig.OneLineBitCount;
        }
    }
}
