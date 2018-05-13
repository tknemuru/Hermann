using Accord.IO;
using Accord.Neuro.Networks;
using Accord.Statistics.Models.Regression.Linear;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hermann.Ai
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
        /// DeepBeliefNetworks
        /// </summary>
        private Dictionary<AiPlayer.Version, DeepBeliefNetwork> DeepBeliefNetworks { get; set; }

        /// <summary>
        /// MultipleLinearRegressions
        /// </summary>
        private Dictionary<AiPlayer.Version, MultipleLinearRegression> MultipleLinearRegressions { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LearnerManager()
        {
            this.DeepBeliefNetworks = new Dictionary<AiPlayer.Version, DeepBeliefNetwork>();
            this.MultipleLinearRegressions = new Dictionary<AiPlayer.Version, MultipleLinearRegression>();
        }

        /// <summary>
        /// DeepBeliefNetworkを取得します。
        /// </summary>
        /// <param name="version">AIプレイヤのバージョン</param>
        /// <returns>DeepBeliefNetwork</returns>
        public DeepBeliefNetwork GetDeepBeliefNetwork(AiPlayer.Version version)
        {
            if(!this.DeepBeliefNetworks.ContainsKey(version))
            {
                this.DeepBeliefNetworks[version] = Serializer.Load<DeepBeliefNetwork>(string.Format(LearningConfig.LearnerSavePath + @"/{0}_{1}.bin", DeepBeliefNetworkFileName, version.ToString().ToLower()));
            }
            return this.DeepBeliefNetworks[version];
        }

        /// <summary>
        /// DeepBeliefNetworkを保存します。
        /// </summary>
        /// <param name="network">DeepBeliefNetwork</param>
        public void SaveDeepBeliefNetwork(DeepBeliefNetwork network, AiPlayer.Version version)
        {
            var filePath = string.Format(LearningConfig.LearnerSavePath + @"/{0}_{1}_{2}.bin", DeepBeliefNetworkFileName, DateTime.Now.ToString("yyyyMMddhhmmss"), version.ToString().ToLower());
            network.Save(filePath);
        }

        /// <summary>
        /// MultipleLinearRegressionを取得します。
        /// </summary>
        /// <param name="version">AIプレイヤのバージョン</param>
        /// <returns>MultipleLinearRegression</returns>
        public MultipleLinearRegression GetMultipleLinearRegression(AiPlayer.Version version)
        {
            if (!this.MultipleLinearRegressions.ContainsKey(version))
            {
                var csv = Resources.Load<TextAsset>($"Csvs/{MultipleLinearRegressionFileName}_{version.ToString().ToLower()}").ToString();
                //var filePath = string.Format(LearningConfig.LearnerSavePath + @"/{0}_{1}.csv", MultipleLinearRegressionFileName, version.ToString().ToLower());
                //var csv = FileHelper.ReadTextLines(filePath);
                //var weightsAndIntercept = csv.First().Split(',');
                var weightsAndIntercept = csv.Split(',');
                var weightsLength = weightsAndIntercept.Count() - 1;
                var intercept = weightsAndIntercept.Last();
                var weights = weightsAndIntercept.Take(weightsLength).Select(w => double.Parse(w)).ToArray();
                this.MultipleLinearRegressions[version] = new MultipleLinearRegression();
                this.MultipleLinearRegressions[version].NumberOfInputs = weightsLength;
                this.MultipleLinearRegressions[version].Intercept = double.Parse(intercept);
                this.MultipleLinearRegressions[version].Weights = weights;
            }
            return this.MultipleLinearRegressions[version];
        }

        /// <summary>
        /// MultipleLinearRegression。
        /// </summary>
        /// <param name="learner">MultipleLinearRegression</param>
        /// <param name="version">AIプレイヤのバージョン</param>
        public void SaveMultipleLinearRegression(MultipleLinearRegression learner, AiPlayer.Version version)
        {
            var filePath = string.Format(LearningConfig.LearnerSavePath + @"/{0}_{1}_{2}.csv", MultipleLinearRegressionFileName, DateTime.Now.ToString("yyyyMMddhhmmss"), version.ToString().ToLower());
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
