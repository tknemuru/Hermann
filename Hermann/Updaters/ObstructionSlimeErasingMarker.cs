using Hermann.Contexts;
using Hermann.Helper;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライムを消し済にマーキングする機能を提供します。
    /// </summary>
    public class ObstructionSlimeErasingMarker : IPlayerFieldUpdatable
    {
        /// <summary>
        /// おじゃまスライムを消し済にマーキングします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var erased = context.SlimeFields[(int)player][Slime.Erased];
            var obs = context.SlimeFields[(int)player][Slime.Obstruction];
            var move = new MoveHelper.Move();
            var moveFuncs = new List<Func<int, int, MoveHelper.Move, bool>>()
                {
                    MoveHelper.TryMoveUp,
                    MoveHelper.TryMoveDown,
                    MoveHelper.TryMoveRight,
                    MoveHelper.TryMoveLeft
                };
            var updErased = erased.Select(e => e).ToArray();
            var updObs = obs.Select(o => o).ToArray();

            for (var unit = 0; unit < FieldContextConfig.FieldUnitCount; unit++)
            {
                for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
                {
                    if ((erased[unit] & 1u << i) > 0)
                    {
                        // 上下左右のおじゃまスライムを消し済にマーキングする
                        foreach (var func in moveFuncs)
                        {
                            if (func(unit, i, move))
                            {
                                if ((obs[move.Unit] & 1u << move.Index) > 0)
                                {
                                    updObs[move.Unit] &= ~(1u << move.Index);
                                    updErased[move.Unit] |= (1u << move.Index);
                                }
                            }
                        }
                    }
                }
            }

            context.SlimeFields[(int)player][Slime.Erased] = updErased;
            context.SlimeFields[(int)player][Slime.Obstruction] = updObs;
        }
    }
}
