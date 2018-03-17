using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters.Times
{
    /// <summary>
    /// 設置残タイムの更新機能を提供します。
    /// </summary>
    public interface IBuiltRemainingTimeUpdatable : IFieldUpdatable
    {
        /// <summary>
        /// 計測を停止して、経過時間をゼロにリセットします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        void Reset(FieldContext context);
    }
}
