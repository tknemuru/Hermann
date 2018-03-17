using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Initializers.Fields
{
    /// <summary>
    /// 設置残タイムの初期化機能を提供します。
    /// </summary>
    public class BuiltRemainingTimeInitializer : IFieldInitializable
    {
        /// <summary>
        /// 設置残タイムの初期化を行います。
        /// </summary>
        /// <param name="context">初期化されたフィールド状態</param>
        public void Initialize(FieldContext context)
        {
            var player = context.OperationPlayer;

            context.BuiltRemainingTime[(int)player] = FieldContextConfig.MaxBuiltRemainingTime;
        }
    }
}
