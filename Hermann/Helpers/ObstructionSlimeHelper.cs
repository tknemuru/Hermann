using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helpers
{
    /// <summary>
    /// おじゃまスライムに関する補助機能を提供します。
    /// </summary>
    public static class ObstructionSlimeHelper
    {
        /// <summary>
        /// レート
        /// </summary>
        public const long Rate = 70;

        /// <summary>
        ///　得点をおじゃまスライムの集合に変換します。
        /// </summary>
        /// <param name="score">得点</param>
        /// <returns>おじゃまスライムの集合</returns>
        public static Dictionary<ObstructionSlime, int> ScoreToObstructions(long score)
        {
            return CountToObstructions(ScoreToCount(score));
        }

        /// <summary>
        /// おじゃまスライム（小）の数をおじゃまスライムの集合に変換します。
        /// </summary>
        /// <param name="count">おじゃまスライム（小）の数</param>
        /// <returns>おじゃまスライムの集合</returns>
        public static Dictionary<ObstructionSlime, int> CountToObstructions(int count)
        {
            var obs = ((IEnumerable<ObstructionSlime>)Enum.GetValues(typeof(ObstructionSlime))).
                OrderByDescending(o => o);
            var ret = new Dictionary<ObstructionSlime, int>();

            foreach (var o in obs)
            {
                var modCount = count / (int)o;
                if (ret.ContainsKey(o))
                {
                    ret[o] += (int)modCount;
                }
                else
                {
                    ret.Add(o, (int)modCount);
                }
                count -= modCount * (int)o;
            }

            return ret;
        }

        /// <summary>
        /// おじゃまスライムの集合をおじゃまスライム（小）の数に変換します。
        /// </summary>
        /// <param name="obstruciotnSlimes">おじゃまスライムの集合</param>
        /// <returns>おじゃまスライム（小）の数</returns>
        public static int ObstructionsToCount(Dictionary<ObstructionSlime, int> obstruciotnSlimes)
        {
            var count = 0;

            foreach (var ob in obstruciotnSlimes)
            {
                count += (int)ob.Key * ob.Value;
            }

            return count;
        }

        /// <summary>
        /// おじゃまスライムの集合を得点に変換します。
        /// </summary>
        /// <param name="obstruciotnSlimes">おじゃまスライムの集合</param>
        /// <returns>得点</returns>
        public static long ObstructionsToScore(Dictionary<ObstructionSlime, int> obstruciotnSlimes)
        {
            return ObstructionsToCount(obstruciotnSlimes) * Rate;
        }

        /// <summary>
        /// 得点をおじゃまスライム（小）の数に変換します。
        /// </summary>
        /// <param name="score">得点</param>
        /// <returns>おじゃまスライム（小）の数</returns>
        public static int ScoreToCount(long score)
        {
            return (int)(score / Rate);
        }

        /// <summary>
        /// 得点をおじゃまスライム（小）の数に換算した際の余りを取得します。
        /// </summary>
        /// <param name="score">得点</param>
        /// <returns>おじゃまスライム（小）の数に換算した際の余り</returns>
        public static long GetScoreRemainder(long score)
        {
            return score % Rate;
        }

        /// <summary>
        /// 落下させていないおじゃまスライムがまだ残っているかどうかを取得します。
        /// </summary>
        /// <param name="obstructionSlimes">おじゃまスライムの集合</param>
        /// <returns>落下させていないおじゃまスライムがまだ残っているかどうか</returns>
        public static bool ExistsObstructionSlime(Dictionary<ObstructionSlime, int> obstructionSlimes)
        {
            return obstructionSlimes.Any(o => o.Value > 0);
        }
    }
}
