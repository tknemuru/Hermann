using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Helpers
{
    /// <summary>
    /// 完全読みに関する補助機能を提供します。
    /// </summary>
    public static class CompleteReadingHelper
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
        private static readonly IEnumerable<IEnumerable<Direction>> MovePatterns = BuildMovePatterns();

        /// <summary>
        /// 全ての移動パターンを取得します。
        /// </summary>
        /// <returns>全ての移動パターン</returns>
        public static IEnumerable<IEnumerable<Direction>> GetAllMovePatterns()
        {
            return MovePatterns;
        }

        /// <summary>
        /// 全移動パターンを組み立てます。
        /// </summary>
        /// <returns>全移動パターン</returns>
        private static List<List<Direction>> BuildMovePatterns()
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
                foreach (var m in movePattern)
                {
                    ret.Add(r.Concat(m).ToList());
                }
            }

            return ret;
        }
    }
}
