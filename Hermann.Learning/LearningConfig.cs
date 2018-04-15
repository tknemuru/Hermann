using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning
{
    /// <summary>
    /// 学習に関する設定情報を保持します。
    /// </summary>
    public static class LearningConfig
    {
        /// <summary>
        /// ログ出力パス
        /// </summary>
        public static string LogOutputPath = @"C:\work\visualstudio\Hermann\Hermann.Client.NativeClient\log\learning";

        /// <summary>
        /// 学習済ネットワークの保存先パス
        /// </summary>
        public static string NetworkSavePath = @"C:\work\visualstudio\Hermann\Hermann.Learning.LearningClient\binarys";
    }
}
