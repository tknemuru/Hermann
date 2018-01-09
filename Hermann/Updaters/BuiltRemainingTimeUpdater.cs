using Hermann.Collections;
using Hermann.Contexts;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 設置残タイムの更新機能を提供します。
    /// </summary>
    public class BuiltRemainingTimeUpdater : IFieldUpdatable, INotifiable<BuiltRemainingTimeUpdater.Notification>
    {
        /// <summary>
        /// 通知
        /// </summary>
        public enum Notification
        {
            /// <summary>
            /// 移動中
            /// </summary>
            Moving,

            /// <summary>
            /// 接地中
            /// </summary>
            Grounding,

            /// <summary>
            /// 設置済
            /// </summary>
            HasBuilt,
        }

        /// <summary>
        /// 通知オブジェクト
        /// </summary>
        public ReactiveProperty<Notification> Notifier { get; private set; }

        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BuiltRemainingTimeUpdater()
        {
            this.Stopwatch = new Stopwatch();
            this.Notifier = new ReactiveProperty<Notification>();
        }

        /// <summary>
        /// 設置残タイムを更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            var player = context.OperationPlayer;

            // 接地していなければ設置残タイムは最大値
            if (!context.Ground[(int)player].Value)
            {
                context.BuiltRemainingTime[(int)player] = FieldContextConfig.MaxBuiltRemainingTime;
                this.Notifier.Value = Notification.Moving;
                return;
            }

            // 設置残タイムを減らす
            this.Stopwatch.Stop();
            context.BuiltRemainingTime[(int)player] -= this.Stopwatch.ElapsedTicks;
            this.Stopwatch.Start();

            if (context.BuiltRemainingTime[(int)player] <= 0)
            {
                this.Notifier.Value = Notification.HasBuilt;
            }
            else
            {
                this.Notifier.Value = Notification.Grounding;
            }
        }
    }
}
