using Hermann.Collections;
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
            var player = context.OperationPlayer;
            // 消す対象のスライムを消済スライムに変換する
            var erasedSlime = 0u;
            // １．縦4
            uint vertical4 = 0x04040404u;
            var field = context.SlimeFields[(int)player].Value[Slime.Red];
            erasedSlime |= field[1] & vertical4;
            vertical4 <<= 1;
            erasedSlime |= field[1] & vertical4;

            context.SlimeFields[(int)player].Value[Slime.Erased][1] = erasedSlime;
            context.SlimeFields[(int)player].Value[Slime.Red][1] &= ~erasedSlime;

            // 連鎖回数をインクリメントする

            // 消す対象が存在しなかった場合は連鎖回数を0に戻す
        }
    }
}
