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
}
