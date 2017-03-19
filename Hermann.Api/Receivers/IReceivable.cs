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
    public interface IReceivable<in T>
    {
        /// <summary>
        /// ソースを受信しフィールドの状態に変換します。
        /// </summary>
        /// <param name="source">ソース</param>
        /// <returns>フィールドの状態</returns>
        FieldContext Receive(T source);
    }
}
