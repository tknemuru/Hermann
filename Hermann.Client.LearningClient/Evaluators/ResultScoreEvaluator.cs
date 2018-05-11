using System;
using System.Diagnostics;
using Hermann.Ai.Helpers;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Evaluators
{
    /// <summary>
    /// 得点による評価値の算出機能を提供します。
    /// </summary>
    public class ResultScoreEvaluator : ResultEvaluator
    {
        /// <summary>
        /// 結果の評価を実施します。
        /// </summary>
        /// <returns>評価値</returns>
        /// <param name="param">パラメータ</param>
        public override double Evaluate(Param param)
        {
            Debug.Assert(param.LastContext.OperationPlayer == param.Context.OperationPlayer, "OperationPlayerが一致していません。");

            var player = param.Context.OperationPlayer;
            var score = param.Context.Score[player.ToInt()];
            var lastScore = param.LastContext.Score[player.ToInt()];
            var diff = score - lastScore;
            LogWriter.WirteLog($"----- score evaluate -----");
            LogWriter.WirteLog($"player:{player.GetName()}");
            LogWriter.WirteLog($"score:{score}");
            LogWriter.WirteLog($"lastScore:{lastScore}");
            LogWriter.WirteLog($"scoreDiff:{diff}");
            var paritiedDiff = (double)diff * this.GetParity(param);
            LogWriter.WirteLog($"paritiedDiff:{paritiedDiff}");
            return paritiedDiff;
        }
    }
}
