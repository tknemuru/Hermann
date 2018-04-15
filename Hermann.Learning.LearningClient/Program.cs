using Accord.IO;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.Statistics;
using Hermann.Helpers;
using Hermann.Learning.Di;
using Hermann.Learning.Helpers;
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
            //double[][] inputs;
            //double[][] outputs;
            double[][] testInputs;
            double[][] testOutputs;

            // Load ascii digits dataset.
            //inputs = DataManager.Load(@"../../../data/data.txt", out outputs);
            double[][] inputs = {
                new double[] { 6,20,3,3,0,0,15,10,8,1},
                new double[] { 4,20,8,23,1,45,20,7,3,0},
                new double[] { 6,3,20,20,2,20,8,5,6,0},
                new double[] { 30,30,5,1,1,4,8,30,20,1},
                new double[] { 23,16,9,5,0,0,20,30,18,0},
                new double[] { 10,8,40,33,0,10,30,2,30,2},
                new double[] { 40,50,30,10,1,30,40,9,0,0},
                new double[] { 50,60,0,0,0,3,8,20,40,2},
                new double[] { 60,10,10,15,0,10,10,25,25,1},
                new double[] { 40,20,30,5,0,7,8,8,10,1},
            };
            double[][] outputs = {
                new double[] { 1, 0 },
                new double[] { 0, 1 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 0 },
                new double[] { 0, 1 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 0 },
                new double[] { 1, 0 },
            };

            //var data = BuildLearningData();
            //inputs = data.Inputs;
            //outputs = data.Outputs;
            //int separateIndex = (int)Math.Floor(inputs.Count() * 0.7);
            int separateIndex = 8;

            // The first 500 data rows will be for training. The rest will be for testing.
            //testInputs = inputs.Skip(500).ToArray();
            //testOutputs = outputs.Skip(500).ToArray();
            //inputs = inputs.Take(500).ToArray();
            //outputs = outputs.Take(500).ToArray();
            testInputs = inputs.Skip(separateIndex).ToArray();
            testOutputs = outputs.Skip(separateIndex).ToArray();
            inputs = inputs.Take(separateIndex).ToArray();
            outputs = outputs.Take(separateIndex).ToArray();

            // Setup the deep belief network and initialize with random weights.
            //DeepBeliefNetwork network = new DeepBeliefNetwork(inputs.First().Length, 10, 10);
            //DeepBeliefNetwork network = new DeepBeliefNetwork(inputs.First().Length, 28, 14, 4, 2);
            DeepBeliefNetwork network = new DeepBeliefNetwork(inputs.First().Length, 8, 4, 2);
            new GaussianWeights(network, 0.1).Randomize();
            network.UpdateVisibleWeights();

            // Setup the learning algorithm.
            DeepBeliefNetworkLearning teacher = new DeepBeliefNetworkLearning(network)
            {
                Algorithm = (h, v, i) => new ContrastiveDivergenceLearning(h, v)
                {
                    LearningRate = 0.1,
                    Momentum = 0.5,
                    Decay = 0.001,
                }
            };

            // Setup batches of input for learning.
            int batchCount = Math.Max(1, inputs.Length / 100);
            // Create mini-batches to speed learning.
            int[] groups = Classes.Random(inputs.Length, batchCount);
            double[][][] batches = Classes.Separate(inputs, groups);
            // Learning data for the specified layer.
            double[][][] layerData;

            // Unsupervised learning on each hidden layer, except for the output layer.
            for (int layerIndex = 0; layerIndex < network.Machines.Count - 1; layerIndex++)
            {
                teacher.LayerIndex = layerIndex;
                layerData = teacher.GetLayerInput(batches);
                for (int i = 0; i < 200; i++)
                {
                    double error = teacher.RunEpoch(layerData) / inputs.Length;
                    if (i % 10 == 0)
                    {
                        Console.WriteLine(i + ", Error = " + error);
                    }
                }
            }

            // Supervised learning on entire network, to provide output classification.
            var teacher2 = new BackPropagationLearning(network)
            {
                LearningRate = 0.1,
                Momentum = 0.5
            };

            // Run supervised learning.
            for (int i = 0; i < 500; i++)
            {
                double error = teacher2.RunEpoch(inputs, outputs) / inputs.Length;
                if (i % 10 == 0)
                {
                    Console.WriteLine(i + ", Error = " + error);
                }
            }

            // Test the resulting accuracy.
            int correct = 0;
            for (int i = 0; i < testInputs.Length; i++)
            {
                double[] outputValues = network.Compute(testInputs[i]);
                Console.WriteLine($"outputValues:{outputValues[0]} {outputValues[1]}");
                if (DataManager.FormatOutputResult(outputValues) == DataManager.FormatOutputResult(testOutputs[i]))
                {
                    Console.WriteLine(string.Format("expected{0} actual:{1}", DataManager.FormatOutputResult(testOutputs[i]), DataManager.FormatOutputResult(outputValues)));
                    correct++;
                }
            }

            Console.WriteLine("Correct " + correct + "/" + testInputs.Length + ", " + Math.Round(((double)correct / (double)testInputs.Length * 100), 2) + "%");
            Console.WriteLine("Now saving ...");
            var filePath = string.Format(LearningConfig.NetworkSavePath + @"/{0}.bin", DateTime.Now.ToString("yyyyMMddhhmmss"));
            network.Save(filePath);
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

        static void __Main(string[] args)
        {
            var network = Serializer.Load<DeepBeliefNetwork>(string.Format(LearningConfig.NetworkSavePath + @"/{0}.bin", "20180414014723"));
            double[][] inputs;
            double[][] outputs;
            double[][] testInputs;
            double[][] testOutputs;

            var data = BuildLearningData();
            inputs = data.Inputs;
            outputs = data.Outputs;
            int separateIndex = (int)Math.Floor(inputs.Count() * 0.7);

            testInputs = inputs.Skip(separateIndex).ToArray();
            testOutputs = outputs.Skip(separateIndex).ToArray();

            // Test the resulting accuracy.
            int correct = 0;
            for (int i = 0; i < testInputs.Length; i++)
            {
                double[] outputValues = network.Compute(testInputs[i]);
                if (DataManager.FormatOutputResult(outputValues) == DataManager.FormatOutputResult(testOutputs[i]))
                {
                    Console.WriteLine(string.Format("expected{0} actual:{1}", DataManager.FormatOutputResult(testOutputs[i]), DataManager.FormatOutputResult(outputValues)));
                    correct++;
                }
            }

            Console.WriteLine("Correct " + correct + "/" + testInputs.Length + ", " + Math.Round(((double)correct / (double)testInputs.Length * 100), 2) + "%");
            Console.Write("Press any key to quit ..");
            Console.ReadKey();
        }

        static void _Main(string[] args)
        {
            // トレーニングデータ
            double[][] inputs = {
                new double[] { 6,20,3,3,0,0,15,10,8,1},
                new double[] { 4,20,8,23,1,45,20,7,3,0},
                new double[] { 6,3,20,20,2,20,8,5,6,0},
                new double[] { 30,30,5,1,1,4,8,30,20,1},
                new double[] { 23,16,9,5,0,0,20,30,18,0},
                new double[] { 10,8,40,33,0,10,30,2,30,2},
                new double[] { 40,50,30,10,1,30,40,9,0,0},
                new double[] { 50,60,0,0,0,3,8,20,40,2},
                //new double[] { 60,10,10,15,0,10,10,25,25,1},
            };
            double[][] outputs = {
                new double[] { 1, 0 },
                new double[] { 0, 1 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 0 },
                new double[] { 0, 1 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                //new double[] { 1, 0 },
            };

            //double[][] inputs = {
            //    new double[] { 1, 1, 1, 0, 0, 0, 0 },
            //    new double[] { 1, 0, 1, 0, 0, 0, 0 },
            //    new double[] { 1, 1, 1, 0, 0, 0, 0 },
            //    new double[] { 0, 0, 1, 1, 1, 0, 0 },
            //    new double[] { 0, 0, 1, 1, 0, 0, 0 },
            //    new double[] { 0, 0, 1, 1, 1, 0, 0 },
            //    new double[] { 1, 1, 0, 1, 0, 0, 0 },
            //};
            //double[][] outputs = {
            //    new double[] { 1, 0 },
            //    new double[] { 1, 0 },
            //    new double[] { 1, 0 },
            //    new double[] { 0, 1 },
            //    new double[] { 0, 1 },
            //    new double[] { 0, 1 },
            //    new double[] { 1, 0 },
            //};

            // ネットワークの生成
            var network = new DeepBeliefNetwork(
                inputsCount: inputs.First().Length,         // 入力層の次元
                hiddenNeurons: new int[] { 5, 2 }); // 隠れ層と出力層の次元

            // DNNの学習アルゴリズムの生成
            var teacher = new DeepNeuralNetworkLearning(network)
            {
                Algorithm = (ann, i) => new ParallelResilientBackpropagationLearning(ann),
                LayerIndex = network.Machines.Count - 1,
            };

            // 5000回繰り返し学習
            var layerData = teacher.GetLayerInput(inputs);
            for (int i = 0; i < 5000; i++)
                teacher.RunEpoch(layerData, outputs);

            // 重みの更新
            network.UpdateVisibleWeights();

            // 学習されたネットワークでテストデータが各クラスに属する確率を計算
            //double[] input = { 1, 1, 1, 1, 0, 0, 0 };
            double[] input = { 60, 10, 10, 15, 0, 10, 10, 25, 25, 1 };
            var output = network.Compute(input);

            //  一番確率の高いクラスのインデックスを得る
            var max = output.Max();
            var index = -1;
            for (var i = 0; i < output.Count(); i++)
            {
                if (output[i] == max)
                {
                    index = i;
                    break;
                }
            }

            // 結果出力
            Console.WriteLine("class : {0}", index);
            foreach (var o in output)
            {
                Console.Write("{0} ", o);
            }

            while (true)
            {
                ;
            }
        }
    }
}
