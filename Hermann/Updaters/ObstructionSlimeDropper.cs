using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// おじゃまスライムの落下機能を提供します。
    /// </summary>
    public abstract class ObstructionSlimeDropper : IPlayerFieldUpdatable
    {
        /// <summary>
        /// 落下列の初期値
        /// </summary>
        protected const int DefaultLastRank = -1;

        /// <summary>
        /// 一列に落とすおじゃまスライムの最大量
        /// </summary>
        private const int MaxRankObstructionSlimeCount = 5;

        /// <summary>
        /// 一度に落とすおじゃまスライムの最大量
        /// </summary>
        private const int MaxDropCount = MaxRankObstructionSlimeCount * FieldContextConfig.VerticalLineLength;

        /// <summary>
        /// おじゃまスライムをフィールド上に配置します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            var startMaxCount = ObstructionSlimeHelper.ObstructionsToCount(context.ObstructionSlimes[(int)player]);
            var maxCount = Math.Min(startMaxCount, MaxDropCount);
            var dropCount = 0;
            var lastRank = DefaultLastRank;
            var rank = DefaultLastRank;

            var targetAllRanks = Enumerable.Range(FieldContextConfig.OneLineBitCount - FieldContextConfig.VerticalLineLength, FieldContextConfig.VerticalLineLength);
            var targetRanks = targetAllRanks;
            var isFraction = true;
            var fractionCount = 0;

            while (dropCount < maxCount)
            {
                if (isFraction && (maxCount - dropCount) % FieldContextConfig.VerticalLineLength == 0)
                {
                    // 端数処理は終了
                    isFraction = false;
                    fractionCount = dropCount;
                    targetRanks = targetAllRanks;
                    lastRank = DefaultLastRank;
                }

                if (isFraction)
                {
                    rank = this.GetNext(targetRanks, lastRank);
                    targetRanks = targetRanks.Select(r => r).Where(r => r != rank).ToArray();
                }
                else
                {
                    rank = this.GetSeaquentialNext(targetRanks, lastRank);
                }

                // 最小シフト量に加えて配置した行分シフトする
                var shift = FieldContextConfig.MinObstructionSlimeSetShift + (FieldContextConfig.OneLineBitCount * ((dropCount - fractionCount) / FieldContextConfig.VerticalLineLength));
                // 端数の配置が終わったら1行分シフトする
                shift = isFraction ? shift : shift + FieldContextConfig.OneLineBitCount;
                // シフト量によって対象ユニットが変わる
                var unitIndex = FieldContextConfig.MinHiddenUnitIndex + (shift / FieldContextConfig.FieldUnitBitCount);
                // ユニットが変わったら配置済ユニット分を差し引く
                shift = shift % FieldContextConfig.FieldUnitBitCount;
                Debug.Assert(unitIndex <= FieldContextConfig.MaxHiddenUnitIndex, "ユニットインデックスが隠し領域を超えています。");

                shift += rank;
                lastRank = rank;

                Debug.Assert(!FieldContextHelper.ExistsSlime(context, player, unitIndex, shift), "既にスライムが存在しています。");
                context.SlimeFields[(int)player][Slime.Obstruction][unitIndex] |= 1u << shift;

                dropCount++;
                Debug.Assert(maxCount >= dropCount, "最大落下量を超えています。");
            }
            Debug.Assert(maxCount == dropCount, string.Format("最大落下量と実際に落下した量が一致しません。{0}:{1}", maxCount, dropCount));

            context.ObstructionSlimes[(int)player] = ObstructionSlimeHelper.CountToObstructions(startMaxCount - dropCount);
        }

        /// <summary>
        /// 次に落下させる列を順番に取得します。
        /// </summary>
        /// <param name="ranks">列の候補</param>
        /// <param name="lastRank">前回落下させた列</param>
        /// <returns>次に落下させる列</returns>
        protected int GetSeaquentialNext(IEnumerable<int> ranks, int lastRank)
        {
            if (lastRank == DefaultLastRank)
            {
                return ranks.Min();
            }

            var rank = lastRank + 1;
            if (ranks.Max() < rank)
            {
                rank = ranks.Min();
            }

            return rank;
        }

        /// <summary>
        /// 次に落下させる列を取得します。
        /// </summary>
        /// <param name="ranks">列の候補</param>
        /// <param name="lastRank">前回落下させた列</param>
        /// <returns>次に落下させる列</returns>
        protected abstract int GetNext(IEnumerable<int> ranks, int lastRank);
    }
}
