using Accord.Statistics.Models.Regression.Linear;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Learning;
using Hermann.Learning.Di;
using Hermann.Learning.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Evaluators
{
    public class MultipleLinearRegressionFieldContextEvaluator
    {
        /// <summary>
        /// 線形回帰結果
        /// </summary>
        private MultipleLinearRegression Regression { get; set; }

        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MultipleLinearRegressionFieldContextEvaluator()
        {
            this.Regression = AiDiProvider.GetContainer().GetInstance<LearnerManager>().GetMultipleLinearRegression();
            this.RandomGen = new Random();
        }

        /// <summary>
        /// フィールド状態の評価を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double Evaluate(FieldContext context)
        {
            var input = DataConverter.ConvertContextToArray(context).Select(c => double.Parse(c.ToString())).ToArray();
            var answer = this.Regression.Transform(input);
            var correction = 0.01d;
            var parity = this.RandomGen.Next(2);
            //FileHelper.WriteLine($"answer:{answer}");
            answer += correction * parity;
            //FileHelper.WriteLine($"parity:{parity}");
            //FileHelper.WriteLine($"correction answer:{answer}");
            return answer;
        }
    }
}
