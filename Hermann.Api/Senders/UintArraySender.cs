using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Senders
{
    /// <summary>
    /// uint配列に変換した送信機能を提供します。
    /// </summary>
    public class UintArraySender : FieldContextSender<uint[]>
    {
        /// <summary>
        /// 状態をuint配列に変換し送信します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>フィールドの状態を表したuint配列</returns>
        public override uint[] Send(FieldContext context)
        {
            return new uint[1];
        }
    }
}
