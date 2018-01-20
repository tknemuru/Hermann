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
    public abstract class Notifier<TIn, TOut>
    {
        protected ReactiveProperty<TOut> Status { get; set; }

        /// <summary>
        /// インプットに対する結果を通知します。
        /// </summary>
        public ReactiveProperty<TOut> Notify(TIn input)
        {
            this.Update(input);
            return this.Status;
        }

        protected abstract void Update(TIn input);
    }
}
