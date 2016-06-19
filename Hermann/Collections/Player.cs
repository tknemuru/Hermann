using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// プレイヤ
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// 1P
        /// </summary>
        public const int First = 0;

        /// <summary>
        /// 2P
        /// </summary>
        public const int Second = 1;

        /// <summary>
        /// プレイヤ数
        /// </summary>
        public const int PlayerCount = 2;

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        public static ulong[] Move(ulong[] context)
        {
            var direction = context[FieldContext.IndexCommand] & Command.DirectionMask;
            return new ulong[3];
        }
    }
}
