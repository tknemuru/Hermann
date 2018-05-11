using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// 死活監視評価機能を提供します。
    /// </summary>
    public class AliveEvaluator : IEvaluable<FieldContext, double>, IInjectable<AliveEvaluator.Config>
    {
        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// 設定情報
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 危険なインデックス
            /// </summary>
            public int[] DangerIndexes { get; set; } = new int[] { 5, 13, 21, 29 };

            /// <summary>
            /// 危険なユニット
            /// </summary>
            public int[] DangerUnits { get; set; } = new int[] { 1 };

            /// <summary>
            /// 危険ペナルティ評価値
            /// </summary>
            public double DangerEval { get; set;} = -999;
        }

        /// <summary>
        /// 設定情報
        /// </summary>
        private Config MyConfig { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Inject(Config config)
        {
            this.MyConfig = config;
            this.HasInjected = true;
        }

        /// <summary>
        /// 死活監視評価を実行します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double Evaluate(FieldContext context)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            var dangerCount = new int[Player.Length];
            Player.ForEach(player =>
            {
                foreach (var u in this.MyConfig.DangerUnits)
                {
                    foreach (var i in this.MyConfig.DangerIndexes)
                    {
                        dangerCount[player.ToInt()] += FieldContextHelper.ExistsSlime(context, player, FieldContextConfig.MaxHiddenUnitIndex + u, i) ? 1 : 0;
                    }
                }
            });
            var difCount = dangerCount[Player.Index.First.ToInt()] - dangerCount[Player.Index.Second.ToInt()];
            return difCount * this.MyConfig.DangerEval;
        }
    }
}
