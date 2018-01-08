using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 通知機能を提供します。
    /// </summary>
    public interface INotifiable<T>
    {
        /// <summary>
        /// 通知オブジェクト
        /// </summary>
        ReactiveProperty<T> Notifier { get; }
    }
}
