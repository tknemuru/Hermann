using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Updaters;
using Reactive.Bindings;
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
    public sealed class Player : INotifiable<Player.Index>
    {
        /// <summary>
        /// プレイヤ数
        /// </summary>
        public const int Length = 2;
        
        /// <summary>
        /// インデックス
        /// </summary>
        public enum Index
        {
            /// <summary>
            /// 1P
            /// </summary>
            First = 0,

            /// <summary>
            /// 2P
            /// </summary>
            Second,
        }

        /// <summary>
        /// 通知オブジェクト
        /// </summary>
        public ReactiveProperty<Index> Notifier { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Player()
        {
            this.Notifier = new ReactiveProperty<Index>(Index.First);
        }

        /// <summary>
        /// 移動速度
        /// </summary>
        private static class Speed
        {
            /// <summary>
            /// 左に動かす際の速度
            /// </summary>
            public const int Left = 1;

            /// <summary>
            /// 右に動かす際の速度
            /// </summary>
            public const int Right = -1;

            /// <summary>
            /// 下に動かす際の速度
            /// </summary>
            public const int Down = 4 * FieldContextConfig.OneLineBitCount;

            /// <summary>
            /// 何もしない際の速度
            /// </summary>
            public const int None = FieldContextConfig.OneLineBitCount;
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        public FieldContext Move(FieldContext context)
        {
            switch (context.OperationDirection)
            {
                case Direction.None :
                    Move(context, ModifyDownShift(context, Speed.None));
                    break;
                case Direction.Up:
                    // TODO:あとで実装
                    //throw new NotSupportedException();
                case Direction.Down:
                    Move(context, ModifyDownShift(context, Speed.Down));
                    break;
                case Direction.Left:
                    if (IsEnabledLeftMove(context))
                    {
                        Move(context, Speed.Left);
                    }
                    break;
                case Direction.Right:
                    if (IsEnabledRightMove(context))
                    {
                        Move(context, Speed.Right);
                    }
                    break;
                default :
                    throw new ApplicationException(string.Format("不正な方向です。{0}", context.OperationDirection));
            }
            return context;
        }

        /// <summary>
        /// 両プレイヤのフィールド情報に対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEach(Action<Player.Index> action)
        {
            for (var player = Player.Index.First; (int)player < Player.Length; player++)
            {
                action(player);
            }
        }

        /// <summary>
        /// 指定したプレイヤが接地しているかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns></returns>
        public static bool IsGround(FieldContext context, Player.Index player)
        {
            return ModifyDownShift(context, player, FieldContextConfig.OneLineBitCount) <= 0;
        }

        /// <summary>
        /// 下に移動するシフト量の調整を行います。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="shift">シフト量</param>
        /// <returns>調整された下に移動するシフト量</returns>
        private static int ModifyDownShift(FieldContext context, Player.Index player, int shift)
        {
            // 底辺越えの判定対象は最下である2つめの移動可能スライムが対象
            var movableSlimes = context.MovableSlimes[(int)player];
            var second = movableSlimes[(int)MovableSlime.UnitIndex.Second];
            var position = second.Position + shift;
            var index = second.Index + (position / FieldContextConfig.FieldUnitBitCount);

            if (index > FieldContextConfig.FieldUnitCount - 1)
            {
                // 底辺を越えている場合は、底辺に着地させる
                var absCurrentPosition = (second.Index * FieldContextConfig.FieldUnitBitCount) + second.Position;
                var absBottomPosition = ((FieldContextConfig.FieldUnitCount - 1) * FieldContextConfig.FieldUnitBitCount) + ((FieldContextConfig.FieldUnitBitCount - 1) - (8 - ((second.Position % FieldContextConfig.OneLineBitCount) + 1)));
                shift = absBottomPosition - absCurrentPosition;
            }

            // 移動先に他スライムが存在している場合は、それ以上下に移動させない
            var first = movableSlimes[(int)MovableSlime.UnitIndex.First];
            var maxShiftLine = shift / FieldContextConfig.OneLineBitCount;
            var shiftLine = maxShiftLine;

            // 1行ずつ移動して移動場所に他スライムが存在するか確認していく
            for (var line = 1; line <= maxShiftLine; line++)
            {
                // 2つめは縦横どちらでも検証が必要
                position = second.Position + (line * FieldContextConfig.OneLineBitCount);
                index = second.Index + (position / FieldContextConfig.FieldUnitBitCount);
                position %= FieldContextConfig.FieldUnitBitCount;
                if (FieldContextHelper.ExistsSlime(context, player, index, position))
                {
                    // 他スライムのひとつ上まで移動させる
                    shiftLine = line - 1;
                    break;
                }

                // 1つめは横向きの場合のみ検証が必要
                if (MovableSlime.GetUnitForm(movableSlimes) == MovableSlime.UnitForm.Horizontal)
                {
                    position = first.Position + (line * FieldContextConfig.OneLineBitCount);
                    index = first.Index + (position / FieldContextConfig.FieldUnitBitCount);
                    position %= FieldContextConfig.FieldUnitBitCount;
                    if (FieldContextHelper.ExistsSlime(context, player, index, position))
                    {
                        // 他スライムのひとつ上まで移動させる
                        shiftLine = line - 1;
                        break;
                    }
                }
            }

            return shiftLine * FieldContextConfig.OneLineBitCount;
        }

        /// <summary>
        /// 下に移動するシフト量の調整を行います。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="shift">シフト量</param>
        /// <returns>調整された下に移動するシフト量</returns>
        private static int ModifyDownShift(FieldContext context, int shift)
        {
            return ModifyDownShift(context, context.OperationPlayer, shift);
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">状態</param>
        /// <param name="movableField">操作可能スライムの状態</param>
        /// <param name="colorField">対象色スライムの状態</param>
        /// <param name="shift">シフト量</param>
        private void Move(FieldContext context, int shift)
        {
            var movableSlimes = context.MovableSlimes[(int)context.OperationPlayer];
            var slimeFields = context.SlimeFields[(int)context.OperationPlayer].Value;

            // １．移動前スライムを消す
            foreach (var movable in movableSlimes)
            {
                slimeFields[movable.Slime][movable.Index] &= ~(1u << movable.Position);
            }

            // ２．スライムを移動させる
            foreach (var movable in movableSlimes)
            {
                var position = movable.Position + shift;
                movable.Index += (position / FieldContextConfig.FieldUnitBitCount);
                movable.Position = position % FieldContextConfig.FieldUnitBitCount;
                Debug.Assert(!FieldContextHelper.ExistsSlime(context, context.OperationPlayer, movable.Index, movable.Position), string.Format("他のスライムが移動場所に存在しています。 Index : {0} Position : {1}", movable.Index, movable.Position));
                slimeFields[movable.Slime][movable.Index] |= 1u << movable.Position;
            }

            this.Notifier.Value = context.OperationPlayer;
        }

        /// <summary>
        /// 右に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>右に移動可能かどうか</returns>
        private static bool IsEnabledRightMove(FieldContext context)
        {
            var movableSlimes = context.MovableSlimes[(int)context.OperationPlayer];
            var first = movableSlimes[(int)MovableSlime.UnitIndex.First];
            var second = movableSlimes[(int)MovableSlime.UnitIndex.Second];

            // 壁を越えないか？
            // 壁越えチェック対象は最右である1つめの移動可能スライムが対象
            if (!(((1u << first.Position) & 0xf8f8f8f8u) > 0))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            // 縦向きの場合は最下である2つめの移動可能スライムが対象
            var form = MovableSlime.GetUnitForm(movableSlimes);
            if (form == MovableSlime.UnitForm.Vertical &&
                FieldContextHelper.ExistsSlime(context, context.OperationPlayer, second.Index, second.Position + Speed.Right))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            // 横向きの場合は最右である1つめの移動可能スライムが対象
            if (form == MovableSlime.UnitForm.Horizontal &&
                FieldContextHelper.ExistsSlime(context, context.OperationPlayer, first.Index, first.Position + Speed.Right))
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
            var movableSlimes = context.MovableSlimes[(int)context.OperationPlayer];
            var second = movableSlimes[(int)MovableSlime.UnitIndex.Second];

            // 壁を越えないか？
            if (!(((1u << second.Position) & 0x7f7f7f7fu) > 0))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            if (FieldContextHelper.ExistsSlime(context, context.OperationPlayer, second.Index, second.Position + Speed.Left))
            {
                return false;
            }

            return true;
        }
    }
}
