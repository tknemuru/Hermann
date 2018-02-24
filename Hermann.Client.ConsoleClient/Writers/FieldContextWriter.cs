using Hermann.Contexts;
using Hermann.Client.ConsoleClient.Di;
using Hermann.Api.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hermann.Client.ConsoleClient.Writers
{
    /// <summary>
    /// フィールド状態の出力機能を提供します。
    /// </summary>
    public static class FieldContextWriter
    {
        /// <summary>
        /// 情報を分割する文字列
        /// </summary>
        private const string Separator = "---------------";

        /// <summary>
        /// フィールド情報の送信機能
        /// </summary>
        private static FieldContextSender<string> Sender { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static FieldContextWriter()
        {
            Sender = ConsoleClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
        }

        /// <summary>
        /// フィールド状態の出力を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public static void Write(FieldContext context)
        {
            Console.WriteLine(Separator);
            Console.Write(Sender.Send(context));
            Console.WriteLine(Separator);
        }
    }
}
