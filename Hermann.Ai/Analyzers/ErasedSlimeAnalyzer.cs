using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Models;

namespace Hermann.Ai.Analyzers
{
    /// <summary>
    /// 削除されたスライムの分析機能を提供します。
    /// </summary>
    public class ErasedSlimeAnalyzer : IAnalyzable<ErasedSlimeAnalyzer.Param, FieldContext>
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 分析対象のフィールド状態
            /// </summary>
            public FieldContext TargetContext { get; set; }

            /// <summary>
            /// 削除スライム
            /// </summary>
            public uint[] ErasedSlimes { get; set; }
        }

        /// <summary>
        /// 分析を実行します。
        /// </summary>
        /// <returns>分析結果</returns>
        /// <param name="param">入力情報</param>
        public FieldContext Analyze(Param param)
        {
            var player = param.TargetContext.OperationPlayer;
            MovableSlime.ForEach(moveble =>
            {
                if (param.TargetContext.MovableSlimes[player.ToInt()][(int)moveble].Index > FieldContextConfig.MaxHiddenUnitIndex)
                {
                    throw new InvalidOperationException("移動可能スライムが隠し領域外に存在するときに呼び出されることは想定していません");
                }
            });
            var target = param.TargetContext.SlimeFields[param.TargetContext.OperationPlayer.ToInt()];
            foreach(var fields in target)
            {
                for (var i = 0; i < param.ErasedSlimes.Length; i++)
                {
                    // 隠し領域は対象外
                    if (i < FieldContextConfig.MinDisplayUnitIndex)
                    {
                        continue;
                    }
                    fields.Value[i] &= param.ErasedSlimes[i];
                }
            }

            return param.TargetContext;
        }
    }
}
