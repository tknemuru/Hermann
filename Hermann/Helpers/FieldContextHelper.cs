using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <param name="player">対象プレイヤ</param>
        /// <param name="index">フィールド単位のインデックス</param>
        /// <param name="position">フィールド単位内の場所</param>
        /// <returns>スライムが存在しているかどうか</returns>
        public static bool ExistsSlime(FieldContext context, Player.Index player, int index, int position)
        {
            return context.SlimeFields[(int)player].Any(f => (f.Value[index] & (1u << position)) > 0u);
        }

        /// <summary>
        /// 指定した場所に対象色のスライムが存在しているかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">対象プレイヤ</param>
        /// <param name="index">フィールド単位のインデックス</param>
        /// <param name="position">フィールド単位内の場所</param>
        /// <param name="slime">対象スライム</param>
        /// <returns>スライムが存在しているかどうか</returns>
        public static bool ExistsSlime(FieldContext context, Player.Index player, int index, int position, Slime slime)
        {
            return ((context.SlimeFields[(int)player][slime][index] & (1u << position)) > 0u);
        }

        /// <summary>
        /// 移動可能なスライムを初期位置に配置します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="slimes">セットするスライム</param>
        /// <param name="movables">セット対象の移動可能スライム</param>
        public static void SetMovableSlimeInitialPosition(FieldContext context, Player.Index player, Slime[] slimes, MovableSlime[] movables)
        {
            Debug.Assert(slimes.Length == MovableSlime.Length, string.Format("スライムの数が不正です。数：{0}", slimes.Length));
            Debug.Assert(movables.Length == MovableSlime.Length, string.Format("移動可能なスライムの数が不正です。数：{0}", movables.Length));

            MovableSlime.ForEach((unitIndex) =>
            {
                movables[(int)unitIndex].Slime = slimes[(int)unitIndex];
                movables[(int)unitIndex].Index = FieldContextConfig.HiddenUnitIndex;
                movables[(int)unitIndex].Position = FieldContextConfig.MovableSlimeInitialShift + ((int)unitIndex * FieldContextConfig.OneLineBitCount);
                context.MovableSlimes[(int)player][(int)unitIndex] = movables[(int)unitIndex];

                // フィールドにも反映させる
                context.SlimeFields[(int)player][movables[(int)unitIndex].Slime][movables[(int)unitIndex].Index] |= 1u << movables[(int)unitIndex].Position;
            });
        }

        /// <summary>
        /// 移動可能なスライムを初期位置に配置します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="slimes">セットするスライム</param>
        public static void SetMovableSlimeInitialPosition(FieldContext context, Player.Index player, Slime[] slimes)
        {
            var movables = new MovableSlime[MovableSlime.Length];
            MovableSlime.ForEach((i) => movables[(int)i] = new MovableSlime());
            SetMovableSlimeInitialPosition(context, player, slimes, movables);
        }
    }
}
