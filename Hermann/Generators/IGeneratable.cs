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
}
