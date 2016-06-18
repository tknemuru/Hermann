using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Senders
{
    /// <summary>
    /// 送信機能を提供します。
    /// </summary>
    public abstract class Sender
    {
        /// <summary>
        /// 状態を文字列に変換し送信します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>フィールドの状態を表す文字列</returns>
        public abstract string Send(ulong[] context);
    }
}
