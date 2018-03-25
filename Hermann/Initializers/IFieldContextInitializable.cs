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
        /// <summary>
        /// 依存する機能を注入します。
        /// </summary>
        /// <param name="nextSlimeGen">Nextスライム生成機能</param>
        /// <param name="movableUp">移動可能スライム更新機能</param>
        /// <param name="nextSlimeUp">NEXTスライム更新機能</param>
        void Injection(NextSlimeGenerator nextSlimeGen, MovableSlimesUpdater movableUp, NextSlimeUpdater nextSlimeUp);
    }
}
