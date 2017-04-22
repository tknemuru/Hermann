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
    /// <summary>
    /// 接地の更新機能を提供します。
    /// </summary>
    public class GroundUpdater : IUpdatable
    {
        /// <summary>
        /// 接地を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Update(FieldContext context)
        {
            FieldContextHelper.ActionForEachPlayer(context, (con, player) =>
            {
                con.Ground[player] = Player.ModifyDownShift(context, player, FieldContextConfig.OneLineBitCount) <= 0;
            });
        }
    }
}
