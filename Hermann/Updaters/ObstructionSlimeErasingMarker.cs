using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライムを消し済にマーキングする機能を提供します。
    /// </summary>
    public class ObstructionSlimeErasingMarker : IPlayerFieldUpdatable
    {
        /// <summary>
        /// 移動情報
        /// </summary>
        private class Move
        {
            /// <summary>
            /// ユニットインデックス
            /// </summary>
            public int Unit { get; set; }

            /// <summary>
            /// ユニット内のインデックス
            /// </summary>
            public int Index { get; set; }
        }

        /// <summary>
        /// おじゃまスライムを消し済にマーキングします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var erased = context.SlimeFields[(int)player][Slime.Erased];
            var obs = context.SlimeFields[(int)player][Slime.Obstruction];
            var move = new Move();
            var moveFuncs = new List<Func<int, int, Move, bool>>()
                {
                    TryMoveUp,
                    TryMoveDown,
                    TryMoveRight,
                    TryMoveLeft
                };
            var updErased = erased.Select(e => e).ToArray();
            var updObs = obs.Select(o => o).ToArray();

            for (var unit = 0; unit < FieldContextConfig.FieldUnitCount; unit++)
            {
                for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
                {
                    if ((erased[unit] & 1u << i) > 0)
                    {
                        // 上下左右のおじゃまスライムを消し済にマーキングする
                        foreach (var func in moveFuncs)
                        {
                            if (func(unit, i, move))
                            {
                                if ((obs[move.Unit] & 1u << move.Index) > 0)
                                {
                                    updObs[move.Unit] &= ~(1u << move.Index);
                                    updErased[move.Unit] |= (1u << move.Index);
                                }
                            }
                        }
                    }
                }
            }

            context.SlimeFields[(int)player][Slime.Erased] = updErased;
            context.SlimeFields[(int)player][Slime.Obstruction] = updObs;
        }

        /// <summary>
        /// 上に移動できる場合は移動した位置をセットします。
        /// </summary>
        /// <param name="unit">ユニットインデックス</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="result">移動した位置</param>
        /// <returns>移動できるかどうか</returns>
        private bool TryMoveUp(int unit, int index, Move result)
        {
            var isSuccess = false;
            var movedUnit = unit;
            var movedIndex = index;
            if (index >= FieldContextConfig.OneLineBitCount)
            {
                movedIndex = index - FieldContextConfig.OneLineBitCount;
                isSuccess = true;
            }
            else if (unit > 0)
            {
                movedUnit = --unit;
                movedIndex = FieldContextConfig.FieldUnitBitCount - (FieldContextConfig.OneLineBitCount - index);
                isSuccess = true;
            }

            result.Unit = movedUnit;
            result.Index = movedIndex;
            return isSuccess;
        }

        /// <summary>
        /// 下に移動できる場合は移動した位置をセットします。
        /// </summary>
        /// <param name="unit">ユニットインデックス</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="result">移動した位置</param>
        /// <returns>移動できるかどうか</returns>
        private bool TryMoveDown(int unit, int index, Move result)
        {
            var isSuccess = false;
            var movedUnit = unit;
            var movedIndex = index;
            if (index < (FieldContextConfig.FieldUnitBitCount - FieldContextConfig.OneLineBitCount))
            {
                movedIndex = index + FieldContextConfig.OneLineBitCount;
                isSuccess = true;
            }
            else if (unit < FieldContextConfig.FieldUnitCount - 1)
            {
                movedUnit = ++unit;
                movedIndex = FieldContextConfig.OneLineBitCount - (FieldContextConfig.FieldUnitBitCount - index);
                isSuccess = true;
            }

            result.Unit = movedUnit;
            result.Index = movedIndex;
            return isSuccess;
        }

        /// <summary>
        /// 右に移動できる場合は移動した位置をセットします。
        /// </summary>
        /// <param name="unit">ユニットインデックス</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="result">移動した位置</param>
        /// <returns>移動できるかどうか</returns>
        private bool TryMoveRight(int unit, int index, Move result)
        {
            var isSuccess = false;
            var movedUnit = unit;
            var movedIndex = index;
            if (!FieldContextHelper.IsCloseToRightWall(index))
            {
                movedIndex = --index;
                isSuccess = true;
            }

            result.Unit = movedUnit;
            result.Index = movedIndex;
            return isSuccess;
        }

        /// <summary>
        /// 左に移動できる場合は移動した位置をセットします。
        /// </summary>
        /// <param name="unit">ユニットインデックス</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="result">移動した位置</param>
        /// <returns>移動できるかどうか</returns>
        private bool TryMoveLeft(int unit, int index, Move result)
        {
            var isSuccess = false;
            var movedUnit = unit;
            var movedIndex = index;
            if (!FieldContextHelper.IsCloseToLeftWall(index))
            {
                movedIndex = ++index;
                isSuccess = true;
            }

            result.Unit = movedUnit;
            result.Index = movedIndex;
            return isSuccess;
        }
    }
}
