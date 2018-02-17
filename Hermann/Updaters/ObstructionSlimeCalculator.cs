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
    /// おじゃまスライム数の計算機能を提供します。
    /// </summary>
    public class ObstructionSlimeCalculator : IPlayerFieldUpdatable
    {
        /// <summary>
        /// おじゃまスライム数を算出します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var score = context.Score[(int)player] - context.UsedScore[(int)player];
            var opposite = Player.GetOppositeIndex(player);

            // 得点には既に存在するおじゃまスライム分の得点も加算する
            score += ObstructionSlimeHelper.ObstructionsToScore(context.ObstructionSlimes[(int)opposite]);

            // 加算するおじゃまスライムを生成
            context.ObstructionSlimes[(int)opposite] = ObstructionSlimeHelper.ScoreToObstructions(score);

            // 使用済得点の更新
            context.UsedScore[(int)player] = context.Score[(int)player] - ObstructionSlimeHelper.GetScoreRemainder(score);
        }
    }
}
