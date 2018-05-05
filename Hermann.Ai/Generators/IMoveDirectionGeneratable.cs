using System;
using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Models;

namespace Hermann.Ai.Generators
{
    /// <summary>
    /// 動かす方向の生成機能を提供します。
    /// </summary>
    public interface IMoveDirectionGeneratable : IGeneratable<FieldContext, Direction>
    {
    }
}
