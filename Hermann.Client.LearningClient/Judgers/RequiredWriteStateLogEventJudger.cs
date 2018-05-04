using System;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// イベントから状態を書き込む必要があるかどうかを判定する機能を提供します。
    /// </summary>
    public class RequiredWriteStateLogEventJudger : RequiredWriteStateLogJudger
    {
        /// <summary>
        /// イベントから状態を書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns>状態を書き込む必要があるかどうか</returns>
        /// <param name="param">パラメータ</param>
        public override bool Judge(Param param)
        {
            var context = param.Context;
            var player = context.OperationPlayer;
            return (context.FieldEvent[(int)player] == FieldEvent.MarkErasing || context.FieldEvent[(int)player] == FieldEvent.NextPreparation);
        }
    }
}
