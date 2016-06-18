using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Receivers
{
    /// <summary>
    /// 受信機能を提供します。
    /// </summary>
    public abstract class Receiver
    {
        /// <summary>
        /// ソースを受信しフィールドの状態に変換します。
        /// </summary>
        /// <param name="source">ソース</param>
        /// <returns>フィールドの状態</returns>
        public abstract ulong[] Receive(string source);
    }
}
