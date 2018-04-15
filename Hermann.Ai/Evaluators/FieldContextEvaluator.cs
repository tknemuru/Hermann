using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using Hermann.Learning;
using Hermann.Learning.Di;
using System.Diagnostics;
using Hermann.Models;
using Hermann.Learning.Helpers;
using Hermann.Helpers;

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// フィールド状態の評価機能を提供します。
    /// </summary>
    public class FieldContextEvaluator : IEvaluable<FieldContext, double>
    {
        /// <summary>
        /// フィールド状態配列の使用開始インデックス
        /// </summary>
        private const int UsingStartIndex = 1;

        /// <summary>
        /// 評価に使用する学習ネットワーク
        /// </summary>
        private Network Network { get; set; }

        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FieldContextEvaluator()
        {
            this.Network = AiDiProvider.GetContainer().GetInstance<NetworkManager>().GetDeepBeliefNetwork();
            this.RandomGen = new Random();
        }

        /// <summary>
        /// フィールド状態の評価を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double Evaluate(FieldContext context)
        {
            var input = DataConverter.ConvertContextToArray(context).Skip(UsingStartIndex).Select(c => double.Parse(c.ToString())).ToArray();
            var answer = this.Network.Compute(input);
            var correction = 0.1d;
            var correctionTarget = this.RandomGen.Next(Player.Length);
            foreach (var val in input)
            {
                FileHelper.Write($", {val}");
            }
            FileHelper.WriteLine("");
            FileHelper.WriteLine($"answer:{answer[0]}, {answer[1]}");
            answer[correctionTarget] += correction;
            FileHelper.WriteLine($"correction answer:{answer[0]}, {answer[1]}");
            Debug.Assert(answer.Length == Player.Length, "出力層の数が不正です");
            return answer[(int)Player.Index.First];
        }
    }
}
