using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// フィールドの状態を表すコンテキストに関する情報/機能を提供します。
    /// </summary>
    public static class FieldContext
    {
        /// <summary>
        /// インデックス：命令
        /// </summary>
        public const int IndexCommand = 0;

        /// <summary>
        /// インデックス：フィールド上部（占有状態）
        /// </summary>
        public const int IndexOccupiedFieldUpper = 1;

        /// <summary>
        /// インデックス：フィールド下部（占有状態）
        /// </summary>
        public const int IndexOccupiedFieldLower = 2;

        /// <summary>
        /// インデックス：フィールド上部（操作対象スライム）
        /// </summary>
        public const int IndexMovableFieldUpper = 3;

        /// <summary>
        /// インデックス：フィールド下部（操作対象スライム）
        /// </summary>
        public const int IndexMovableFieldLower = 4;

        /// <summary>
        /// コンテキストを構成する要素数
        /// </summary>
        public const int ElementCount = 5;
    }
}
