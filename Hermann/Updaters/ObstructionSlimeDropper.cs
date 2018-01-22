using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    public class ObstructionSlimeDropper : IPlayerFieldUpdatable
    {
        public void Update(FieldContext context, Player.Index player)
        {
            // おじゃまスライムをフィールドに落とす
            var opposite = Player.GetOppositeIndex(player);
            var maxIndex = FieldContextConfig.FieldUnitBitCount - 1;
            var obsSlimes = context.ObstructionSlimes[(int)opposite];
            var updObsSlimes = new Dictionary<ObstructionSlime, int>();

            foreach (var ob in obsSlimes)
            {
                updObsSlimes.Add(ob.Key, 0);
                updObsSlimes[ob.Key] = ob.Value;
                if (ob.Value > 0)
                {
                    for (var i = maxIndex; i >= 0; i--)
                    {
                        context.SlimeFields[(int)opposite][Slime.Obstruction][0] |= 1u << i;
                        updObsSlimes[ob.Key]--;
                        if (updObsSlimes[ob.Key] <= 0)
                        {
                            break;
                        }
                    }
                }
            }

            context.ObstructionSlimes[(int)opposite] = updObsSlimes;
        }
    }
}
