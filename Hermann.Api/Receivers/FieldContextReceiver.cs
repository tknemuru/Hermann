using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// フィールド状態の受信機能を提供します。
    /// </summary>
    /// <typeparam name="TIn">フィールド状態の型</typeparam>
    public abstract class FieldContextReceiver<TIn> : IReceivable<TIn, FieldContext>
    {
        /// <summary>
        /// フィールド状態のソースを受信し受信結果を返却します。
        /// </summary>
        /// <param name="source">ソース</param>
        /// <returns>受信結果</returns>
        public abstract FieldContext Receive(TIn source);
    }
}
