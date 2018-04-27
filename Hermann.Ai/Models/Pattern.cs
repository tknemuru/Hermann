using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Models
{
    /// <summary>
    /// パターン
    /// </summary>
    public enum Pattern
    {
        /// <summary>
        /// 階段-左上一段
        /// </summary>
        StairsOneLeft,

        /// <summary>
        /// 階段-右上一段
        /// </summary>
        StairsOneRight,

        /// <summary>
        /// 階段-左上二段
        /// </summary>
        StairsTwoLeft,

        /// <summary>
        /// 階段-右上二段
        /// </summary>
        StairsTwoRight,

        /// <summary>
        /// 浮き-右
        /// </summary>
        FloatRight,

        /// <summary>
        /// 浮き-遠い右
        /// </summary>
        FloatFarRight,

        /// <summary>
        /// 浮き-左
        /// </summary>
        FloatLeft,

        /// <summary>
        /// 浮き-遠い左
        /// </summary>
        FloatFarLeft,

        /// <summary>
        /// 挟み込み-左上
        /// </summary>
        InterposeUpperLeft,

        /// <summary>
        /// 挟み込み-右上
        /// </summary>
        InterposeUpperRight,

        /// <summary>
        /// 挟み込み-左下
        /// </summary>
        InterposeLowerLeft,

        /// <summary>
        /// 挟み込み-右下
        /// </summary>
        InterposeLowerRight,
    }

    /// <summary>
    /// 拡張パターン
    /// </summary>
    public static class ExtensionPattern
    {
        /// <summary>
        /// パターン名を取得します。
        /// </summary>
        /// <param name="pattern">パターン</param>
        /// <returns>パターン名</returns>
        public static string GetName(this Pattern pattern)
        {
            var name = string.Empty;
            switch (pattern)
            {
                case Pattern.FloatFarLeft:
                    name = "FloatFarLeft";
                    break;
                case Pattern.FloatFarRight:
                    name = "FloatFarRight";
                    break;
                case Pattern.FloatLeft:
                    name = "FloatLeft";
                    break;
                case Pattern.FloatRight:
                    name = "FloatRight";
                    break;
                case Pattern.InterposeLowerLeft:
                    name = "InterposeLowerLeft";
                    break;
                case Pattern.InterposeLowerRight:
                    name = "InterposeLowerRight";
                    break;
                case Pattern.InterposeUpperLeft:
                    name = "InterposeUpperLeft";
                    break;
                case Pattern.InterposeUpperRight:
                    name = "InterposeUpperRight";
                    break;
                case Pattern.StairsOneLeft:
                    name = "StairsOneLeft";
                    break;
                case Pattern.StairsOneRight:
                    name = "StairsOneRight";
                    break;
                case Pattern.StairsTwoLeft:
                    name = "StairsTwoLeft";
                    break;
                case Pattern.StairsTwoRight:
                    name = "StairsTwoRight";
                    break;
                default:
                    throw new ArgumentException("パターンが不正です");
            }

            return name;
        }
    }
}
