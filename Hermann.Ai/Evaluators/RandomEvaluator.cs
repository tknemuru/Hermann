using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// ランダムに評価する機能を提供します。
    /// </summary>
    public sealed class RandomEvaluator : IEvaluable<RandomEvaluator.Param, double>
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 基底評価値
            /// </summary>
            public double BaseValue { get; set; }

            /// <summary>
            /// 評価を行う頻度（10回中何回評価を行うか）
            /// </summary>
            public int EvaluateFrequency { get; set; }

            /// <summary>
            /// 評価値の幅
            /// </summary>
            public int ValueRange { get; set; }
        }

        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RandomEvaluator()
        {
            this.RandomGen = new Random();
        }

        /// <summary>
        /// ランダムな評価を行います。
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns>評価値</returns>
        public double Evaluate(Param param)
        {
            var value = 0.0d;

            // 評価を行うか判定
            var freq = this.RandomGen.Next(10);
            if(param.EvaluateFrequency < freq)
            {
                return value;
            }

            // 評価を行う
            var range = this.RandomGen.Next(1, param.ValueRange);
            value = param.BaseValue * range;

            return value;
        }
    }
}
