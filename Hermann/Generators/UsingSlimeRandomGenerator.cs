using Hermann.Contexts;
using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Generators
{
    /// <summary>
    /// 使用スライムのランダムな生成機能を提供します。
    /// </summary>
    public class UsingSlimeRandomGenerator : UsingSlimeGenerator
    {
        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UsingSlimeRandomGenerator()
        {
            this.RandomGen = new Random();
        }

        /// <summary>
        /// 使用スライムを生成します。
        /// </summary>
        /// <returns>使用スライム</returns>
        public override Slime[] GetNext()
        {
            var slimes = ExtensionSlime.Slimes.Where(s => s != Slime.Obstruction);
            var index = this.RandomGen.Next(slimes.Count() - 1);
            var exclusionSlime = slimes.ElementAt(index);
            slimes = slimes.Where(s => s != exclusionSlime);
            Debug.Assert(slimes.Count() == FieldContextConfig.UsingSlimeCount, "使用スライムの数が不正です。数：" + slimes.Count());

            return slimes.ToArray();
        }
    }
}
