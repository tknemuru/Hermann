using Hermann.Models;
using Hermann.Contexts;
using Hermann.Helpers;
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
    /// 経過時間の更新機能を提供します。
    /// </summary>
    public class TimeElapsedTicksUpdater : ITimeUpdatable
    {
        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimeElapsedTicksUpdater()
        {
            this.Stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 時間計測を開始します。
        /// </summary>
        public void Start()
        {
            this.Stopwatch.Start();
        }

        /// <summary>
        /// 時間計測をリセットします。
        /// </summary>
        public void Reset()
        {
            this.Stopwatch.Reset();
        }

        /// <summary>
        /// 時間を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Update(FieldContext context)
        {
            // 経過時間の更新
            this.Stopwatch.Stop();
            context.Time += this.Stopwatch.ElapsedMilliseconds;
            this.Stopwatch.Restart();
        }
    }
}
