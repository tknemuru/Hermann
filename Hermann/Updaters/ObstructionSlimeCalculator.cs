using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライム数の計算機能を提供します。
    /// </summary>
    public class ObstructionSlimeCalculator : IPlayerFieldUpdatable
    {
        /// <summary>
        /// おじゃまスライム数を算出します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var chain = context.Chain[(int)player];
            if (chain > 0)
            {
                var opposite = player == Player.Index.First ? Player.Index.Second : Player.Index.First;
                context.ObstructionSlimes[(int)opposite][ObstructionSlime.Small] = 6;
            }
        }
    }
}
