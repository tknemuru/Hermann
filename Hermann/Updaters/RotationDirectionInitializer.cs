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
    /// 回転方向の初期化機能を提供します。
    /// </summary>
    public sealed class RotationDirectionInitializer : IFieldUpdatable
    {
        /// <summary>
        /// 回転方向の初期値
        /// </summary>
        private const Direction InitialDirection = Direction.Right;

        /// <summary>
        /// 回転方向を初期化します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            context.RotationDirection[(int)context.OperationPlayer] = InitialDirection;
        }
    }
}
