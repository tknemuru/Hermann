using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Serchers
{
    /// <summary>
    /// 探索機能を提供します。
    /// </summary>
    public interface ISerchable<TIn, TOut>
    {
        /// <summary>
        /// 入力情報を元に探索し、結果を返却します。
        /// </summary>
        /// <param name="input">入力情報</param>
        /// <returns>探索結果</returns>
        TOut Search(TIn input);
    }
}
