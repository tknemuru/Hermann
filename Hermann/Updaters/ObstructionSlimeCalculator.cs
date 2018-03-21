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
            var addScore = score;
            var opposite = Player.GetOppositeIndex(player);

            // 自分のおじゃまスライムの得点を取得
            var myScore = ObstructionSlimeHelper.ObstructionsToScore(context.ObstructionSlimes[(int)player]);

            // 0より大きければ自分のおじゃまスライムの得点を減算し、相殺する
            if(myScore > 0)
            {
                var subScore = (myScore - score) < 0 ? 0 : (myScore - score);
                addScore = (score - myScore) < 0 ? 0 : (score - myScore);
                context.ObstructionSlimes[(int)player] = ObstructionSlimeHelper.ScoreToObstructions(subScore);
            }

            // 得点には既に存在するおじゃまスライム分の得点も加算する
            addScore += ObstructionSlimeHelper.ObstructionsToScore(context.ObstructionSlimes[(int)opposite]);

            // 加算するおじゃまスライムを生成
            context.ObstructionSlimes[(int)opposite] = ObstructionSlimeHelper.ScoreToObstructions(addScore);

            // 使用済得点の更新
            context.UsedScore[(int)player] = context.Score[(int)player] - ObstructionSlimeHelper.GetScoreRemainder(score);
        }
    }
}
