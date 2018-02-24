using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 勝数の更新機能を提供します。
    /// </summary>
    public class WinCountUpdater : IPlayerFieldUpdatable
    {
        /// <summary>
        /// 勝敗を判定するユニットのインデックス
        /// </summary>
        private const int WinjudgmentUnitIndex = FieldContextConfig.MaxHiddenUnitIndex + 1;

        /// <summary>
        /// 勝敗を判定する位置
        /// </summary>
        private const int WinjudgmentPosition = 5;

        /// <summary>
        /// 勝ちを判定して勝数を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            // 勝敗を判定する
            var isEnd = FieldContextHelper.ExistsSlime(context, player, WinjudgmentUnitIndex, WinjudgmentPosition);

            // 勝敗が決まっている場合は、勝ち数をインクリメントする
            if (isEnd)
            {
                context.WinCount[(int)Player.GetOppositeIndex(player)]++;
            }
        }
    }
}
