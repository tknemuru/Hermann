using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers.Fields
{
    /// <summary>
    /// 移動可能スライムの状態の分析機能を提供します。
    /// </summary>
    public class MovableSlimeStateAnalyzer : IFieldAnalyzable<MovableSlimeStateAnalyzer.Status>
    {
        /// <summary>
        /// 移動可能スライムの状態
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 未定義
            /// </summary>
            Undefined,

            /// <summary>
            /// 移動中
            /// </summary>
            Moving,

            /// <summary>
            /// 接地中
            /// </summary>
            Grounding,

            /// <summary>
            /// 設置済
            /// </summary>
            HasBuilt,
        }

        /// <summary>
        /// 移動可能スライムの状態を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>移動可能スライムの状態</returns>
        public Status Analyze(FieldContext context)
        {
            var player = context.OperationPlayer;
            var status = Status.Undefined;

            // 接地していなければ移動中
            if (!context.Ground[(int)player])
            {
                status = Status.Moving;
                return status;
            }

            if (context.BuiltRemainingTime[(int)player] <= 0)
            {
                status = Status.HasBuilt;
            }
            else
            {
                status = Status.Grounding;
            }

            Debug.Assert(status != Status.Undefined, "状態が未定義です。");
            return status;
        }
    }
}
