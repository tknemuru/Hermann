using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Models
{
    /// <summary>
    /// NEXTスライム
    /// </summary>
    public static class NextSlime
    {
        /// <summary>
        /// NEXTスライムの数
        /// </summary>
        public const int Length = 2;

        /// <summary>
        /// NEXTスライムのインデックス
        /// </summary>
        public enum Index
        {
            /// <summary>
            /// 1つめ
            /// </summary>
            First = 0,

            /// <summary>
            /// 2つめ
            /// </summary>
            Second,
        }

        /// 両NEXTスライムに対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEach(Action<NextSlime.Index> action)
        {
            for (var unit = NextSlime.Index.First; (int)unit < NextSlime.Length; unit++)
            {
                action(unit);
            }
        }
    }
}
