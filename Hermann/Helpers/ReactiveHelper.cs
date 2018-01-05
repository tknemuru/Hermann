using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;

namespace Hermann.Helpers
{
    /// <summary>
    /// Reactiveに関する機能を提供します。
    /// </summary>
    public static class ReactiveHelper
    {
        /// <summary>
        /// ReactivePropertyの配列から値の配列を取得します。
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="reactive">ReactiveProperty</param>
        /// <returns>値の配列</returns>
        public static T[] GetValues<T>(ReactiveProperty<T>[] reactive)
        {
            return reactive.Select(r => r.Value).ToArray();
        }
    }
}
