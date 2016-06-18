using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Fields
{
    /// <summary>
    /// フィールドの状態を保持します。
    /// </summary>
    public sealed class FieldContext
    {
        /// <summary>
        /// フィールドに配置されているスライムの集合
        /// 1番目の配列：1P/2P
        /// 2番目の配列：スライムの集合
        /// </summary>
        public ulong[][] Slimes { get; set; }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public FieldContext()
        {
            this.Slimes = new ulong[ExtensionPlayer.PlayerCount][];
        }
    }
}
