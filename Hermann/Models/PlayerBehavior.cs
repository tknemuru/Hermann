using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Models
{
    /// <summary>
    /// プレイヤの行動
    /// </summary>
    public enum PlayerBehavior
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,

        /// <summary>
        /// 移動
        /// </summary>
        Move,

        /// <summary>
        /// 移動可能スライムを通常スライムに変換
        /// </summary>
        ChangeMovableSlimes,

        /// <summary>
        /// 削除対象スライムをマーキング
        /// </summary>
        MarkErasingSlime,

        /// <summary>
        /// おじゃまスライム算出
        /// </summary>
        CalcObstructionSlime,

        /// <summary>
        /// おじゃまスライム落下
        /// </summary>
        Drop,
    }
}
