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
        /// 方向
        /// </summary>
        public enum Direction : uint
        {
            /// <summary>
            /// 無し
            /// </summary>
            None = 0x0u,

            /// <summary>
            /// 上
            /// </summary>
            Up = 0x1u,

            /// <summary>
            /// 下
            /// </summary>
            Down = 0x2u,

            /// <summary>
            /// 左
            /// </summary>
            Left = 0x3u,

            /// <summary>
            /// 右
            /// </summary>
            Right = 0x4u,
        }

        /// <summary>
        /// プレイヤを取得するためのマスク量
        /// </summary>
        private const uint PlayerMask = 0x00000001u;

        /// <summary>
        /// 方向を取得するためのマスク量
        /// </summary>
        private const uint DirectionMask = 0x0000000eu;

        /// <summary>
        /// プレイヤを取得します。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static uint GetPlyer(uint command)
        {
            return command & Command.PlayerMask;
        }

        /// <summary>
        /// 移動方向を取得します。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static Direction GetDirection(uint command)
        {
            return (Direction)(command & DirectionMask);
        }
    }
}
