using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Notifiers
{
    /// <summary>
    /// 通知機能を提供します。
    /// </summary>
    public interface INotifiable<T>
    {
        /// <summary>
        /// 通知オブジェクト
        /// </summary>
        T Notifier { get; }
    }
}
