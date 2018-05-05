using Hermann.Helpers;
using Hermann.Ai.Helpers;
using Hermann.Ai.Learners;
using Hermann.Ai.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Client.LearningClient.Di;
using Hermann.Ai;
using System.Diagnostics;

namespace Hermann.Client.LearningClient.Excecuters
{
    /// <summary>
    /// 学習実行機能を提供します。
    /// </summary>
    public class LearningExecuter : IExecutable, IInjectable<AiPlayer.Version>
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
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="version">バージョン</param>
        public void Inject(AiPlayer.Version version)
        {
            this.Version = version;
            this.HasInjected = true;
        }

        /// <summary>
        /// 学習を実行します。
        /// </summary>
        /// <param name="args"></param>
        public void Execute(string[] args)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            double[][] inputs;
            double[] outputs;
            var data = BuildLearning2DData();
            inputs = data.Inputs;
            outputs = data.Outputs;

            var regression = new MultipleLinearRegressionLearner().Learn(inputs, outputs);

            Console.WriteLine("Now saving ...");
            LearningClientDiProvider.GetContainer().GetInstance<LearnerManager>().SaveMultipleLinearRegression(regression, this.Version);
            Console.WriteLine("Save completed");
            Console.Write("Press any key to quit ..");
            Console.ReadKey();
        }

        /// <summary>
        /// 学習用データを組み立てます。
        /// </summary>
        /// <returns>学習用データ</returns>
        private static LearningData BuildLearningData()
        {
            var data = LearningClientDiProvider.GetContainer().GetInstance<LearningData>();
            var fileNames = Directory.EnumerateFiles(LearningConfig.LogOutputPath);
            foreach (var fileName in fileNames)
            {
                var logs = FileHelper.ReadTextLines(fileName);
                var d = DataConverter.ConvertLogToLearningData(logs);
                data.Inputs = data.Inputs.Concat(d.Inputs).ToArray();
                data.Outputs = data.Outputs.Concat(d.Outputs).ToArray();
            }
            return data;
        }

        /// <summary>
        /// 学習用データを組み立てます。
        /// </summary>
        /// <returns>学習用データ</returns>
        private static Learning2DData BuildLearning2DData()
        {
            var data = LearningClientDiProvider.GetContainer().GetInstance<Learning2DData>();
            var fileNames = Directory.EnumerateFiles(LearningConfig.LogOutputPath);
            foreach (var fileName in fileNames)
            {
                var firstChar = FileHelper.GetFileName(fileName).First();
                if(firstChar == '.')
                {
                    // 隠しファイルは読み込み対象外
                    continue;
                }

                var logs = FileHelper.ReadTextLines(fileName);
                var d = DataConverter.ConvertLogToLearning2DData(logs);
                data.Inputs = data.Inputs.Concat(d.Inputs).ToArray();
                data.Outputs = data.Outputs.Concat(d.Outputs).ToArray();
            }
            return data;
        }
    }
}
