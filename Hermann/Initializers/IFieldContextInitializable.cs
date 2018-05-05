using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Initializers
{
    /// <summary>
    /// フィールド状態に対する初期化機能を提供します。
    /// </summary>
    public interface IFieldContextInitializable : IInitializable<FieldContext>
    {
    }
}
