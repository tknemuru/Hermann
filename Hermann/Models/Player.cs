using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hermann.Models
{
    /// <summary>
    /// プレイヤ
    /// </summary>
    public sealed class Player
    {
        /// <summary>
        /// プレイヤ数
        /// </summary>
        public const int Length = 2;

        /// <summary>
        /// インデックス
        /// </summary>
        public enum Index
        {
            /// <summary>
            /// 1P
            /// </summary>
            First = 0,

            /// <summary>
            /// 2P
            /// </summary>
            Second,
        }

        /// <summary>
        /// 両プレイヤのフィールド情報に対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEach(Action<Index> action)
        {
            for (var player = Index.First; (int)player < Player.Length; player++)
            {
                action(player);
            }
        }

        /// <summary>
        /// 反対のインデックスを取得します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>反対のインデックス</returns>
        public static Index GetOppositeIndex(Index index)
        {
            return index == Index.First ? Index.Second : Index.First;
        }
    }

    public static class ExtensionPlayer
    {
        /// <summary>
        /// プレイヤ名を取得します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <returns>プレイヤ名</returns>
        public static string GetName(this Player.Index player)
        {
            var name = string.Empty;

            switch (player)
            {
                case Player.Index.First:
                    name = "1P";
                    break;
                case Player.Index.Second:
                    name = "2P";
                    break;
                default:
                    throw new ArgumentException("プレイヤが不正です。");
            }

            return name;
        }

        /// <summary>
        /// 数値化したインデックスを取得します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>数値化したインデックス</returns>
        public static int ToInt(this Player.Index index)
        {
            return (int)index;
        }

        /// <summary>
        /// 反対のインデックスを取得します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>反対のインデックス</returns>
        public static Player.Index GetOppositeIndex(this Player.Index index)
        {
            return index == Player.Index.First ? Player.Index.Second : Player.Index.First;
        }
    }
}
