using System;
using Hermann.Ai.Evaluators;
using Hermann.Contexts;
using Hermann.Models;

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

            /// <summary>
            /// パリティを適用するかどうか
            /// </summary>
            /// <value><c>true</c> if parity; otherwise, <c>false</c>.</value>
            public bool Parity { get; set; } = false;
        }

        /// <summary>
        /// 結果値の評価を実施します。
        /// </summary>
        /// <returns>結果値</returns>
        /// <param name="param">パラメータ</param>
        public abstract double Evaluate(Param param);

        /// <summary>
        /// パリティ値を取得します。
        /// </summary>
        /// <returns>パリティ</returns>
        /// <param name="param">パラメータ</param>
        protected double GetParity(Param param)
        {
            var parity = 1.0d;
            if (!param.Parity)
            {
                return parity;
            }

            return (param.Context.OperationPlayer == Player.Index.First) ? parity : -1.0d;
        }
    }
}
