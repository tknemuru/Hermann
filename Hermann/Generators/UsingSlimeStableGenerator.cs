using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Generators
{
    /// <summary>
    /// 指定した使用スライムの生成機能を提供します。
    /// </summary>
    public class UsingSlimeStableGenerator : UsingSlimeGenerator
    {
        /// <summary>
        /// 使用スライム
        /// </summary>
        private Slime[] UsingSlimes { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="usingSlimes">使用スライム</param>
        public UsingSlimeStableGenerator(Slime[] usingSlimes)
        {
            Debug.Assert(usingSlimes.Count() == FieldContextConfig.UsingSlimeCount, "使用スライムの数が不正です。数：" + usingSlimes.Count());
            this.UsingSlimes = usingSlimes;
        }

        /// <summary>
        /// 使用スライムを生成します。
        /// </summary>
        /// <returns>使用スライム</returns>
        public override Slime[] GetNext()
        {
            return this.UsingSlimes;
        }
    }
}
