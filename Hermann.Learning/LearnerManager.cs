using Accord.IO;
using Accord.Neuro.Networks;
using Accord.Statistics.Models.Regression.Linear;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning
{
    /// <summary>
    /// 学習機能の管理機能を提供します。
    /// </summary>
    public class LearnerManager
    {
        /// <summary>
        /// DeepBeliefNetworkのファイル名
        /// </summary>
        private const string DeepBeliefNetworkFileName = "deep_belief_network";

        /// <summary>
        /// MultipleLinearRegressionのファイル名
        /// </summary>
        private const string MultipleLinearRegressionFileName = "multiple_linear_regression";

        /// <summary>
        /// DeepBeliefNetwork
        /// </summary>
        private DeepBeliefNetwork DeepBeliefNetwork { get; set; }

        /// <summary>
        /// MultipleLinearRegression
        /// </summary>
        private MultipleLinearRegression MultipleLinearRegression { get; set; }

        /// <summary>
        /// DeepBeliefNetworkを取得します。
        /// </summary>
        /// <returns>DeepBeliefNetwork</returns>
        public DeepBeliefNetwork GetDeepBeliefNetwork()
        {
            if(this.DeepBeliefNetwork == null)
            {
                this.DeepBeliefNetwork = Serializer.Load<DeepBeliefNetwork>(string.Format(LearningConfig.LearnerSavePath + @"/{0}.bin", DeepBeliefNetworkFileName));
            }
            return this.DeepBeliefNetwork;
        }

        /// <summary>
        /// DeepBeliefNetworkを保存します。
        /// </summary>
        /// <param name="network">DeepBeliefNetwork</param>
        public void SaveDeepBeliefNetwork(DeepBeliefNetwork network)
        {
            var filePath = string.Format(LearningConfig.LearnerSavePath + @"/{0}_{1}.bin", DeepBeliefNetworkFileName, DateTime.Now.ToString("yyyyMMddhhmmss"));
            network.Save(filePath);
        }

        /// <summary>
        /// MultipleLinearRegressionを取得します。
        /// </summary>
        /// <returns>MultipleLinearRegression</returns>
        public MultipleLinearRegression GetMultipleLinearRegression()
        {
            if (this.MultipleLinearRegression == null)
            {
                var filePath = string.Format(LearningConfig.LearnerSavePath + @"/{0}.csv", MultipleLinearRegressionFileName);
                var csv = FileHelper.ReadTextLines(filePath);
                var weightsAndIntercept = csv.First().Split(',');
                var weightsLength = weightsAndIntercept.Count() - 1;
                var intercept = weightsAndIntercept.Last();
                var weights = weightsAndIntercept.Take(weightsLength).Select(w => double.Parse(w)).ToArray();
                this.MultipleLinearRegression = new MultipleLinearRegression();
                this.MultipleLinearRegression.NumberOfInputs = weightsLength;
                this.MultipleLinearRegression.Intercept = double.Parse(intercept);
                this.MultipleLinearRegression.Weights = weights;
            }
            return this.MultipleLinearRegression;
        }

        /// <summary>
        /// MultipleLinearRegression。
        /// </summary>
        /// <param name="learner">MultipleLinearRegression</param>
        public void SaveMultipleLinearRegression(MultipleLinearRegression learner)
        {
            var filePath = string.Format(LearningConfig.LearnerSavePath + @"/{0}_{1}.csv", MultipleLinearRegressionFileName, DateTime.Now.ToString("yyyyMMddhhmmss"));
            var sb = new StringBuilder();
            foreach(var w in learner.Weights)
            {
                sb.Append($"{w},");
            }
            sb.Append(learner.Intercept.ToString());
            FileHelper.Write(sb.ToString(), filePath);
        }
    }
}
