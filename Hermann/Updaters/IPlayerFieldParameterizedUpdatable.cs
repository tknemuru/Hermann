using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// フィールド状態をパラメータ付きで更新する機能を提供します。
    /// </summary>
    public interface IPlayerFieldParameterizedUpdatable<T>
    {
        /// <summary>
        /// 指定したプレイヤのフィールド状態を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        void Update(FieldContext context, Player.Index player, T param);
    }
}
