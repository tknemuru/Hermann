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
    /// 移動可能スライムの更新機能を提供します。
    /// </summary>
    public sealed class MovableSlimesUpdater : IPlayerFieldUpdatable
    {
        /// <summary>
        /// 移動可能スライムを更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var slimes = context.NextSlimes[(int)player][(int)NextSlime.Index.First];
            var movables = context.MovableSlimes[(int)player];

            // Nextスライムを移動可能スライムにセットする
            // フィールド上には移動可能スライムと同じ位置に通常のスライムが配置済なので、移動可能スライムの情報を書き換えれば通常のスライムに変わることになる
            MovableSlime.ForEach((unitIndex) =>
            {
                movables[(int)unitIndex].Slime = slimes[(int)unitIndex];
                movables[(int)unitIndex].Index = FieldContextConfig.MinHiddenUnitIndex;
                movables[(int)unitIndex].Position = FieldContextConfig.MovableSlimeInitialShiftBeforeDroped + ((int)unitIndex * FieldContextConfig.OneLineBitCount);
                context.MovableSlimes[(int)player][(int)unitIndex] = movables[(int)unitIndex];

                // フィールドにも反映させる
                context.SlimeFields[(int)player][movables[(int)unitIndex].Slime][movables[(int)unitIndex].Index] |= 1u << movables[(int)unitIndex].Position;
            });
        }
    }
}
