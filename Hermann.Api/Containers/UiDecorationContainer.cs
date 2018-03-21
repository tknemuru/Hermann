using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Containers
{
    /// <summary>
    /// UIの飾りに必要な情報を格納します。
    /// </summary>
    public class UiDecorationContainer
    {
        /// <summary>
        /// フィールド状態
        /// </summary>
        public FieldContext FieldContext { get; set; }

        /// <summary>
        /// スライムの結合状態
        /// </summary>
        public SlimeJoinState[][] SlimeJoinStatus { get; set; }

        /// <summary>
        /// 削除時のアニメーション累積フレーム数
        /// </summary>
        public int[] EraseAnimationFrameElapsedCount { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UiDecorationContainer()
        {
            this.SlimeJoinStatus = new SlimeJoinState[Player.Length][];
            this.EraseAnimationFrameElapsedCount = new[] { 0, 0 };
        }
    }
}
