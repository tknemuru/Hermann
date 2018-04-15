using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Evaluators
{
    /// <summary>
    /// 評価機能を提供します。
    /// </summary>
    public interface IEvaluable<TIn, TOut>
    {
        /// <summary>
        /// 評価を行い、結果を返却します。
        /// </summary>
        /// <param name="input">評価対象の入力情報</param>
        /// <returns>評価結果</returns>
        TOut Evaluate(TIn input);
    }
}
