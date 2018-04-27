using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Learning.Models;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Learning.Helpers;

namespace Hermann.Learning
{
    /// <summary>
    /// ログ出力に関する機能を提供します。
    /// </summary>
    public static class LogWriter
    {
        /// <summary>
        /// ファイルパスの初期値
        /// </summary>
        private static readonly string DefaultFilePath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static LogWriter()
        {
            DefaultFilePath = string.Format(LearningConfig.LogOutputPath +　@"/{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
        }

        /// <summary>
        /// ログを書き込みます。
        /// </summary>
        /// <param name="log">ログ</param>
        public static void WirteLog(string log)
        {
            Console.WriteLine(log);
            FileHelper.WriteLine(log);
        }

        /// <summary>
        /// フィールド状態をログに書き込みます。
        /// </summary>
        /// <param name="state">フィールド状態</param>
        public static void WriteState(double[] status)
        {
            var sb = new StringBuilder();
            sb.Append((int)LogWriteTarget.State);
            foreach (var s in status)
            {
                sb.Append(",");
                sb.Append(s);
            }
            FileHelper.WriteLine(sb.ToString(), DefaultFilePath);
        }

        /// <summary>
        /// 勝敗結果をログに書き込みます。
        /// </summary>
        /// <param name="value">評価点</param>
        public static void WriteWinResult(double value)
        {
            var sb = new StringBuilder();
            sb.Append((uint)LogWriteTarget.WinResult);
            sb.Append(string.Format(",{0}", value));
            FileHelper.WriteLine(sb.ToString(), DefaultFilePath);
        }
    }
}
