using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// 受信機能を提供します。
    /// </summary>
    public interface IReceivable<in TIn, out TOut>
    {
        /// <summary>
        /// ソースを受信し受信結果を返却します。
        /// </summary>
        /// <param name="source">ソース</param>
        /// <returns>受信結果</returns>
        TOut Receive(TIn source);
    }
}
