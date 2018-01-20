using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers
{
    /// <summary>
    /// 分析機能を提供します。
    /// </summary>
    /// <typeparam name="TIn">分析対象の情報</typeparam>
    /// <typeparam name="TOut">分析結果</typeparam>
    public interface IAnalyzable<TIn, TOut>
    {
        /// <summary>
        /// 渡された情報を分析した結果を返却します。
        /// </summary>
        /// <param name="input">入力情報</param>
        /// <returns>分析結果</returns>
        TOut Analyze(TIn input);
    }
}
