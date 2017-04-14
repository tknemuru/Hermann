using Hermann.Contexts;
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
    public interface ISendable<in TIn, out TOut>
    {
        /// <summary>
        /// ソースを送信します。
        /// </summary>
        /// <param name="source">ソース</param>
        /// <returns>送信結果</returns>
        TOut Send(TIn source);
    }
}
