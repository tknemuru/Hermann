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
        public static bool ExistsSlime(FieldContext context, int player, int index, int position)
        {
            return context.SlimeFields.Value[player].Any(f => (f.Value[index] & (1u << position)) > 0u);
        }

        /// <summary>
        /// 両プレイヤのフィールド情報に対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ActionForEachPlayer(FieldContext context, Action<FieldContext, int> action)
        {
            for (var player = Player.First; player < Player.Length; player++)
            {
                action(context, player);
            }
        }

        /// <summary>
        /// 移動可能なスライムに対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public static void ActionForEachMovableSlimes(FieldContext context, Action<FieldContext, int, MovableSlime.UnitIndex> action)
        {
            for (var player = Player.First; player < Player.Length; player++)
            {
                for (var unitIndex = MovableSlime.UnitIndex.First; (int)unitIndex < MovableSlime.Length; unitIndex++)
                {
                    action(context, player, unitIndex);
                }
            }
        }

        /// <summary>
        /// スライムごとの配置状態に対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ActionForEachSlimeFields(FieldContext context, Action<uint, int, Slime, int> action)
        {
            for (var player = Player.First; player < Player.Length; player++)
            {
                foreach (var fields in context.SlimeFields.Value[player])
                {
                    for (var i = 0; i < fields.Value.Length; i++)
                    {
                        action(fields.Value[i], player, fields.Key, i);
                    }
                }
            }
        }
    }
}
