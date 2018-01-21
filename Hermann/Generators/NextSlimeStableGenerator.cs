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
    /// 指定したNEXTスライムを生成する機能を提供します。
    /// </summary>
    public class NextSlimeStableGenerator : NextSlimeGenerator
    {
        /// <summary>
        /// NEXTスライム
        /// </summary>
        private Slime[] NextSlimes { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nextSlimes">NEXTスライム</param>
        public NextSlimeStableGenerator(Slime[] nextSlimes)
        {
            Debug.Assert(nextSlimes.Length == NextSlime.Length, string.Format("NEXTスライムの要素数が不正です。数：{0}", nextSlimes.Length));

            this.NextSlimes = nextSlimes;
        }

        /// <summary>
        /// NEXTスライムを生成します。
        /// </summary>
        /// <returns>NEXTスライム</returns>
        protected override Slime[] GetNextSlime()
        {
            return this.NextSlimes;
        }
    }
}
