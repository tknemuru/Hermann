using Hermann.Collections;
using Hermann.Contexts;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    public class TimeUpdater : IFieldUpdatable
    {
        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimeUpdater()
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
            this.Stopwatch.Stop();

            // 経過時間の更新
            context.Time += this.Stopwatch.ElapsedTicks;

            Player.ForEach((player) =>
            {
                // 接地していなければ設置残タイムの更新はしない
                if (!context.Ground[(int)player].Value)
                {
                    return;
                }

                // 設置残タイムの更新
                var time = context.BuiltRemainingTime[(int)player] - this.Stopwatch.ElapsedTicks;
                var hasBuilt = (time <= 0);
                context.BuiltRemainingTime[(int)player] = hasBuilt ? 0 : time;

                // 設置残タイムが0以下になったら接地をfalseに戻す
                context.Ground[(int)player].Value = !hasBuilt;
            });

            this.Stopwatch.Start();
        }
    }
}
