﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Environments;

namespace Hermann.Learning
{
    /// <summary>
    /// 学習に関する設定情報を保持します。
    /// </summary>
    public static class LearningConfig
    {
        /// <summary>
        /// ログ出力パス
        /// テスト
        /// </summary>
        public static readonly string LogOutputPath = $"{EnvConfig.GetRootDir()}/Hermann.Client.NativeClient/log/learning";

        /// <summary>
        /// 学習済機能の保存先パス
        /// </summary>
        public static readonly string LearnerSavePath = $"{EnvConfig.GetRootDir()}/Hermann.Learning.LearningClient/binarys";
    }
}
