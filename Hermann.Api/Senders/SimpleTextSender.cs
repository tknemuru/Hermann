using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Senders
{
    /// <summary>
    /// 標準テキスト形式文字列の送信機能を提供します。
    /// </summary>
    public class SimpleTextSender : Sender
    {
        /// <summary>
        /// 状態を文字列に変換し送信します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string Send(ulong[] context)
        {
            var command = context[0];
            return "hoge";
        }
    }
}
