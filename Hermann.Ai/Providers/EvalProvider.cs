using Hermann.Ai.Evaluators;
using Hermann.Contexts;
using Hermann.Ai.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Di;

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
        private Dictionary<AiPlayer.Version, LinearRegressionEvaluator> LinearRegressionEvaluators { get; set; }

        /// <summary>
        /// ランダムに評価する機能
        /// </summary>
        private RandomEvaluator RandomEvaluator { get; set; }

        /// <summary>
        /// ランダム評価のパラメータ
        /// </summary>
        private Dictionary<AiPlayer.Version, RandomEvaluator.Param> RandomParams { get; set; }

        /// <summary>
        /// 死活監視評価機能
        /// </summary>
        private Dictionary<AiPlayer.Version, AliveEvaluator[]> AliveEvaluators { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EvalProvider()
        {
            this.RandomEvaluator = DiProvider.GetContainer().GetInstance<RandomEvaluator>();
            this.AliveEvaluators = new Dictionary<AiPlayer.Version, AliveEvaluator[]>();
            this.LinearRegressionEvaluators = new Dictionary<AiPlayer.Version, LinearRegressionEvaluator>();
            this.RandomParams = new Dictionary<AiPlayer.Version, RandomEvaluator.Param>();

            // V1.0
            var version = AiPlayer.Version.V1_0;
            var evaluator = DiProvider.GetContainer().GetInstance<LinearRegressionEvaluator>();
            evaluator.Inject(version);
            this.LinearRegressionEvaluators.Add(version, evaluator);
            this.RandomParams.Add(version, new RandomEvaluator.Param()
            {
                BaseValue = 0.0001,
                EvaluateFrequency = 10,
                ValueRange = 100,
            });
            var alive = DiProvider.GetContainer().GetInstance<AliveEvaluator>();
            alive.Inject(new AliveEvaluator.Config());
            this.AliveEvaluators.Add(version, new[] { alive });

            // V2.0
            version = AiPlayer.Version.V2_0;
            evaluator = DiProvider.GetContainer().GetInstance<LinearRegressionEvaluator>();
            evaluator.Inject(version);
            this.LinearRegressionEvaluators.Add(version, evaluator);
            this.RandomParams.Add(version, new RandomEvaluator.Param()
            {
                BaseValue = 0.0001,
                EvaluateFrequency = 10,
                ValueRange = 100,
            });
            var alives = new List<AliveEvaluator>();
            alive = DiProvider.GetContainer().GetInstance<AliveEvaluator>();
            alive.Inject(new AliveEvaluator.Config()
            {
                DangerEval = -9999,
            });
            alives.Add(alive);
            alive = DiProvider.GetContainer().GetInstance<AliveEvaluator>();
            alive.Inject(new AliveEvaluator.Config()
            {
                DangerEval = -999,
                DangerIndexes = Enumerable.Range(0, 16).Select(i => i).ToArray(),
            });
            alives.Add(alive);
            this.AliveEvaluators.Add(version, alives.ToArray());
        }

        /// <summary>
        /// 指定した種別の評価値を取得します。
        /// </summary>
        /// <param name="version">バージョン</param>
        /// <param name="context">フィールド状態</param>
        /// <returns>評価値</returns>
        public double GetEval(AiPlayer.Version version, FieldContext context)
        {
            double ev = 0.0d;
            switch (version)
            {
                case AiPlayer.Version.V1_0:
                case AiPlayer.Version.V2_0:
                    ev = this.LinearRegressionEvaluators[version].Evaluate(context);
                    ev += this.AliveEvaluators[version].Sum(e => e.Evaluate(context));
                    ev += this.RandomEvaluator.Evaluate(this.RandomParams[version]);
                    break;
                default:
                    throw new ArgumentException("バージョンが不正です");
            }
            return ev;
        }
    }
}
