using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 消去対象のスライムを消済スライムとしてマーキングする機能を提供します。
    /// </summary>
    public class SlimeErasingMarker : IFieldUpdatable
    {
        public void Update(FieldContext context)
        {
            // 消す対象のスライムを消済スライムに変換する

            // 連鎖回数をインクリメントする

            // 消す対象が存在しなかった場合は連鎖回数を0に戻す
        }
    }
}
