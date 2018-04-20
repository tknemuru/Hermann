using Hermann.Contexts;
using Hermann.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Exceptions
{
    public class FieldException : Exception
    {
        /// <summary>
        /// フィールド状態
        /// </summary>
        public FieldContext Context { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="context">フィールド状態</param>
        public FieldException(string message, FieldContext context)
            : base(BuildMessage(message, context))
        {
            this.Context = context;
        }

        /// <summary>
        /// メッセージを組み立てます。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="context">フィールド状態</param>
        /// <returns>メッセージ</returns>
        private static string BuildMessage(string message, FieldContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine(message);
            sb.AppendLine(DebugHelper.FieldToString(context));
            return sb.ToString();
        }
    }
}
