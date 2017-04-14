using Hermann.Api.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// コマンドの受信機能を提供します。
    /// </summary>
    /// <typeparam name="TIn">コマンドの型</typeparam>
    /// <typeparam name="TOut">受信結果の型</typeparam>
    public abstract class CommandReceiver<TIn, TOut> : IReceivable<TIn, TOut>
    {
        /// <summary>
        /// コマンドを受信し受信結果を返却します。
        /// </summary>
        /// <param name="command">ネイティブコマンド</param>
        /// <returns>受信結果</returns>
        public abstract TOut Receive(TIn command);
    }
}
