using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helper
{
    /// <summary>
    /// デバッグに関する補助機能を提供します。
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// フィールド状態分析機能
        /// </summary>
        private static FieldAnalyzer FieldAnalyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static DebugHelper()
        {
            FieldAnalyzer = DiProvider.GetContainer().GetInstance<FieldAnalyzer>();
        }

        /// <summary>
        /// フィールド状態を示す文字列を取得します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>フィールド状態を示す文字列</returns>
        public static string FieldToString(FieldContext context)
        {
            return FieldAnalyzer.Analyze(context);
        }

        /// <summary>
        /// uintの値をフィールド単位の文字列に変換します。
        /// </summary>
        /// <param name="value">フィールド単位の値</param>
        /// <returns>フィールド単位を示す文字列</returns>
        public static string ConvertUintToFieldUnit(uint value)
        {
            var field = Convert.ToString(value, 2);
            field = field.PadLeft(FieldContextConfig.FieldUnitBitCount, '0');
            var fieldStates = field.ToCharArray().Reverse().ToArray();
            var result = new StringBuilder();

            for (var line = 1; line < 5; line++)
            {
                result.Append(Environment.NewLine);
                for (var i = ((line * 8) - 1); i > (((line - 1) * 8) - 1); i--)
                {
                    if ((i % 8) == 1)
                    {
                        result.Append("|");
                    }
                    result.Append(fieldStates[i]);
                }
            }

            return result.ToString();
        }
    }
}
