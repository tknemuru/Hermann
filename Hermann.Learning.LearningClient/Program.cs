using Accord.IO;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.Statistics;
using Accord.Statistics.Models.Regression.Linear;
using Hermann.Helpers;
using Hermann.Ai.Di;
using Hermann.Ai.Helpers;
using Hermann.Ai.Learners;
using Hermann.Ai.LearningClient.Excecuter;
using Hermann.Ai.Models;
using Hermann.LearningClient.Di;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.LearningClient
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
