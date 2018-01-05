using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 設置に関するフィールドの更新機能を提供します。
    /// </summary>
    public class BuiltingUpdater : IFieldUpdatable
    {
        public void Update(FieldContext context)
        {
            //Debug.Assert((context.Ground[(int)Player.Index.First] && context.BuiltRemainingTime[(int)Player.Index.First].Value == 0L) ||
            //    (context.Ground[(int)Player.Index.Second] && context.BuiltRemainingTime[(int)Player.Index.Second].Value == 0L),
            //    "どちらかのプレイヤの接地がtrueで設置残タイムが0秒であることが呼び出しの前提条件です。");

            // 移動可能スライムを通常のスライムに変換する
        }
    }
}
