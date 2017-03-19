﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Collections;

namespace Hermann.Contexts
{
    /// <summary>
    /// 移動可能な色オブジェクトの配置情報
    /// </summary>
    public class MovableInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MovableInfo()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="slime">スライム</param>
        /// <param name="index">何番目のフィールドに属しているか</param>
        /// <param name="positon">フィールド内のポジション（シフト量）</param>
        public MovableInfo(Slime slime, int index, int positon)
        {
            this.Slime = slime;
            this.Index = index;
            this.Position = positon;
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

            var movable = (MovableInfo)obj;
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
