using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Contexts
{
    /// <summary>
    /// フィールドの設定情報
    /// </summary>
    public static class FieldContextConfig
    {
        /// <summary>
        /// フィールドを分割したユニットの要素数
        /// </summary>
        public const int FieldUnitCount = 4;

        /// <summary>
        /// フィールドユニット内の行数
        /// </summary>
        public const int FieldUnitLineCount = 4;

        /// <summary>
        /// フィールドの行数
        /// </summary>
        public const int FieldLineCount = FieldUnitCount * FieldUnitLineCount;

        /// <summary>
        /// フィールドを分割したユニット1つあたりのビット数
        /// </summary>
        public const int FieldUnitBitCount = 32;

        /// <summary>
        /// 1行あたりのビット数
        /// </summary>
        public const int OneLineBitCount = 8;

        /// <summary>
        /// 使用するスライムの数
        /// </summary>
        public const int UsingSlimeCount = 4;

        /// <summary>
        /// 隠しフィールドのインデックス
        /// </summary>
        public const int HiddenUnitIndex = 0;

        /// <summary>
        /// 移動可能なスライムの初期シフト量
        /// </summary>
        public const int MovableSlimeInitialShift = 21;
    }
}
