using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Senders
{
    /// <summary>
    /// フィールド状態の送信機能を提供します。
    /// </summary>
    /// <typeparam name="TOut">送信するフィールド状態の型</typeparam>
    public abstract class FieldContextSender<TOut> : ISendable<FieldContext, TOut>
    {
        /// <summary>
        /// フィールド状態を送信します。
        /// </summary>
        /// <param name="source">フィールド状態</param>
        /// <returns>送信結果</returns>
        public abstract TOut Send(FieldContext context);
    }
}
