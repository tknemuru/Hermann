using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Fields;

namespace Hermann.Collections
{
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
        /// <param name="context"></param>
        /// <param name="command"></param>
        public static void Move(FieldContext context, ushort command)
        {

        }
    }
}
