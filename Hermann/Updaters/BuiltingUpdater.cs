using Hermann.Collections;
using Hermann.Contexts;
using Hermann.Helpers;
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
            Debug.Assert(
                (context.Ground[(int)Player.Index.First].Value && context.BuiltRemainingTime[(int)Player.Index.First] <= 0L) ||
                (context.Ground[(int)Player.Index.Second].Value && context.BuiltRemainingTime[(int)Player.Index.Second] <= 0L),
                "どちらかのプレイヤの接地がtrueで設置残タイムが0秒以下であることが呼び出しの前提条件です。");

            var player = context.OperationPlayer;

            // Nextスライムを移動可能スライムにセットする
            // フィールド上には移動可能スライムと同じ位置に通常のスライムが配置済なので、移動可能スライムの情報を書き換えれば通常のスライムに変わることになる
            FieldContextHelper.SetMovableSlimeInitialPosition(
                context,
                player,
                context.NextSlimes[(int)player][(int)NextSlime.Index.First],
                context.MovableSlimes[(int)player]);
        }
    }
}
