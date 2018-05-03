using Hermann.Ai.Helpers;
using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Calculators
{
    /// <summary>
    /// 結果評価値の計算機能を提供します。
    /// </summary>
    public class ResultEvaluationCalculator
    {
        /// <summary>
        /// ボーナス加算割合
        /// </summary>
        private const double BonusRate = 0.001d;

        /// <summary>
        /// 結果評価値の計算を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="win">勝利プレイヤ</param>
        /// <returns>結果評価値</returns>
        public double Evaluate(FieldContext context, Player.Index win)
        {
            var score = (win == Player.Index.First) ? 1.0d : -1.0d;
            LogWriter.WirteLog($"value:{score}");
            var scoreDiff = context.Score[(int)Player.Index.First] - context.Score[(int)Player.Index.Second];
            LogWriter.WirteLog($"wind:{score}");
            LogWriter.WirteLog($"score:{context.Score[(int)Player.Index.First]} | {context.Score[(int)Player.Index.Second]}");
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
