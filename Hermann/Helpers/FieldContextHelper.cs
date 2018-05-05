using Hermann.Models;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        /// 指定した場所に移動可能スライムが存在しているかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">対象プレイヤ</param>
        /// <param name="index">フィールド単位のインデックス</param>
        /// <param name="position">フィールド単位内の場所</param>
        /// <returns>移動スライムが存在しているかどうか</returns>
        public static bool ExistsMovableSlime(FieldContext context, Player.Index player, int index, int position)
        {
            var existsMovable = false;
            MovableSlime.ForEach(movable =>
            {
                if (context.MovableSlimes[(int)player][(int)movable].Index == index && context.MovableSlimes[(int)player][(int)movable].Position == position)
                {
                    existsMovable = true;
                }
            });
            return existsMovable;
        }

        /// <summary>
        /// 指定した場所のスライムを取得します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="unit">フィールドユニット</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <returns></returns>
        public static Slime GetSlime(FieldContext context, Player.Index player, int unit, int index)
        {
            return ExtensionSlime.Slimes.Select(s => s).Where(s => ExistsSlime(context, player, unit, index, s)).FirstOrDefault();
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

        /// <summary>
        /// フィールドユニットとユニット内のインデックスから指定した位置の行数を取得します。
        /// </summary>
        /// <param name="unit">フィールドユニット</param>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="containsHiddenLine">非表示行を含んだ行数を取得するかどうか</param>
        /// <returns>指定した位置の行数</returns>
        public static int GetLineIndex(int unit, int index, bool containsHiddenLine = false)
        {
            var line = (unit * FieldContextConfig.FieldUnitLineCount) + (index / FieldContextConfig.OneLineBitCount);

            if (!containsHiddenLine)
            {
                line -= (FieldContextConfig.MaxHiddenUnitIndex + 1) * FieldContextConfig.FieldUnitLineCount;
            }

            return line;
        }

        /// <summary>
        /// ユニット内のインデックスから指定した位置の列数を取得します。
        /// </summary>
        /// <param name="index">ユニット内のインデックス</param>
        /// <param name="containsHiddenColumn">非表示列を含んだ列数を取得するかどうか</param>
        /// <returns>指定した位置の列数</returns>
        public static int GetColumnIndex(int index, bool containsHiddenColumn = false)
        {
            var column = index % FieldContextConfig.OneLineBitCount;

            if (!containsHiddenColumn)
            {
                column -= (FieldContextConfig.OneLineBitCount - FieldContextConfig.VerticalLineLength);
            }

            return column;
        }

        /// <summary>
        /// 勝利プレイヤを取得します。
        /// </summary>
        /// <returns>勝利プレイヤ</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public static Player.Index? GetWinPlayer(FieldContext lastContext, FieldContext context)
        {
            if (context.FieldEvent[context.OperationPlayer.ToInt()] != FieldEvent.End)
            {
                return null;   
            }
            var firstWin = lastContext.WinCount[Player.Index.First.ToInt()] != context.WinCount[Player.Index.First.ToInt()];
            return firstWin ? Player.Index.First : Player.Index.Second;
        }

        /// <summary>
        /// スライムフィールドのマージを実施します。
        /// </summary>
        /// <returns>マージされたスライムフィールド</returns>
        /// <param name="x">マージ対象のスライムフィールド</param>
        /// <param name="y">マージ対象のスライムフィールド</param>
        public static uint[] MergeSlimeFields(uint[] x, uint[] y)
        {
            Debug.Assert(x.Length == y.Length, "要素数が一致しません。");

            var m = new uint[x.Length];
            for (var i = 0; i < x.Length; i++)
            {
                m[i] = x[i] | y[i];
            }

            return m;
        }

        /// <summary>
        /// 2進数文字列のリストをユニットに変換します。
        /// </summary>
        /// <returns>ユニット</returns>
        public static uint ConvertDigitStrsToUnit(IEnumerable<string> digitStrs)
        {
            var sb = new StringBuilder();
            foreach (var s in digitStrs.Reverse().ToArray())
            {
                sb.Append(s);
            }
            return Convert.ToUInt32(sb.ToString(), 2);
        }
    }
}
