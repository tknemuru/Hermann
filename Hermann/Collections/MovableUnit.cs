using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// 移動可能な集合体
    /// </summary>
    public enum MovableUnit
    {
        /// <summary>
        /// 1つめ
        /// </summary>
        First,

        /// <summary>
        /// 2つめ
        /// </summary>
        Second,
    }

    /// <summary>
    /// 移動可能な集合体の拡張
    /// </summary>
    public static class ExtensionMovableUnit
    {
        /// <summary>
        /// 集合体の要素数
        /// </summary>
        public static readonly int Length = Enum.GetValues(typeof(MovableUnit)).Length;
    }
}
