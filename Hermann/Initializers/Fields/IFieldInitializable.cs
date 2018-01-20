using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Initializers.Fields
{
    /// <summary>
    /// フィールド状態に対する初期化機能を提供します。
    /// </summary>
    public interface IFieldInitializable : IInitializable<FieldContext>
    {
        /// <summary>
        /// フィールド状態に対する初期化処理を行います。
        /// </summary>
        /// <param name="context">初期化されたフィールド状態</param>
        void Initialize(FieldContext context);
    }
}
