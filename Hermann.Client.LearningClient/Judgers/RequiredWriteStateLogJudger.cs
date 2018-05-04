using System;
using Hermann.Contexts;

namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// 状態ログ書き込みが必要かどうかを判定する機能を提供します。
    /// </summary>
    public abstract class RequiredWriteStateLogJudger : IJudgeable<RequiredWriteStateLogJudger.Param>
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
        /// ログ書き込みが必要かどうかを判定します。
        /// </summary>
        /// <returns>ログ書き込みが必要かどうか</returns>
        /// <param name="param">パラメータ</param>
        public abstract bool Judge(Param param);
    }
}
