using System;
using Hermann.Contexts;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// 結果ログ書き込みが必要かどうかを判定する機能を提供します。
    /// </summary>
    public abstract class RequiredWriteResultLogJudger : IJudgeable<RequiredWriteResultLogJudger.Param>
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 前回のフィールド状態
            /// </summary>
            public FieldContext LastContext { get; set; }

            /// <summary>
            /// 現在のフィールド状態
            /// </summary>
            public FieldContext Context { get; set; }
        }

        /// <summary>
        /// 結果ログ書き込みが必要かどうかを判定します。
        /// </summary>
        /// <returns>結果ログ書き込みが必要かどうか</returns>
        /// <param name="param">パラメータ</param>
        public abstract bool Judge(RequiredWriteResultLogJudger.Param param);
    }
}
