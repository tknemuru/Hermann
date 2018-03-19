using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Models
{
    /// <summary>
    /// おじゃまスライム
    /// </summary>
    public enum ObstructionSlime
    {
        /// <summary>
        /// 小スライム
        /// </summary>
        Small = 1,

        /// <summary>
        /// 大スライム
        /// </summary>
        Big = 6,

        /// <summary>
        /// 岩スライム
        /// </summary>
        Rock = 30,

        /// <summary>
        /// 星スライム
        /// </summary>
        Star = 180,

        /// <summary>
        /// 月スライム
        /// </summary>
        Moon = 360,

        /// <summary>
        /// 王冠スライム
        /// </summary>
        Crown = 720,

        /// <summary>
        /// 彗星スライム
        /// </summary>
        Comet = 1440,
    }

    /// <summary>
    /// おじゃまスライム拡張
    /// </summary>
    public static class ExtensionObstructionSlime
    {
        /// <summary>
        /// おじゃまスライム名を取得します。
        /// </summary>
        /// <param name="obs">おじゃまスライム</param>
        /// <returns>おじゃまスライム名</returns>
        public static string GetName(this ObstructionSlime obs)
        {
            var name = string.Empty;
            switch (obs)
            {
                case ObstructionSlime.Big:
                    name = "Big";
                    break;
                case ObstructionSlime.Comet:
                    name = "Comet";
                    break;
                case ObstructionSlime.Crown:
                    name = "Crown";
                    break;
                case ObstructionSlime.Moon:
                    name = "Moon";
                    break;
                case ObstructionSlime.Rock:
                    name = "Rock";
                    break;
                case ObstructionSlime.Small:
                    name = "Small";
                    break;
                case ObstructionSlime.Star:
                    name = "Star";
                    break;
                default:
                    throw new ArgumentException("おじゃまスライムが不正です");
            }
            return name;
        }
    }
}
