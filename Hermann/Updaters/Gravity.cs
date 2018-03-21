using Hermann.Models;
using Hermann.Contexts;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 重力
    /// </summary>
    public sealed class Gravity : IPlayerFieldParameterizedUpdatable<Gravity.Param>
    {
        /// <summary>
        /// 重力をかけた結果
        /// </summary>
        public enum ResultState
        {
            /// <summary>
            /// 移動有
            /// </summary>
            Moved,

            /// <summary>
            /// 移動無
            /// </summary>
            NotMoved,

            /// <summary>
            /// 未確定
            /// </summary>
            Undefined,
        }

        /// <summary>
        /// 重力に関するパラメータを格納します。
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 重力の強さ
            /// </summary>
            public int Strength { get; set; }

            /// <summary>
            /// 落下距離
            /// </summary>
            public ResultState ResultState { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Param()
            {
                this.Strength = DefaultStrength;
                this.ResultState = ResultState.Undefined;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="strength">重力の強さ</param>
            public Param(int strength)
            {
                this.Strength = strength;
                this.ResultState = ResultState.Undefined;
            }
        }

        /// <summary>
        /// 重力量の規定値
        /// </summary>
        private const int DefaultStrength = FieldContextConfig.HorizontalLineLength;

        /// <summary>
        /// 重力によるフィールド状態の更新を行います。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        public void Update(FieldContext context, Player.Index player, Param param)
        {
            param.ResultState = ResultState.NotMoved;

            // 最底辺から順に重力をかけていく
            for (var unitIndex = FieldContextConfig.FieldUnitCount - 1; unitIndex >= 0; unitIndex--)
            {
                for (var position = FieldContextConfig.FieldUnitBitCount - 1; position >= 0; position--)
                {
                    foreach (var slime in ExtensionSlime.Slimes)
                    {
                        Reflect(context, player, slime, position, unitIndex, param);
                    }
                }
            }
        }

        /// <summary>
        /// 指定した場所に対して重力をかけます。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="slime">スライム</param>
        /// <param name="position">ユニット内の位置</param>
        /// <param name="unitIndex">ユニットのインデックス</param>
        /// <param name="param">パラメータ</param>
        private static void Reflect(FieldContext context, Player.Index player, Slime slime, int position, int unitIndex, Param param)
        {
            Debug.Assert(param.Strength > 0, "重力が1より小さいです。");

            var isMovable = false;

            // 移動可能スライムは重力に影響されない
            MovableSlime.ForEach((index) =>
            {
                if (context.MovableSlimes[(int)player][(int)index].Slime == slime &&
                    context.MovableSlimes[(int)player][(int)index].Position == position &&
                    context.MovableSlimes[(int)player][(int)index].Index == unitIndex)
                {
                    isMovable = true;
                }
            });
            if (isMovable)
            {
                return;
            }

            // 対象の場所にスライムが存在しなければ何もしない
            if (!FieldContextHelper.ExistsSlime(context, player, unitIndex, position, slime))
            {
                return;
            }

            // シフト量
            var shift = param.Strength * FieldContextConfig.OneLineBitCount;
            var testPosition = position + shift;
            var testUnitIndex = unitIndex + (testPosition / FieldContextConfig.FieldUnitBitCount);

            if (testUnitIndex > FieldContextConfig.FieldUnitCount - 1)
            {
                // 底辺を越えている場合は、底辺に着地させる
                var absCurrentPosition = (unitIndex * FieldContextConfig.FieldUnitBitCount) + position;
                var absBottomPosition = ((FieldContextConfig.FieldUnitCount - 1) * FieldContextConfig.FieldUnitBitCount) + ((FieldContextConfig.FieldUnitBitCount - 1) - (8 - ((position % FieldContextConfig.OneLineBitCount) + 1)));
                shift = absBottomPosition - absCurrentPosition;
            }

            // 移動先に他スライムが存在している場合は、それ以上下に移動させない
            var maxShiftLine = shift / FieldContextConfig.OneLineBitCount;
            var shiftLine = maxShiftLine;

            // 1行ずつ移動して移動場所に他スライムが存在するか確認していく
            for (var line = 1; line <= maxShiftLine; line++)
            {
                testPosition = position + (line * FieldContextConfig.OneLineBitCount);
                testUnitIndex = unitIndex + (testPosition / FieldContextConfig.FieldUnitBitCount);
                testPosition %= FieldContextConfig.FieldUnitBitCount;
                Debug.Assert(testUnitIndex < FieldContextConfig.FieldUnitCount, string.Format("ユニットインデックスが不正です。{0}", testUnitIndex));
                if (FieldContextHelper.ExistsSlime(context, player, testUnitIndex, testPosition))
                {
                    // 他スライムのひとつ上まで移動させる
                    shiftLine = line - 1;
                    break;
                }
            }

            // 補正したシフト量
            var modifiedShift = shiftLine * FieldContextConfig.OneLineBitCount;

            // 一度でも移動するのならば結果を移動有にする
            if(modifiedShift > 0)
            {
                param.ResultState = ResultState.Moved;
            }

            // 移動前スライムを消す
            context.SlimeFields[(int)player][slime][unitIndex] &= ~(1u << position);

            // スライムを移動させる
            var updPosition = position + modifiedShift;
            var updUnitIndex = unitIndex + (updPosition / FieldContextConfig.FieldUnitBitCount);
            updPosition %= FieldContextConfig.FieldUnitBitCount;
            Debug.Assert(!FieldContextHelper.ExistsSlime(context, player, updUnitIndex, updPosition), string.Format("他のスライムが移動場所に存在しています。 Index : {0} Position : {1}", updUnitIndex, updPosition));
            context.SlimeFields[(int)player][slime][updUnitIndex] |= 1u << updPosition;
        }
    }
}
