using Hermann.Contexts;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Hermann.Helpers;
using Hermann.Analyzers;
using Hermann.Di;

namespace Hermann.Api.Senders
{
    /// <summary>
    /// 標準テキスト形式文字列の送信機能を提供します。
    /// </summary>
    public class SimpleTextSender : FieldContextSender<string>
    {
        /// <summary>
        /// フィールド状態分析機能
        /// </summary>
        private FieldAnalyzer FieldAnalyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleTextSender()
        {
            this.FieldAnalyzer = DiProvider.GetContainer().GetInstance<FieldAnalyzer>();
        }

        /// <summary>
        /// 状態を文字列に変換し送信します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>フィールドの状態を表した文字列</returns>
        public override string Send(FieldContext context)
        {
            return this.FieldAnalyzer.Analyze(context);
        }
    }
}
