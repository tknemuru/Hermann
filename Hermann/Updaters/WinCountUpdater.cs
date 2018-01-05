using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 勝数の更新機能を提供します。
    /// </summary>
    public class WinCountUpdater : IFieldUpdatable
    {
        public void Update(FieldContext context)
        {
            // 勝敗を判定する
            // 連鎖数が0でなければ、連鎖中なので勝敗判定はしない

            // 勝敗が決まっている場合は、勝ち数をインクリメントする
        }
    }
}
