using Accord.MachineLearning;
using Hermann.Ai.Providers;
using Hermann.Contexts;
using Hermann.Ai;
using Hermann.Ai.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// 回帰分析によるフィールド評価機能を提供します。
    /// </summary>
    public class LinearRegressionEvaluator : IEvaluable<FieldContext, double>, IInjectable<AiPlayer.Version>
    {
        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// AIプレイヤのバージョン
        /// </summary>
        /// <value>The version.</value>
        private AiPlayer.Version Version { get; set; }

        /// <summary>
        /// 回帰分析機能
        /// </summary>
        private TransformBase<double[], double> Regression { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="version">AIプレイヤのバージョン</param>
        public void Inject(AiPlayer.Version version)
        {
            this.Version = version;
            this.Regression = AiDiProvider.GetContainer().GetInstance<LearnerManager>().GetMultipleLinearRegression(this.Version);
            this.HasInjected = true;
        }

        /// <summary>
        /// フィールド状態の評価を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double Evaluate(FieldContext context)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            var input = AiDiProvider.GetContainer().GetInstance<InputDataProvider>().
                GetVector(this.Version, context);
            return this.Regression.Transform(input.ToArray());
        }
    }
}
