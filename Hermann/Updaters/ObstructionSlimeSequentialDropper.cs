using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライムのシーケンシャルな落下機能を提供します。
    /// </summary>
    public class ObstructionSlimeSequentialDropper : ObstructionSlimeDropper
    {
        /// <summary>
        /// 次に落下させる列を取得します。
        /// </summary>
        /// <param name="ranks">列の候補</param>
        /// <returns>次に落下させる列</returns>
        protected override int GetNext(IEnumerable<int> ranks, int lastRank)
        {
            return this.GetSeaquentialNext(ranks, lastRank);
        }
    }
}
