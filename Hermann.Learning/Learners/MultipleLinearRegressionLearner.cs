using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Kernels;
using Accord.Statistics.Models.Regression.Linear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning.Learners
{
    public class MultipleLinearRegressionLearner
    {
        public MultipleLinearRegression Learn(double[][] inputs, double[] outputs)
        {
            var ols = new OrdinaryLeastSquares()
            {
                UseIntercept = true
            };

            // Use Ordinary Least Squares to estimate a regression model
            MultipleLinearRegression regression = ols.Learn(inputs, outputs);

            // As result, we will be given the following:
            //double a = regression.Weights[0]; // a = 0
            //double b = regression.Weights[1]; // b = 0
            //double c = regression.Intercept;  // c = 1

            // This is the plane described by the equation
            // ax + by + c = z => 0x + 0y + 1 = z => 1 = z.

            // We can compute the predicted points using
            double[] predicted = regression.Transform(inputs);

            // And the squared error loss using 
            double error = new SquareLoss(outputs).Loss(predicted);

            // We can also compute other measures, such as the coefficient of determination r²
            double r2 = new RSquaredLoss(numberOfInputs: 2, expected: outputs).Loss(predicted); // should be 1

            // We can also compute the adjusted or weighted versions of r² using
            var r2loss = new RSquaredLoss(numberOfInputs: 2, expected: outputs)
            {
                Adjust = true,
                // Weights = weights; // (if you have a weighted problem)
            };

            double ar2 = r2loss.Loss(predicted); // should be 1

            // Alternatively, we can also use the less generic, but maybe more user-friendly method directly:
            double ur2 = regression.CoefficientOfDetermination(inputs, outputs, adjust: true); // should be 1

            Console.WriteLine("Weights:");
            foreach (var w in regression.Weights){
                Console.WriteLine($",{w}");
            }
            Console.WriteLine("Intercept:");
            Console.WriteLine($",{regression.Intercept}");
            Console.WriteLine($"error:{error}");
            Console.WriteLine($"r2:{r2}");
            Console.WriteLine($"r2loss:{r2loss}");
            Console.WriteLine($"ar2:{ar2}");
            Console.WriteLine($"ur2:{ur2}");

            return regression;
        }
    }
}
