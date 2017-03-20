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
        public const int FieldUnitCount = 3;

        /// <summary>
        /// フィールドユニット内の行数
        /// </summary>
        public const int FieldLineCount = 4;

        /// <summary>
        /// フィールドを分割したユニット1つあたりのビット数
        /// </summary>
        public const int FieldUnitBitCount = 32;

        /// <summary>
        /// 1行あたりのビット数
        /// </summary>
        public const int OneLineBitCount = 8;
    }
}
