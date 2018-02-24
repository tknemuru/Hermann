using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using Hermann.Notifiers;
using Hermann.Updaters;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// スライムの移動機能を提供します。
    /// </summary>
    public sealed class SlimeMover : IPlayerFieldUpdatable, INotifiable<SlimeMover.MoveResult>
    {
        /// <summary>
        /// 移動結果
        /// </summary>
        public enum MoveResult
        {
            /// <summary>
            /// 失敗
            /// </summary>
            Failed,

            /// <summary>
            /// 成功
            /// </summary>
            Success,

            /// <summary>
            /// 未確定
            /// </summary>
            Undefined,
        }

        /// <summary>
        /// 通知オブジェクト
        /// </summary>
        public ReactiveProperty<MoveResult> Notifier { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlimeMover()
        {
            this.Notifier = new ReactiveProperty<MoveResult>(MoveResult.Undefined);
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
            /// 上に動かす際の速度
            /// </summary>
            public const int Up = FieldContextConfig.OneLineBitCount * -1;

            /// <summary>
            /// 何もしない際の速度
            /// </summary>
            public const int None = FieldContextConfig.OneLineBitCount;
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var result = MoveResult.Undefined;
            var shift = 0;

            switch (context.OperationDirection)
            {
                case Direction.None :
                    shift = ModifyDownShift(context, Speed.None, player);
                    result = shift == 0 ? MoveResult.Failed : MoveResult.Success;
                    Move(context, shift, player);
                    break;
                case Direction.Up:
                    result = MoveUp(context, player);
                    break;
                case Direction.Down:
                    shift = ModifyDownShift(context, Speed.Down, player);
                    result = shift == 0 ? MoveResult.Failed : MoveResult.Success;
                    Move(context, shift, player);
                    break;
                case Direction.Left:
                    result = IsEnabledLeftMove(context, player) ? MoveResult.Success : MoveResult.Failed;
                    if (result == MoveResult.Success)
                    {
                        Move(context, Speed.Left, player);
                    }
                    break;
                case Direction.Right:
                    result = IsEnabledRightMove(context, player) ? MoveResult.Success : MoveResult.Failed;
                    if (result == MoveResult.Success)
                    {
                        Move(context, Speed.Right, player);
                    }
                    break;
                default :
                    throw new ApplicationException(string.Format("不正な方向です。{0}", context.OperationDirection));
            }

            Debug.Assert(result != MoveResult.Undefined, "移動結果がUndefinedです。");
            this.Notifier.Value = result;
        }

        /// <summary>
        /// 指定したプレイヤが接地しているかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns></returns>
        public static bool IsGround(FieldContext context, Player.Index player)
        {
            return ModifyDownShift(context, FieldContextConfig.OneLineBitCount, player) <= 0;
        }

        /// <summary>
        /// 下に移動するシフト量の調整を行います。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="shift">シフト量</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>調整された下に移動するシフト量</returns>
        private static int ModifyDownShift(FieldContext context, int shift, Player.Index player)
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
        /// 上に移動します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private MoveResult MoveUp(FieldContext context, Player.Index player)
        {
            var movables = context.MovableSlimes[(int)player];
            int beforePosition = 0;
            int beforeIndex = 0;
            Slime beforeSlime = Slime.None;
            int afterPosition = 0;
            int afterIndex = 0;

            switch (context.RotationDirection[(int)player])
            {
                case Direction.Right :
                    Debug.Assert(MovableSlime.GetUnitForm(context.MovableSlimes[(int)player]) == MovableSlime.UnitForm.Vertical, "移動可能スライムの向きが不正です。");
                    if (!IsEnabledRightMove(context, player))
                    {
                        return MoveResult.Failed;
                    }

                    beforePosition = movables[(int)MovableSlime.UnitIndex.First].Position;
                    beforeIndex = movables[(int)MovableSlime.UnitIndex.First].Index;
                    afterPosition = movables[(int)MovableSlime.UnitIndex.Second].Position - 1;
                    afterIndex = movables[(int)MovableSlime.UnitIndex.Second].Index;
                    Debug.Assert(afterIndex >= 0, string.Format("インデックスが不正です。{0}", afterIndex));

                    RemoveSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.First].Slime], beforeIndex, beforePosition);
                    movables[(int)MovableSlime.UnitIndex.First].Position = afterPosition;
                    movables[(int)MovableSlime.UnitIndex.First].Index = afterIndex;
                    SetSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.First].Slime], afterIndex, afterPosition);
                    break;
                case Direction.Down :
                    Debug.Assert(MovableSlime.GetUnitForm(context.MovableSlimes[(int)player]) == MovableSlime.UnitForm.Horizontal, "移動可能スライムの向きが不正です。");
                    if (IsGround(context, player))
                    {
                        return MoveResult.Failed;
                    }

                    // rb⇒□□⇒□□⇒r□
                    // □□ rb bb b□
                    beforePosition = movables[(int)MovableSlime.UnitIndex.Second].Position;
                    beforeIndex = movables[(int)MovableSlime.UnitIndex.Second].Index;
                    beforeSlime = movables[(int)MovableSlime.UnitIndex.Second].Slime;

                    this.Move(context, ModifyDownShift(context, FieldContextConfig.OneLineBitCount, player), player);

                    RemoveSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.Second].Slime], movables[(int)MovableSlime.UnitIndex.Second].Index, movables[(int)MovableSlime.UnitIndex.Second].Position);
                    movables[(int)MovableSlime.UnitIndex.Second].Slime = movables[(int)MovableSlime.UnitIndex.First].Slime;
                    SetSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.Second].Slime], movables[(int)MovableSlime.UnitIndex.Second].Index, movables[(int)MovableSlime.UnitIndex.Second].Position);

                    RemoveSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.First].Slime], movables[(int)MovableSlime.UnitIndex.First].Index, movables[(int)MovableSlime.UnitIndex.First].Position);
                    movables[(int)MovableSlime.UnitIndex.First].Position = beforePosition;
                    movables[(int)MovableSlime.UnitIndex.First].Index = beforeIndex;
                    movables[(int)MovableSlime.UnitIndex.First].Slime = beforeSlime;
                    SetSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.First].Slime], movables[(int)MovableSlime.UnitIndex.First].Index, movables[(int)MovableSlime.UnitIndex.First].Position);
                    break;
                case Direction.Left :
                    Debug.Assert(MovableSlime.GetUnitForm(context.MovableSlimes[(int)player]) == MovableSlime.UnitForm.Vertical, "移動可能スライムの向きが不正です。");
                    if (!IsEnabledLeftMove(context, player))
                    {
                        return MoveResult.Failed;
                    }

                    beforePosition = movables[(int)MovableSlime.UnitIndex.Second].Position;
                    beforeIndex = movables[(int)MovableSlime.UnitIndex.Second].Index;
                    afterPosition = movables[(int)MovableSlime.UnitIndex.First].Position + 1;
                    afterIndex = movables[(int)MovableSlime.UnitIndex.First].Index;
                    Debug.Assert(afterIndex < FieldContextConfig.FieldUnitBitCount, string.Format("インデックスが不正です。{0}", afterIndex));

                    RemoveSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.Second].Slime], beforeIndex, beforePosition);
                    movables[(int)MovableSlime.UnitIndex.Second].Position = afterPosition;
                    movables[(int)MovableSlime.UnitIndex.Second].Index = afterIndex;
                    SetSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.Second].Slime], afterIndex, afterPosition);
                    break;
                case Direction.Up :
                    Debug.Assert(MovableSlime.GetUnitForm(context.MovableSlimes[(int)player]) == MovableSlime.UnitForm.Horizontal, "移動可能スライムの向きが不正です。");
                    // □□⇒rb⇒rr⇒□r
                    // rb □□ □□ □b
                    beforePosition = movables[(int)MovableSlime.UnitIndex.First].Position;
                    beforeIndex = movables[(int)MovableSlime.UnitIndex.First].Index;
                    beforeSlime = movables[(int)MovableSlime.UnitIndex.First].Slime;

                    this.Move(context, ModifyUpShift(context, Speed.Up, player), player);

                    RemoveSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.First].Slime], movables[(int)MovableSlime.UnitIndex.First].Index, movables[(int)MovableSlime.UnitIndex.First].Position);
                    movables[(int)MovableSlime.UnitIndex.First].Slime = movables[(int)MovableSlime.UnitIndex.Second].Slime;
                    SetSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.First].Slime], movables[(int)MovableSlime.UnitIndex.First].Index, movables[(int)MovableSlime.UnitIndex.First].Position);

                    RemoveSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.Second].Slime], movables[(int)MovableSlime.UnitIndex.Second].Index, movables[(int)MovableSlime.UnitIndex.Second].Position);
                    movables[(int)MovableSlime.UnitIndex.Second].Position = beforePosition;
                    movables[(int)MovableSlime.UnitIndex.Second].Index = beforeIndex;
                    movables[(int)MovableSlime.UnitIndex.Second].Slime = beforeSlime;
                    SetSlime(context.SlimeFields[(int)player][movables[(int)MovableSlime.UnitIndex.Second].Slime], movables[(int)MovableSlime.UnitIndex.Second].Index, movables[(int)MovableSlime.UnitIndex.Second].Position);
                    break;
                default :
                    throw new ArgumentException(string.Format("想定外の方向です。{0}", context.RotationDirection[(int)player]));
            }
            return MoveResult.Success;
        }

        /// <summary>
        /// 上に移動するシフト量の調整を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="shift">シフト量</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>調整されたシフト量</returns>
        private int ModifyUpShift(FieldContext context, int shift, Player.Index player)
        {
            var first = context.MovableSlimes[(int)player][(int)MovableSlime.UnitIndex.First];
            var position = first.Position + shift;
            var index = first.Index + (position / FieldContextConfig.FieldUnitBitCount) + (position < 0 ? -1 : 0);
            var modShift = shift;

            if (index < 0)
            {
                // 天井を越えている場合は、最上辺に配置させる
                var absCurrentPosition = (first.Index * FieldContextConfig.FieldUnitBitCount) + first.Position;
                var absBottomPosition = first.Position % FieldContextConfig.OneLineBitCount;
                modShift = absBottomPosition - absCurrentPosition;
            }

            return modShift;
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">状態</param>
        /// <param name="movableField">操作可能スライムの状態</param>
        /// <param name="colorField">対象色スライムの状態</param>
        /// <param name="shift">シフト量</param>
        /// <param name="player">プレイヤ</param>
        private void Move(FieldContext context, int shift, Player.Index player)
        {
            if (shift <= (FieldContextConfig.OneLineBitCount * -1))
            {
                this.MoveUp(context, shift, player);
            }
            else
            {
                this.MoveDown(context, shift, player);
            }
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">状態</param>
        /// <param name="shift">シフト量</param>
        /// <param name="player">プレイヤ</param>
        private void MoveDown(FieldContext context, int shift, Player.Index player)
        {
            var movableSlimes = context.MovableSlimes[(int)player];
            var slimeFields = context.SlimeFields[(int)player];

            // １．移動前スライムを消す
            foreach (var movable in movableSlimes)
            {
                RemoveSlime(slimeFields[movable.Slime], movable.Index, movable.Position);
            }

            // ２．スライムを移動させる
            foreach (var movable in movableSlimes)
            {
                var position = movable.Position + shift;
                movable.Index += (position / FieldContextConfig.FieldUnitBitCount);
                movable.Position = position % FieldContextConfig.FieldUnitBitCount;
                Debug.Assert(!FieldContextHelper.ExistsSlime(context, player, movable.Index, movable.Position), string.Format("他のスライムが移動場所に存在しています。 Index : {0} Position : {1}", movable.Index, movable.Position));
                SetSlime(slimeFields[movable.Slime], movable.Index, movable.Position);
            }
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">状態</param>
        /// <param name="shift">シフト量</param>
        /// <param name="player">プレイヤ</param>
        private void MoveUp(FieldContext context, int shift, Player.Index player)
        {
            var movableSlimes = context.MovableSlimes[(int)player];
            var slimeFields = context.SlimeFields[(int)player];

            // １．移動前スライムを消す
            foreach (var movable in movableSlimes)
            {
                RemoveSlime(slimeFields[movable.Slime], movable.Index, movable.Position);
            }

            // ２．スライムを移動させる
            foreach (var movable in movableSlimes)
            {
                var position = movable.Position + shift;
                movable.Index += (position / FieldContextConfig.FieldUnitBitCount) + (position < 0 ? -1 : 0);
                if (position < 0)
                {
                    movable.Position = FieldContextConfig.FieldUnitBitCount + (position % FieldContextConfig.FieldUnitBitCount);
                }
                else
                {
                    movable.Position = position % FieldContextConfig.FieldUnitBitCount;
                }
                Debug.Assert(!FieldContextHelper.ExistsSlime(context, player, movable.Index, movable.Position), string.Format("他のスライムが移動場所に存在しています。 Index : {0} Position : {1}", movable.Index, movable.Position));
                SetSlime(slimeFields[movable.Slime], movable.Index, movable.Position);
            }
        }

        /// <summary>
        /// 指定したフィールドの位置のスライムを削除します。
        /// </summary>
        /// <param name="fields">フィールド</param>
        /// <param name="index">フィールド単位のインデックス</param>
        /// <param name="position">位置</param>
        private static void RemoveSlime(uint[] fields, int index, int position)
        {
            fields[index] &= ~(1u << position);
        }

        /// <summary>
        /// 指定したフィールドの位置にスライムを排除します。
        /// </summary>
        /// <param name="fields">フィールド</param>
        /// <param name="index">フィールド単位のインデックス</param>
        /// <param name="position">位置</param>
        private static void SetSlime(uint[] fields, int index, int position)
        {
            fields[index] |= (1u << position);
        }

        /// <summary>
        /// 右に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>右に移動可能かどうか</returns>
        private static bool IsEnabledRightMove(FieldContext context, Player.Index player)
        {
            var movableSlimes = context.MovableSlimes[(int)player];
            var first = movableSlimes[(int)MovableSlime.UnitIndex.First];
            var second = movableSlimes[(int)MovableSlime.UnitIndex.Second];

            // 壁を越えないか？
            // 壁越えチェック対象は最右である1つめの移動可能スライムが対象
            if (FieldContextHelper.IsCloseToRightWall(first.Position))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            // 縦向きの場合は最下である2つめの移動可能スライムが対象
            var form = MovableSlime.GetUnitForm(movableSlimes);
            if (form == MovableSlime.UnitForm.Vertical &&
                FieldContextHelper.ExistsSlime(context, player, second.Index, second.Position + Speed.Right))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            // 横向きの場合は最右である1つめの移動可能スライムが対象
            if (form == MovableSlime.UnitForm.Horizontal &&
                FieldContextHelper.ExistsSlime(context, player, first.Index, first.Position + Speed.Right))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 左に移動可能かどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>左に移動可能かどうか</returns>
        private static bool IsEnabledLeftMove(FieldContext context, Player.Index player)
        {
            // 判定対象は最左・最下である2つめの移動可能スライムが対象
            var movableSlimes = context.MovableSlimes[(int)player];
            var second = movableSlimes[(int)MovableSlime.UnitIndex.Second];

            // 壁を越えないか？
            if (FieldContextHelper.IsCloseToLeftWall(second.Position))
            {
                return false;
            }

            // 移動場所に他スライムが存在していないか？
            if (FieldContextHelper.ExistsSlime(context, player, second.Index, second.Position + Speed.Left))
            {
                return false;
            }

            return true;
        }
    }
}
