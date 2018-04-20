using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning.Learners
{
    public class DeepBeliefNetworkLearner
    {
        public void Learn(double[][] inputs, double[][] outputs)
        {
            double[][] testInputs;
            double[][] testOutputs;
            int separateIndex = (int)Math.Floor(inputs.Count() * 0.7);
            //int separateIndex = 8;

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
            DeepBeliefNetwork network = new DeepBeliefNetwork(inputs.First().Length, 8, 2);
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
        }
    }
}
