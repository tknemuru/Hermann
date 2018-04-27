using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Generators
{
    /// <summary>
    /// 生成機能を提供します。
    /// </summary>
    /// <typeparam name="T">生成するオブジェクトの型</typeparam>
    public interface IGeneratable<out T>
    {
        /// <summary>
        /// オブジェクトを生成します。
        /// </summary>
        /// <returns>オブジェクト</returns>
        T GetNext();
    }

    /// <summary>
    /// 生成機能を提供します。
    /// </summary>
    /// <typeparam name="TIn">生成のインプットとして用いるオブジェクトの型</typeparam>
    /// <typeparam name="TOut">生成するオブジェクトの型</typeparam>
    public interface IGeneratable<in TIn, out TOut>
    {
        /// <summary>
        /// オブジェクトを生成します。
        /// </summary>
        /// <returns>オブジェクト</returns>
        TOut GetNext(TIn input);
    }
}
