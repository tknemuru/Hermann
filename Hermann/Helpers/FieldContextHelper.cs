using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helpers
{
    /// <summary>
    /// フィールドコンテキストに対する操作機能を提供します。
    /// </summary>
    public static class FieldContextHelper
    {
        /// <summary>
        /// 指定した場所にスライムが存在しているかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="index">フィールド単位のインデックス</param>
        /// <param name="position">フィールド単位内の場所</param>
        /// <returns></returns>
        public static bool ExistsSlime(FieldContext context, Player.Index player, int index, int position)
        {
            return context.SlimeFields.Value[(int)player].Any(f => (f.Value[index] & (1u << position)) > 0u);
        }

        /// <summary>
        /// 両プレイヤのフィールド情報に対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEachPlayer(Action<Player.Index> action)
        {
            for (var player = Player.Index.First; (int)player < Player.Length; player++)
            {
                action(player);
            }
        }

        /// <summary>
        /// 移動可能なスライムに対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEachMovableSlimes(Action<MovableSlime.UnitIndex> action)
        {
            for (var player = Player.Index.First; (int)player < Player.Length; player++)
            {
                for (var unitIndex = MovableSlime.UnitIndex.First; (int)unitIndex < MovableSlime.Length; unitIndex++)
                {
                    action(unitIndex);
                }
            }
        }

        /// <summary>
        /// 両NEXTスライムに対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEachNextSlime(Action<NextSlime.Index> action)
        {
            for (var unit = NextSlime.Index.First; (int)unit < NextSlime.Length; unit++)
            {
                action(unit);
            }
        }
    }
}
