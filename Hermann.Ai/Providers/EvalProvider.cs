using Hermann.Ai.Evaluators;
using Hermann.Contexts;
using Hermann.Learning.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Providers
{
    /// <summary>
    /// 評価値の提供機能を提供します。
    /// </summary>
    public class EvalProvider
    {
        /// <summary>
        /// 回帰分析によるフィールド評価機能
        /// </summary>
        private LinearRegressionEvaluator LinearRegressionEvaluator { get; set; }

        /// <summary>
        /// ランダムに評価する機能
        /// </summary>
        private RandomEvaluator RandomEvaluator { get; set; }

        /// <summary>
        /// ランダム評価のパラメータ
        /// </summary>
        private RandomEvaluator.Param RandomParam { get; set; }

        /// <summary>
        /// 死活監視評価機能
        /// </summary>
        private AliveEvaluator AliveEvaluator { get; set; }

        /// <summary>
        /// 評価機能種別
        /// </summary>
        public enum Type
        {
            Main,
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EvalProvider()
        {
            this.LinearRegressionEvaluator = AiDiProvider.GetContainer().GetInstance<LinearRegressionEvaluator>();
            this.RandomEvaluator = AiDiProvider.GetContainer().GetInstance<RandomEvaluator>();
            this.RandomParam = new RandomEvaluator.Param()
            {
                BaseValue = 0.0001,
                EvaluateFrequency = 10,
                ValueRange = 100,
            };
            this.AliveEvaluator = AiDiProvider.GetContainer().GetInstance<AliveEvaluator>();
        }

        /// <summary>
        /// 指定した種別の評価値を取得します。
        /// </summary>
        /// <param name="type"> 評価機能種別</param>
        /// <returns>評価値</returns>
        public double GetEval(Type type, FieldContext context)
        {
            double ev = 0.0d;
            switch (type)
            {
                case Type.Main:
                    ev = this.GetMainEval(context);
                    break;
                default:
                    throw new ArgumentException("ベクトル種別が不正です");
            }
            return ev;
        }

        /// <summary>
        /// Mainの評価値を取得します。
        /// </summary>
        /// <returns>評価値</returns>
        private double GetMainEval(FieldContext context)
        {
            var ev = this.LinearRegressionEvaluator.Evaluate(context);
            ev += this.AliveEvaluator.Evaluate(context);
            ev += this.RandomEvaluator.Evaluate(this.RandomParam);
            return ev;
        }
    }
}
