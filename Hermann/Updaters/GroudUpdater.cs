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
    /// 接地・設置に関する情報の更新機能を提供します。
    /// </summary>
    public class GroudUpdater : IUpdatable
    {
        /// <summary>
        /// 接地・設置に関する情報を更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Update(FieldContext context)
        {
            
        }

        private void Update(FieldContext context, int player)
        {
            // 接地済か？
            if (context.Ground[player])
            {
                // 設置残タイムが残っていたら何もせずに処理終了
                if (context.BuiltRemainingTime[player] > 0)
                {
                    return;
                }

                // 設置残タイムが0以下
                // １．移動可能スライムの情報をもとに、移動不可能なスライムの情報を更新する
                foreach (var movable in context.MovableSlimes[player])
                {
                    Debug.Assert((context.SlimeFields[player][movable.Slime][movable.Index] &= 1u << movable.Position) == 0u, string.Format("設置場所に既にスライムが存在しています。{0}", movable));
                    context.SlimeFields[player][movable.Slime][movable.Index] |= 1u << movable.Position;
                }

                // ２．移動可能スライムをクリアする
                context.MovableSlimes[player] = new MovableSlime[MovableSlime.Length];
            }
        }
    }
}
