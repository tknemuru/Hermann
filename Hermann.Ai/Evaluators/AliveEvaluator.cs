using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// 死活監視評価機能を提供します。
    /// </summary>
    public class AliveEvaluator : IEvaluable<FieldContext, double>
    {
        /// <summary>
        /// 危険なインデックス
        /// </summary>
        private int[] DangerIndexes = new int[] { 5, 13, 21, 29 };

        /// <summary>
        /// 危険なユニット
        /// </summary>
        private int[] DangerUnits = new int[] { 1 };

        /// <summary>
        /// 危険ペナルティ評価値
        /// </summary>
        private const double DangerEval = -999;

        /// <summary>
        /// 死活監視評価を実行します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double Evaluate(FieldContext context)
        {
            var dangerCount = new int[Player.Length];
            Player.ForEach(player =>
            {
                foreach (var u in DangerUnits)
                {
                    foreach (var i in DangerIndexes)
                    {
                        dangerCount[player.ToInt()] += FieldContextHelper.ExistsSlime(context, player, FieldContextConfig.MaxHiddenUnitIndex + u, i) ? 1 : 0;
                    }
                }
            });
            var difCount = dangerCount[Player.Index.First.ToInt()] - dangerCount[Player.Index.Second.ToInt()];
            return difCount * DangerEval;
        }
    }
}
