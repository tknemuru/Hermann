using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Generators
{
    /// <summary>
    /// スライムを生成します。
    /// </summary>
    public abstract class SlimeGenerator
    {
        /// <summary>
        /// 次のスライムを生成します。
        /// </summary>
        /// <returns></returns>
        public abstract ulong GetNext();
    }
}
