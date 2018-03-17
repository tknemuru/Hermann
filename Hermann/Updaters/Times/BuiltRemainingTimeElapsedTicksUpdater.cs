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
            if(this.Stopwatch.ElapsedMilliseconds > 0)
            {
                context.BuiltRemainingTime[(int)context.OperationPlayer] -= this.Stopwatch.ElapsedMilliseconds;
                this.Stopwatch.Restart();
            }
            else
            {
                this.Stopwatch.Start();
            }
        }

        /// <summary>
        /// 計測を停止して、経過時間をゼロにリセットします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Reset(FieldContext context)
        {
            this.Stopwatch.Stop();
            this.Stopwatch.Reset();
            context.BuiltRemainingTime[(int)context.OperationPlayer] = FieldContextConfig.MaxBuiltRemainingTime;
        }
    }
}
