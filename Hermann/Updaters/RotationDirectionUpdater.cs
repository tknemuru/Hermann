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
    /// 回転方向の更新機能を提供します。
    /// </summary>
    public sealed class RotationDirectionUpdater : IFieldUpdatable
    {
        /// <summary>
        /// 回転方向を更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            var player = context.OperationPlayer;

            switch (context.RotationDirection[(int)player])
            {
                case Direction.Right:
                    context.RotationDirection[(int)player] = Direction.Down;
                    break;
                case Direction.Down:
                    context.RotationDirection[(int)player] = Direction.Left;
                    break;
                case Direction.Left:
                    context.RotationDirection[(int)player] = Direction.Up;
                    break;
                case Direction.Up:
                    context.RotationDirection[(int)player] = Direction.Right;
                    break;
            }
        }
    }
}
