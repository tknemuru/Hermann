using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Models
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
}
