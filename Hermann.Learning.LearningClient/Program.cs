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
using Hermann.Learning.LearningClient.Excecuter;
using Hermann.Learning.Models;
using Hermann.LearningClient.Di;
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
            try
            {
                LearningClientDiProvider.GetContainer().GetInstance<LearningExecuter>().Execute(args);
                //LearningClientDiProvider.GetContainer().GetInstance<PatternIndexGenerator>().Execute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
