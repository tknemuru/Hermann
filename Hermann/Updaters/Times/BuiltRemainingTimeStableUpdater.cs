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
        /// コンストラクタ
        /// </summary>
        /// <param name="elapsedTicks"></param>
        public BuiltRemainingTimeStableUpdater(int elapsedTicks)
        {
            this.ElapsedTicks = elapsedTicks;
        }

        /// <summary>
        /// 設置残タイムを更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            context.BuiltRemainingTime[(int)context.OperationPlayer] -= this.ElapsedTicks;
        }
    }
}
