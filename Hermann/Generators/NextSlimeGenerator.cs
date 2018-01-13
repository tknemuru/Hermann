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
    /// NEXTスライムの生成機能を提供します。
    /// </summary>
    public abstract class NextSlimeGenerator : IGeneratable<Slime[]>
    {
        /// <summary>
        /// 使用スライム
        /// </summary>
        public Slime[] UsingSlime { get; set; }

        /// <summary>
        /// NEXTスライムを生成します。
        /// </summary>
        /// <returns></returns>
        public Slime[] GetNext()
        {
            if (null == this.UsingSlime || this.UsingSlime.Length != FieldContextConfig.UsingSlimeCount)
            {
                throw new InvalidOperationException("使用スライムが未設定か、不正です。");
            }

            var slimes = GetNextSlime();

            Debug.Assert(slimes.All(s => this.UsingSlime.Contains(s)), "使用スライムに含まれていないスライムが存在します。");

            return slimes;
        }

        /// <summary>
        /// NEXTスライムを生成します。
        /// </summary>
        /// <returns>NEXTスライム</returns>
        protected abstract Slime[] GetNextSlime();
    }
}
