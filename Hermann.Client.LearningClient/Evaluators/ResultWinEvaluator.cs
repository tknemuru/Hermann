using Hermann.Ai.Evaluators;
using Hermann.Ai.Helpers;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Client.LearningClient.Evaluators
{
    /// <summary>
    /// 結果評価値の計算機能を提供します。
    /// </summary>
    public class ResultWinEvaluator : ResultEvaluator
    {
        /// <summary>
        /// 基底評価値
        /// </summary>
        private const double BaseScore = 1.0d;

        /// <summary>
        /// ボーナス加算割合
        /// </summary>
        private const double BonusRate = 0.001d;

        /// <summary>
        /// 結果の評価を実施します。
        /// </summary>
        /// <returns>評価値</returns>
        /// <param name="param">パラメータ</param>
        public override double Evaluate(Param param)
        {
            var win = FieldContextHelper.GetWinPlayer(param.LastContext, param.Context);
            Debug.Assert(win != null, "勝敗が決まっていません");

            var score = (win == Player.Index.First) ? BaseScore : BaseScore * -1;
            LogWriter.WirteLog($"value:{score}");
            var scoreDiff = param.Context.Score[Player.Index.First.ToInt()] - param.Context.Score[Player.Index.Second.ToInt()];
            LogWriter.WirteLog($"win:{win}");
            LogWriter.WirteLog($"score:{param.Context.Score[Player.Index.First.ToInt()]} | {param.Context.Score[Player.Index.Second.ToInt()]}");
            LogWriter.WirteLog($"scoreDiff:{scoreDiff}");
            if (win == Player.Index.First && scoreDiff > 0)
            {
                score += scoreDiff * BonusRate;
            }
            else if (win == Player.Index.Second && scoreDiff < 0)
            {
                score += scoreDiff * BonusRate;
            }
            LogWriter.WirteLog($"correction value:{score}");
            return score;
        }
    }
}
