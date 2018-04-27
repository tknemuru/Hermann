using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Analyzers
{
    /// <summary>
    /// スライム高低差の分析機能を提供します。
    /// </summary>
    public class DifferenceHeightAnalyzer : IPlayerFieldAnalyzable<int>
    {
        public int Analyze(FieldContext context, Player.Index player)
        {
            var min = 99;
            var max = -99;

            for (var column = FieldContextConfig.OneLineBitCount - FieldContextConfig.VerticalLineLength; column < FieldContextConfig.OneLineBitCount; column++)
            {
                var exists = false;
                for (var unit = FieldContextConfig.MaxHiddenUnitIndex + 1; unit < FieldContextConfig.FieldUnitCount; unit++)
                {
                    for (var line = 0; line < FieldContextConfig.FieldUnitLineCount; line++)
                    {
                        exists = FieldContextHelper.ExistsSlime(context, player, unit, column + (line * FieldContextConfig.OneLineBitCount));
                        if (exists)
                        {
                            var top = (unit - FieldContextConfig.MaxHiddenUnitIndex + 1) * FieldContextConfig.FieldUnitLineCount + line;
                            min = Math.Min(top, min);
                            max = Math.Max(top, max);
                            break;
                        }
                    }
                    if (exists)
                    {
                        break;
                    }
                }
            }

            min = min == 99 ? 0 : min;
            max = max == -99 ? 0 : max;
            return max - min;
        }
    }
}
