using Hermann.Models;
using Hermann.Contexts;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters.Times
{
    /// <summary>
    /// 設置残タイムの更新機能を提供します。
    /// </summary>
    public class BuiltRemainingTimeElapsedTicksUpdater : IBuiltRemainingTimeUpdatable
    {
        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BuiltRemainingTimeElapsedTicksUpdater()
        {
            this.Stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 設置残タイムを更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            // 設置残タイムを減らす
            this.Stopwatch.Stop();
            context.BuiltRemainingTime[(int)context.OperationPlayer] -= this.Stopwatch.ElapsedTicks;
            this.Stopwatch.Start();
        }
    }
}
