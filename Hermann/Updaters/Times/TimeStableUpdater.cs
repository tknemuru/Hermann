using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters.Times
{
    /// <summary>
    /// 経過時間を定まった値による更新機能を提供します。
    /// </summary>
    public class TimeStableUpdater : ITimeUpdatable
    {
        /// <summary>
        /// 経過時間
        /// </summary>
        private long ElapsedTicks { get; set; }

        /// <summary>
        ///コンストラクタ
        /// </summary>
        /// <param name="elapsedTicks">経過時間</param>
        public TimeStableUpdater(long elapsedTicks)
        {
            this.ElapsedTicks = elapsedTicks;
        }

        /// <summary>
        /// 経過時間を更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            context.Time -= this.ElapsedTicks;
        }
    }
}
