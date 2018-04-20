using Accord.IO;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.Statistics;
using Accord.Statistics.Models.Regression.Linear;
using Hermann.Helpers;
using Hermann.Learning.Di;
using Hermann.Learning.Helpers;
using Hermann.Learning.Learners;
using Hermann.Learning.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning.LearningClient
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] inputs;
            double[] outputs;
            var data = BuildLearning2DData();
            inputs = data.Inputs;
            outputs = data.Outputs;

            var regression = new MultipleLinearRegressionLearner().Learn(inputs, outputs);

            Console.WriteLine("Now saving ...");
            LearningClientDiProvider.GetContainer().GetInstance<LearnerManager>().SaveMultipleLinearRegression(regression);
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
            foreach(var fileName in fileNames)
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
                var logs = FileHelper.ReadTextLines(fileName);
                var d = DataConverter.ConvertLogToLearning2DData(logs);
                data.Inputs = data.Inputs.Concat(d.Inputs).ToArray();
                data.Outputs = data.Outputs.Concat(d.Outputs).ToArray();
            }
            return data;
        }
    }
}
