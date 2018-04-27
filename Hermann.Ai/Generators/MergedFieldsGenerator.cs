using Hermann.Contexts;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Generators
{
    /// <summary>
    /// マージされたフィールド状態の生成機能を提供します。
    /// </summary>
    public class MergedFieldsGenerator : IGeneratable<uint[], IEnumerable<uint>>
    {
        /// <summary>
        /// マージされたフィールド状態リストを生成します。
        /// </summary>
        /// <param name="fields">フィールド状態</param>
        /// <returns>マージされたフィールド状態リスト</returns>
        public IEnumerable<uint> GetNext(uint[] fields)
        {
            var mergeFields = new List<uint>();
            // 最後のユニットの先頭行まで繰り返す
            var maxIndex = ((fields.Length - 1) * FieldContextConfig.FieldUnitLineCount) + 1;

            for (var horizontalIndex = 0; horizontalIndex < maxIndex; horizontalIndex++)
            {
                var startUnitIndex = horizontalIndex / FieldContextConfig.FieldUnitLineCount;
                Debug.Assert(startUnitIndex >= 0 && startUnitIndex < FieldContextConfig.FieldUnitCount, string.Format("開始ユニットのインデックスが不正です。{0}", startUnitIndex));
                var shiftLine = horizontalIndex % FieldContextConfig.FieldUnitLineCount;
                Debug.Assert(shiftLine >= 0 && shiftLine < FieldContextConfig.FieldUnitLineCount, string.Format("シフト行数が不正です。{0}", shiftLine));

                // シフト行数が0なら開始ユニットをそのまま返却して終了
                if (shiftLine == 0)
                {
                    mergeFields.Add(fields[startUnitIndex]);
                    continue;
                }

                Debug.Assert(startUnitIndex < (FieldContextConfig.FieldUnitCount - 1), "最終ユニットがこのロジックを通ることは想定外です。");
                var mergedField = (fields[startUnitIndex] >> shiftLine * FieldContextConfig.OneLineBitCount) | (fields[startUnitIndex + 1] << (FieldContextConfig.FieldUnitLineCount - shiftLine) * FieldContextConfig.OneLineBitCount);
                mergeFields.Add(mergedField);
            }

            return mergeFields;
        }
    }
}
