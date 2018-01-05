﻿using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// プレイヤごとのフィールド状態の更新機能を提供します。
    /// </summary>
    public interface IPlayerFieldUpdatable
    {
        /// <summary>
        /// 指定したプレイヤのフィールド状態を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        void Update(FieldContext context, Player.Index player);
    }
}
