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
        None,

        StartChain,

        MarkErasing,

        Erase,

        DropObstructions,
    }
}
