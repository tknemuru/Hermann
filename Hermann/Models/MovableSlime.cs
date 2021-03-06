﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Models
{
    /// <summary>
    /// 移動可能なスライム
    /// </summary>
    public sealed class MovableSlime
    {
        /// <summary>
        /// 集合体のインデックス
        /// </summary>
        public enum UnitIndex
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
        public enum UnitForm
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
        /// スライム
        /// </summary>
        public Slime Slime { get; set; }

        /// <summary>
        /// 何番目のフィールドに属しているか
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// フィールド内のポジション（シフト量）
        /// </summary>
        public int Position { get; set; }      

        /// <summary>
        /// 集合体の要素数
        /// </summary>
        public static readonly int Length = Enum.GetValues(typeof(UnitIndex)).Length;

        /// <summary>
        /// 集合体の形を取得します。
        /// </summary>
        /// <param name="unit">集合体</param>
        /// <returns>集合体の形</returns>
        public static UnitForm GetUnitForm(MovableSlime[] unit)
        {
            var first = unit[(int)UnitIndex.First];
            var second = unit[(int)UnitIndex.Second];

            if (Math.Abs(first.Position - second.Position) == 1)
            {
                return UnitForm.Horizontal;
            }
            else
            {
                return UnitForm.Vertical;
            }
        }

        /// <summary>
        /// 移動可能なスライムに対して戻り値を持たないメソッドを繰り返し実行します。
        /// </summary>
        /// <param name="action">戻り値を持たないメソッド</param>
        public static void ForEach(Action<MovableSlime.UnitIndex> action)
        {
            for (var unitIndex = MovableSlime.UnitIndex.First; (int)unitIndex < MovableSlime.Length; unitIndex++)
            {
                action(unitIndex);
            }
        }

        /// <summary>
        /// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var movable = (MovableSlime)obj;
            var equals = new List<bool>();
            equals.Add(movable.Slime == this.Slime);
            equals.Add(movable.Index == this.Index);
            equals.Add(movable.Position == this.Position);
            return equals.All(e => e);
        }

        /// <summary>
        /// 既定のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在のオブジェクトのハッシュ コード。</returns>
        public override int GetHashCode()
        {
            return this.Slime.GetHashCode() ^ this.Index ^ (int)this.Position;
        }
    }
}
