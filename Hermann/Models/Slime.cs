using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Models
{
    /// <summary>
    /// スライム
    /// </summary>
    public enum Slime
    {
        /// <summary>
        /// 無
        /// </summary>
        None = 0,

        /// <summary>
        /// 赤
        /// </summary>
        Red = 1,

        /// <summary>
        /// 青
        /// </summary>
        Blue = 2,

        /// <summary>
        /// 緑
        /// </summary>
        Green = 3,

        /// <summary>
        /// 黄
        /// </summary>
        Yellow = 4,

        /// <summary>
        /// 紫
        /// </summary>
        Purple = 5,

        /// <summary>
        /// おじゃま
        /// </summary>
        Obstruction = 15,

        /// <summary>
        /// 消済
        /// </summary>
        Erased = 16,
    }

    /// <summary>
    /// スライム拡張
    /// </summary>
    public static class ExtensionSlime
    {
        /// <summary>
        /// スライムリスト
        /// </summary>
        public static readonly IEnumerable<Slime> Slimes;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ExtensionSlime()
        {
            Slimes = ((IEnumerable<Slime>)Enum.GetValues(typeof(Slime))).Where(slime => slime != Slime.None);
        }

        /// <summary>
        /// スライム名を取得します。
        /// </summary>
        /// <param name="slime">スライム</param>
        /// <returns>スライム名</returns>
        public static string GetName(this Slime slime)
        {
            var name = string.Empty;
            switch (slime)
            {
                case Slime.Blue:
                    name = "Blue";
                    break;
                case Slime.Erased:
                    name = "Erased";
                    break;
                case Slime.Green:
                    name = "Green";
                    break;
                case Slime.None:
                    name = "None";
                    break;
                case Slime.Obstruction:
                    name = "Obstruction";
                    break;
                case Slime.Purple:
                    name = "Purple";
                    break;
                case Slime.Red:
                    name = "Red";
                    break;
                case Slime.Yellow:
                    name = "Yellow";
                    break;
                default:
                    throw new ArgumentException("スライムが不正です");
            }
            return name;
        }
    }
}
