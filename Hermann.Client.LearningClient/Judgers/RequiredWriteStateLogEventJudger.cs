using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// イベントから状態を書き込む必要があるかどうかを判定する機能を提供します。
    /// </summary>
    public class RequiredWriteStateLogEventJudger : RequiredWriteStateLogJudger, IInjectable<IEnumerable<FieldEvent>>
    {
        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// 書き込み対象のイベントリスト
        /// </summary>
        private IEnumerable<FieldEvent> TargetFieldEvents { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="targetFieldEvents">書き込み対象のイベントリスト</param>
        public void Inject(IEnumerable<FieldEvent> targetFieldEvents)
        {
            this.TargetFieldEvents = targetFieldEvents;
            this.HasInjected = true;
        }

        /// <summary>
        /// イベントから状態を書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns>状態を書き込む必要があるかどうか</returns>
        /// <param name="param">パラメータ</param>
        public override bool Judge(Param param)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            var context = param.Context;
            var player = context.OperationPlayer;
            return this.TargetFieldEvents.Contains(context.FieldEvent[(int)player]);
        }
    }
}
