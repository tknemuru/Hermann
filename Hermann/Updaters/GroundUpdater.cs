using Hermann.Models;
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
    public sealed class GroundUpdater : IPlayerFieldUpdatable
    {
        /// <summary>
        /// 接地を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            // 接地しているか？
            var isGround = Player.IsGround(context, player);
            context.Ground[(int)player] = isGround;
        }
    }
}
