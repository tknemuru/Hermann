using System;
using System.Collections.Generic;
using System.Linq;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// 連鎖回数から状態を書き込む必要があるかどうかを判定する機能を提供します。
    /// </summary>
    public class RequiredWriteStateLogChainJudger : RequiredWriteStateLogJudger
    {
        /// <summary>
        /// 書き込み対象の最低連鎖回数
        /// </summary>
        private int MinChainCount { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="minChainCount">書き込み対象の最低連鎖回数</param>
        public void Injection(int minChainCount)
        {
            this.MinChainCount = minChainCount;
        }

        /// <summary>
        /// イベントから状態を書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns>状態を書き込む必要があるかどうか</returns>
        /// <param name="param">パラメータ</param>
        public override bool Judge(Param param)
        {
            var context = param.Context;
            var player = context.OperationPlayer;
            return context.Chain[player.ToInt()] >= this.MinChainCount;
        }
    }
}
