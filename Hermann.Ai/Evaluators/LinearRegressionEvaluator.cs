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

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// 回帰分析によるフィールド評価機能を提供します。
    /// </summary>
    public class LinearRegressionEvaluator : IEvaluable<FieldContext, double>
    {
        /// <summary>
        /// 回帰分析機能
        /// </summary>
        private TransformBase<double[], double> Regression { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LinearRegressionEvaluator()
        {
            this.Regression = AiDiProvider.GetContainer().GetInstance<LearnerManager>().GetMultipleLinearRegression();
        }

        /// <summary>
        /// フィールド状態の評価を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double Evaluate(FieldContext context)
        {
            var input = AiDiProvider.GetContainer().GetInstance<InputDataProvider>().
                GetVector(InputDataProvider.Vector.Main, context);
            return this.Regression.Transform(input.ToArray());
        }
    }
}
