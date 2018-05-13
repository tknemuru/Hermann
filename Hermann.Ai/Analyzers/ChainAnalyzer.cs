using System;
using System.Collections.Generic;
using System.Diagnostics;
using Hermann.Ai.Di;
using Hermann.Ai.Helpers;
using Hermann.Ai.Updaters;
using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Di;
using Hermann.Models;

namespace Hermann.Ai.Analyzers
{
    /// <summary>
    /// 起こりうる最大連鎖回数の分析機能を提供します。
    /// </summary>
    public class ChainAnalyzer : IPlayerFieldAnalyzable<int>
    {
        /// <summary>
        /// 起こりうる最大連鎖回数を分析します
        /// </summary>
        /// <returns>起こりうる最大連鎖回数</returns>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public int Analyze(FieldContext context, Player.Index player)
        {
            int maxChain = 0;

            // 移動可能パターンを取得
            var movePatterns = CompleteReadingHelper.GetAllMovePatterns();

            // 分析対象の移動可能スライムを生成
            var usingSlimes = context.UsingSlimes;
            var slimePatterns = new List<Slime[]>();
            slimePatterns.Add(new[] { usingSlimes[0], usingSlimes[1] });
            slimePatterns.Add(new[] { usingSlimes[2], usingSlimes[3] });

            // 自動移動機能を生成
            var updater = DiProvider.GetContainer().GetInstance<AutoMoveAndDropUpdater>();
            updater.Inject(context.UsingSlimes);

            // 各移動可能スライムの全パターンを試していく
            foreach(var slimePattern in slimePatterns)
            {
                var _context = context.DeepCopy();
                // 移動可能スライムを書き換える
                for (var movable = 0; movable < MovableSlime.Length; movable++)
                {
                    var org = _context.MovableSlimes[player.ToInt()][(int)movable];
                    var valid = (org.Index == FieldContextConfig.MaxHiddenUnitIndex &&
                                org.Position == FieldContextConfig.MovableSlimeInitialShiftAfterDroped + (FieldContextConfig.OneLineBitCount * (int)movable));
                    if (!valid)
                    {
                        return 0;
                    }

                    // 元の移動可能スライムを消す
                    _context.SlimeFields[player.ToInt()][org.Slime][org.Index] &= ~(1u << org.Position);

                    // 色を書き換える
                    org.Slime = slimePattern[(int)movable];
                }

                // 自動移動を実行
                foreach(var movePattern in movePatterns)
                {
                    var _movedContext = _context.DeepCopy();
                    var param = new AutoMoveAndDropUpdater.Param()
                    {
                        Pattern = movePattern,
                    };

                    updater.Update(_movedContext, player, param);
                    maxChain = Math.Max(maxChain, param.Chain);
                }
            }

            // 試行内の最大連鎖数を返却
            return maxChain;
        }
    }
}
