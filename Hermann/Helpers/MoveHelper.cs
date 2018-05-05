using Hermann.Contexts;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helpers
{
    /// <summary>
    /// 移動に関する補助機能を提供します。
    /// </summary>
    public static class MoveHelper
    {
        /// <summary>
        /// 移動情報
        /// </summary>
        public class Move
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
        /// 上に移動できる場合は移動した位置をセットします。
        /// </summary>
        /// <param name="unit">ユニットインデックス</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="result">移動した位置</param>
        /// <returns>移動できるかどうか</returns>
        public static bool TryMoveUp(int unit, int index, Move result)
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
        public static bool TryMoveDown(int unit, int index, Move result)
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
        public static bool TryMoveRight(int unit, int index, Move result)
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
        public static bool TryMoveLeft(int unit, int index, Move result)
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
