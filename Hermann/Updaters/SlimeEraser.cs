using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 消済スライムとしてマーキングしたスライムの消去機能を提供します。
    /// </summary>
    public class SlimeEraser : IFieldUpdatable
    {
        /// <summary>
        /// 消済スライムとしてマーキングしたスライムを消去します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            var player = context.OperationPlayer;

            // 消済スライムを消去する
            for(var i = 0; i < FieldContextConfig.FieldUnitCount; i++)
            {
                context.SlimeFields[(int)player][Slime.Erased][i] = 0u;
            }
        }
    }
}
