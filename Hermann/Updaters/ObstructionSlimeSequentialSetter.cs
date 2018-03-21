using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライムのシーケンシャルな配置機能を提供します。
    /// </summary>
    public class ObstructionSlimeSequentialSetter : ObstructionSlimeSetter
    {
        /// <summary>
        /// 次に配置する列を取得します。
        /// </summary>
        /// <param name="ranks">列の候補</param>
        /// <returns>次に落下させる列</returns>
        protected override int GetNext(IEnumerable<int> ranks, int lastRank)
        {
            return this.GetSeaquentialNext(ranks, lastRank);
        }
    }
}
