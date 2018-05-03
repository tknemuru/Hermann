using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Environments;

namespace Hermann.Ai
{
    /// <summary>
    /// 学習に関する設定情報を保持します。
    /// </summary>
    public static class LearningConfig
    {
        /// <summary>
        /// ログ出力パス
        /// </summary>
        public static readonly string LogOutputPath = $"{EnvConfig.GetRootDir()}/Hermann.Ai/learninginputs";

        /// <summary>
        /// 学習済機能の保存先パス
        /// </summary>
        public static readonly string LearnerSavePath = $"{EnvConfig.GetRootDir()}/Hermann.Ai/binarys";
    }
}
