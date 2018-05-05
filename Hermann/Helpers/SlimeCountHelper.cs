using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helpers
{
    /// <summary>
    /// スライム数のカウントに関する補助機能を提供します。
    /// </summary>
    public static class SlimeCountHelper
    {
        /// <summary>
        /// uintの値とスライム数の対応辞書
        /// </summary>
        private static readonly Dictionary<uint, int> CountDic = new Dictionary<uint, int>();

        /// <summary>
        /// 指定したスライム数をカウントします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="slime">スライム</param>
        /// <returns>指定したスライム数</returns>
        public static int GetSlimeCount(FieldContext context, Player.Index player, Slime slime)
        {
            return Enumerable.Range(0, FieldContextConfig.FieldUnitCount).
                Select(i => GetSlimeCount(context, player, slime, i)).Sum();
        }

        /// <summary>
        /// 指定したスライム数をカウントします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="slime">スライム</param>
        /// <param name="index">ユニットインデックス</param>
        /// <returns>指定したスライム数</returns>
        public static int GetSlimeCount(FieldContext context, Player.Index player, Slime slime, int index)
        {
            return GetCount(context.SlimeFields[(int)player][slime][index]);
        }

        /// <summary>
        /// 対象の値のカウント数を取得します。
        /// </summary>
        /// <param name="i">値</param>
        /// <returns>対象の値のカウント数</returns>
        private static int GetCount(uint i)
        {
            if (!CountDic.ContainsKey(i))
            {
                Add(i);
            }
            return CountDic[i];
        }

        /// <summary>
        /// 辞書に対象の値のカウント情報を追加します。
        /// </summary>
        /// <param name="i">数値</param>
        private static void Add(uint i)
        {
            var count = 0;
            for (var shift = 0; shift < 32; shift++)
            {
                if ((i & (1u << shift)) > 0)
                {
                    count++;
                }
            }
            CountDic[i] = count;
        }
    }
}
