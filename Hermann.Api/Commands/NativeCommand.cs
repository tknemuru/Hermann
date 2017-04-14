using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Commands
{
    /// <summary>
    /// Hermannのネイティブコマンド
    /// </summary>
    public class NativeCommand
    {
        /// <summary>
        /// Hermannに対する命令
        /// </summary>
        public Command Command { get; set; }

        /// <summary>
        /// フィールド状態
        /// </summary>
        public FieldContext Context { get; set; }
    }
}
