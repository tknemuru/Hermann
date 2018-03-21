using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライムのランダムな配置機能を提供します。
    /// </summary>
    public class ObstructionSlimeRandomSetter : ObstructionSlimeSetter
    {
        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObstructionSlimeRandomSetter()
        {
            this.RandomGen = new Random();
        }

        /// <summary>
        /// 次に配置する列を取得します。
        /// </summary>
        /// <param name="ranks">列の候補</param>
        /// <returns>次に落下させる列</returns>
        protected override int GetNext(IEnumerable<int> ranks, int lastRank)
        {
            var i = this.RandomGen.Next(ranks.Count());
            return ranks.ElementAt(i);
        }
    }
}
