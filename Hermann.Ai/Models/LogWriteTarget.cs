using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Models
{
    /// <summary>
    /// ログ書き込み対象
    /// </summary>
    public enum LogWriteTarget
    {
        /// <summary>
        /// 状態
        /// </summary>
        State,

        /// <summary>
        /// 勝敗結果
        /// </summary>
        WinResult,
    }
}
