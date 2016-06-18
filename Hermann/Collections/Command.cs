using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// HermannAPIに対する命令に関する情報を提供します。
    /// </summary>
    public static class Command
    {
        /// <summary>
        /// プレイヤを取得するためのマスク量
        /// </summary>
        public const ulong PlayerMask = 0x00000001;

        /// <summary>
        /// 方向を取得するためのマスク量
        /// </summary>
        public const ulong DirectionMask = 0x0000000e;
    }
}
