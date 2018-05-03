using Hermann.Ai.Providers;
using Hermann.Helpers;
using Hermann.Ai.Models;
using Hermann.Ai.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.LearningClient.Excecuter
{
    /// <summary>
    /// パターンのインデックス生成機能を提供します。
    /// </summary>
    public class PatternIndexGenerator : IExecutable
    {
        /// <summary>
        /// インデックス生成処理を実行します。
        /// </summary>
        /// <param name="args">パラメータ</param>
        public void Execute(string[] args)
        {
            // 全パターンを取得する
            var patterns = PatternProvider.Patterns;

            foreach(var pattern in patterns)
            {
                Console.WriteLine($"{pattern.GetName()} start");
                var def = AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(pattern);
                var count = 0;
                var dic = new Dictionary<uint, int>();
                dic.Add(0u, count);
                count++;

                for(var u = uint.MinValue; u < uint.MaxValue; u++)
                {
                    if(u % 100000000 == 0)
                    {
                        Console.WriteLine($"{u} executing...");
                    }
                    var maskedValue = u & def.PatternDigit;
                    if(maskedValue > 0u && !dic.ContainsKey(maskedValue))
                    {
                        dic.Add(maskedValue, count);
                        count++;
                    }
                }
                Console.WriteLine($"{pattern.GetName()} end");

                // ファイルに出力する
                def.WriteIndex(dic);
            }
        }
    }
}
