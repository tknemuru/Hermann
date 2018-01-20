using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers.Fields
{
    /// <summary>
    /// フィールド状態の分析機能を提供します。
    /// </summary>
    /// <typeparam name="TOut">分析結果</typeparam>
    public interface IFieldAnalyzable<TOut> : IAnalyzable<FieldContext, TOut>
    {
        /// <summary>
        /// フィールド状態を分析した結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>分析結果</returns>
        TOut Analyze(FieldContext context);
    }
}
