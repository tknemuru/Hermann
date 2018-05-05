using System;
namespace Hermann.Analyzers
{
    /// <summary>
    /// 分析機能を提供します。
    /// </summary>
    public interface IAnalyzable<in TIn, out TOut>
    {
        /// <summary>
        /// 分析を実行します。
        /// </summary>
        /// <returns>分析結果</returns>
        /// <param name="input">入力情報</param>
        TOut Analyze(TIn input);
    }
}
