using Hermann.Helpers;
using Hermann.Ai.Di;
using Hermann.Ai.Helpers;
using Hermann.Ai.Learners;
using Hermann.Client.LearningClient.Excecuters;
using Hermann.Ai.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Client.LearningClient.Di;

namespace Hermann.Client.LearningClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LearningClientDiProvider.GetContainer().GetInstance<IExecutable>().Execute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
