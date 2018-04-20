using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Analyzers
{
    /// <summary>
    /// 完全読みでの移動可能な方向の分析機能を提供します。
    /// </summary>
    public class CompleteReadingMovableDirectionAnalyzer : IPlayerFieldAnalyzable<IEnumerable<IEnumerable<Direction>>>
    {
        /// <summary>
        /// 回転テンプレート
        /// </summary>
        private static readonly IEnumerable<Direction> RotatePatternTemplate = Enumerable.Range(0, 3).Select(i => Direction.Up);

        /// <summary>
        /// 左移動テンプレート
        /// </summary>
        private static readonly IEnumerable<Direction> RightPatternTemplate = Enumerable.Range(0, 2).Select(i => Direction.Left);

        /// <summary>
        /// 右移動テンプレート
        /// </summary>
        private static readonly IEnumerable<Direction> LeftPatternTemplate = Enumerable.Range(0, 3).Select(i => Direction.Right);

        /// <summary>
        /// 全移動パターン
        /// </summary>
        private static readonly IEnumerable<IEnumerable<Direction>> MovablePatterns = BuildMovablePatterns();

        /// <summary>
        /// 移動可能な方向を分析した結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>分析結果</returns>
        public IEnumerable<IEnumerable<Direction>> Analyze(FieldContext context, Player.Index player)
        {
            var ret = new List<List<Direction>>();
            ret.Add(new List<Direction>());

            // イベント中の場合は移動不可
            if (context.FieldEvent[(int)player] != FieldEvent.None)
            {
                return ret;
            }

            return MovablePatterns;
        }

        /// <summary>
        /// 全移動パターンを組み立てます。
        /// </summary>
        /// <returns>全移動パターン</returns>
        private static List<List<Direction>> BuildMovablePatterns()
        {
            // 回転パターンの生成
            var rotatePattern = new List<List<Direction>>();
            // 回転無し
            rotatePattern.Add(new List<Direction>());
            // 回転有り
            rotatePattern = rotatePattern.Concat(Enumerable.Range(1, 3).Select(i => RotatePatternTemplate.Take(i).ToList())).ToList();

            // 横への移動パターンの生成
            var movePattern = new List<List<Direction>>();
            // 横移動無し
            movePattern.Add(new List<Direction>());
            // 右移動有り
            movePattern = movePattern.Concat(Enumerable.Range(1, 2).Select(i => RightPatternTemplate.Take(i).ToList())).ToList();
            // 左移動有り
            movePattern = movePattern.Concat(Enumerable.Range(1, 3).Select(i => LeftPatternTemplate.Take(i).ToList())).ToList();

            // 回転と横移動のパターンを掛け合わせる
            var ret = new List<List<Direction>>();
            foreach (var r in rotatePattern)
            {
                foreach(var m in movePattern)
                {
                    ret.Add(r.Concat(m).ToList());
                }
            }

            return ret;
        }
    }
}
