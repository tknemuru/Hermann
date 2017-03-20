using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// 移動可能なスライムの集合体に関する情報と機能を提供します。
    /// </summary>
    public static class MovableSlimeUnit
    {
        /// <summary>
        /// 集合体のインデックス
        /// </summary>
        public enum Index
        {
            /// <summary>
            /// 1つめ
            /// </summary>
            First,

            /// <summary>
            /// 2つめ
            /// </summary>
            Second,
        }

        /// <summary>
        /// 集合体の形
        /// </summary>
        public enum Form
        {
            /// <summary>
            /// 横向き
            /// </summary>
            Horizontal,

            /// <summary>
            /// 縦向き
            /// </summary>
            Vertical,
        }

        /// <summary>
        /// 集合体の要素数
        /// </summary>
        public static readonly int Length = Enum.GetValues(typeof(Index)).Length;

        /// <summary>
        /// 集合体の形を取得します。
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Form GetForm(MovableSlime[] unit)
        {
            var first = unit[(int)Index.First];
            var second = unit[(int)Index.Second];

            if (Math.Abs(first.Position - second.Position) == 1)
            {
                return Form.Horizontal;
            }
            else
            {
                return Form.Vertical;
            }
        }
    }
}
