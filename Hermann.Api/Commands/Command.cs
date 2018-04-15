using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Commands
{
    /// <summary>
    /// Hermannに対する命令
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// ゲーム開始
        /// </summary>
        Start,

        /// <summary>
        /// スライムを動かす
        /// </summary>
        Move,

        /// <summary>
        /// AIがスライムを動かす
        /// </summary>
        AiMove,
    }
}
