using Hermann.Models;
using Hermann.Updaters;
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
        /// 右の壁際かどうかを判定します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>右の壁際かどうか</returns>
        public static bool IsCloseToRightWall(int index)
        {
            return !(((1u << index) & 0xf8f8f8f8u) > 0);
        }

        /// <summary>
        /// 左の壁際かどうかを判定します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>左の壁際かどうか</returns>
        public static bool IsCloseToLeftWall(int index)
        {
            return !(((1u << index) & 0x7f7f7f7fu) > 0);
        }
    }
}
