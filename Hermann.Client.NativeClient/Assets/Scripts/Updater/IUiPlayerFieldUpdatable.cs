using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Updater
{
    /// <summary>
    /// 指定したプレイヤのUIフィールドをパラメータ付きで更新する機能を提供します。
    /// </summary>
    public interface IUiPlayerFieldParameterizedUpdatable<TOut, TIn>
    {
        /// <summary>
        /// 指定したプレイヤのUIフィールド状態を更新します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        TOut Update(Player.Index player, TIn param);
    }
}
