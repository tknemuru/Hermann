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
        public const ulong PlayerMask = 0x00000001ul;

        /// <summary>
        /// 方向を取得するためのマスク量
        /// </summary>
        public const ulong DirectionMask = 0x0000000eul;

        /// <summary>
        /// 方向：無し
        /// </summary>
        public const ulong DirectionNone = 0x0ul;

        /// <summary>
        /// 方向：上
        /// </summary>
        public const ulong DirectionUp = 0x2ul;

        /// <summary>
        /// 方向：下
        /// </summary>
        public const ulong DirectionDown = 0x4ul;

        /// <summary>
        /// 方向：左
        /// </summary>
        public const ulong DirectionLeft = 0x6ul;

        /// <summary>
        /// 方向：右
        /// </summary>
        public const ulong DirectionRight = 0x8ul;
    }
}
