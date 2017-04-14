using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Generators
{
    /// <summary>
    /// 使用スライムの生成機能を提供します。
    /// </summary>
    public abstract class UsingSlimeGenerator : IGeneratable<Slime[]>
    {
        /// <summary>
        /// 使用スライムを生成します。
        /// </summary>
        /// <returns>使用スライム</returns>
        public abstract Slime[] GetNext();
    }
}
