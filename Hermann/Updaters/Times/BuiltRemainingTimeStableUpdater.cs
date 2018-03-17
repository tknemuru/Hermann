using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters.Times
{
    /// <summary>
    /// 設置残タイムを定まった値による更新機能を提供します。
    /// </summary>
    public class BuiltRemainingTimeStableUpdater : IBuiltRemainingTimeUpdatable
    {
        /// <summary>
        /// 経過時間
        /// </summary>
        private int ElapsedTicks { get; set; }

        /// <summary>
        /// 最大時間
        /// </summary>
        private long MaxTime { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="elapsedTicks"></param>
        public BuiltRemainingTimeStableUpdater(int elapsedTicks, long maxTime)
        {
            this.ElapsedTicks = elapsedTicks;
            this.MaxTime = maxTime;
        }

        /// <summary>
        /// 設置残タイムを更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            context.BuiltRemainingTime[(int)context.OperationPlayer] -= this.ElapsedTicks;
        }

        /// <summary>
        /// 経過時間の計測を停止します。
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// 計測を停止して、経過時間をゼロにリセットします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Reset(FieldContext context)
        {
            context.BuiltRemainingTime[(int)context.OperationPlayer] = this.MaxTime;
        }
    }
}
