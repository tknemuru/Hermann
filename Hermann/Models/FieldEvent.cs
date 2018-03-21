using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Models
{
    /// <summary>
    /// フィールドイベント
    /// </summary>
    public enum FieldEvent
    {
        /// <summary>
        /// 無
        /// </summary>
        None,

        /// <summary>
        /// 連鎖開始
        /// </summary>
        StartChain,

        /// <summary>
        /// 消済マーク
        /// </summary>
        MarkErasing,

        /// <summary>
        /// おじゃまスライム配置
        /// </summary>
        SetObstructions,

        /// <summary>
        /// 削除
        /// </summary>
        Erase,

        /// <summary>
        /// スライム落下
        /// </summary>
        DropSlimes,

        /// <summary>
        /// おじゃまスライム落下
        /// </summary>
        DropObstructions,

        /// <summary>
        /// 準備
        /// </summary>
        NextPreparation,
    }
}
