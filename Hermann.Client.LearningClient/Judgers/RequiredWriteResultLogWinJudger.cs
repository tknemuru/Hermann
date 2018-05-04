using System;
using Hermann.Contexts;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// 勝敗結果による結果ログの書き込み要否判定機能を提供します。
    /// </summary>
    public class RequiredWriteResultLogWinJudger : RequiredWriteResultLogJudger
    {
        /// <summary>
        /// 勝敗結果によって結果ログを書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns>結果ログを書き込む必要があるかどうか</returns>
        /// <param name="param">パラメータ</param>
        public override bool Judge(RequiredWriteResultLogJudger.Param param)
        {
            var player = param.Context.OperationPlayer;
            return (param.Context.FieldEvent[player.ToInt()] == FieldEvent.End);
        }
    }
}
