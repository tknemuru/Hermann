using System;
using Hermann.Ai.Evaluators;
using Hermann.Contexts;

namespace Hermann.Client.LearningClient.Evaluators
{
    /// <summary>
    /// 結果値の評価機能を提供します。
    /// </summary>
    public abstract class ResultEvaluator : IEvaluable<ResultEvaluator.Param, double>
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 前回のフィールド状態
            /// </summary>
            public FieldContext LastContext { get; set; }

            /// <summary>
            /// 現在のフィールド状態
            /// </summary>
            public FieldContext Context { get; set; }
        }

        /// <summary>
        /// 結果値の評価を実施します。
        /// </summary>
        /// <returns>結果値</returns>
        /// <param name="param">パラメータ</param>
        public abstract double Evaluate(Param param);
    }
}
