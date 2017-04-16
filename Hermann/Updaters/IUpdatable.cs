using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// フィールド状態の更新機能を提供します。
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// フィールド状態を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        void Update(FieldContext context);
    }
}
