using Hermann.Contexts;
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
    /// 得点計算機能を提供します。
    /// </summary>
    public sealed class ScoreCalculator : IPlayerFieldParameterizedUpdatable<ScoreCalculator.Param>
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 最大連結数
            /// </summary>
            public int MaxLinkedCount { get; set; }

            /// <summary>
            /// 色数
            /// </summary>
            public int ColorCount { get; set; }

            /// <summary>
            /// 全消
            /// </summary>
            public bool AllErased { get; set; }

            /// <summary>
            /// 下方向の移動距離
            /// </summary>
            public int DownDistance { get; set; }

            /// <summary>
            /// 加算得点
            /// </summary>
            public long ResultAddScore { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="downDistance">下方向移動距離</param>
            public Param(int downDistance)
            {
                this.MaxLinkedCount = 0;
                this.ColorCount = 0;
                this.AllErased = false;
                this.DownDistance = downDistance;
                this.ResultAddScore = 0;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="maxLinkedCount">最大連結数</param>
            /// <param name="colorCount">色数</param>
            /// <param name="allErased">全消</param>
            public Param(int maxLinkedCount, int colorCount, bool allErased)
            {
                this.MaxLinkedCount = maxLinkedCount;
                this.ColorCount = colorCount;
                this.AllErased = allErased;
                this.DownDistance = 0;
                this.ResultAddScore = 0;
            }
        }

        /// <summary>
        /// 基本点のベース比率
        /// </summary>
        private const int BaseRate = 10;

        /// <summary>
        /// 落下ボーナス点
        /// </summary>
        private const int DownBonus = 1;

        /// <summary>
        /// 全消ボーナス店
        /// </summary>
        private const int AllErasedBonus = 2100;

        /// <summary>
        /// 倍率初期値
        /// </summary>
        private const int DefaultMagnification = 1;

        /// <summary>
        /// 連鎖に対する倍率テーブル
        /// </summary>
        private static readonly Dictionary<int, int> MagnificationTable = BuildMagnificationBonusTable();

        /// <summary>
        /// 連結に対する倍率テーブル
        /// </summary>
        private static readonly Dictionary<int, int> LinkedBonusTable = BuildLinkedBonusTable();

        /// <summary>
        /// 色数に対する倍率テーブル
        /// </summary>
        private static readonly Dictionary<int, int> ColorBonusTable = BuildColorBonusTable();

        /// <summary>
        /// 得点を計算します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        public void Update(FieldContext context, Player.Index player, Param param)
        {
            param.ResultAddScore = 0;

            // 落下ボーナス
            var downBonus = param.DownDistance * DownBonus;
            param.ResultAddScore += downBonus;
            context.Score[(int)player] += downBonus;

            // 全消しボーナス
            var allErasedBonus = param.AllErased ? AllErasedBonus : 0;
            param.ResultAddScore += allErasedBonus;
            context.Score[(int)player] += allErasedBonus;

            // 連鎖数
            var chain = context.Chain[(int)player];

            // 連鎖数0以下ならボーナスのみ加算して処理終了
            if (chain <= 0)
            {
                return;
            }

            // 消したスライム数を算出
            var erasedCount = 0;
            for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
            {
                for (var unitIndex = 0; unitIndex < FieldContextConfig.FieldUnitCount; unitIndex++)
                {
                    if ((context.SlimeFields[(int)player][Slime.Erased][unitIndex] & (1u << i)) > 0)
                    {
                        erasedCount++;
                    }
                }
            }

            // 基本点
            var baseScore = erasedCount * BaseRate;

            // 倍率
            Debug.Assert(chain > 0, "連鎖数が不正です");
            Debug.Assert(param.MaxLinkedCount >= 4, "連結数が不正です");
            Debug.Assert(param.ColorCount > 0, "色数が不正です");
            var chainMag = MagnificationTable.ContainsKey(chain) ? MagnificationTable[chain] : MagnificationTable.Last().Value;
            var linkedMag = LinkedBonusTable.ContainsKey(param.MaxLinkedCount) ? LinkedBonusTable[param.MaxLinkedCount] : LinkedBonusTable.Last().Value;
            var colorMag = ColorBonusTable.ContainsKey(param.ColorCount) ? ColorBonusTable[param.ColorCount] : ColorBonusTable.Last().Value;

            var magnification = Math.Max((chainMag + linkedMag + colorMag), DefaultMagnification);

            param.ResultAddScore += baseScore * magnification;
            context.Score[(int)player] += param.ResultAddScore;
        }

        /// <summary>
        /// 連鎖に対するボーナス倍率テーブルを作成します。
        /// </summary>
        /// <returns>連鎖に対するボーナス倍率テーブル</returns>
        private static Dictionary<int, int> BuildMagnificationBonusTable()
        {
            // 連鎖	1	2	3	4	5	6	7	8	9	10	11	12	13	14	15	16	17	18	19
            // 倍率	0	8	16	32	64	96	128	160	192	224	256	288	320	352	384	416	448	480	512
            var table = new Dictionary<int, int>();
            table.Add(1, 0);
            table.Add(2, 8);
            table.Add(3, 16);
            table.Add(4, 32);
            table.Add(5, 64);
            table.Add(6, 96);
            table.Add(7, 128);
            table.Add(8, 160);
            table.Add(9, 192);
            table.Add(10, 224);
            table.Add(11, 256);
            table.Add(12, 288);
            table.Add(13, 320);
            table.Add(14, 352);
            table.Add(15, 384);
            table.Add(16, 416);
            table.Add(17, 448);
            table.Add(18, 480);
            table.Add(19, 512);
            return table;
        }

        /// <summary>
        /// 連結に対するボーナス倍率テーブルを作成します。
        /// </summary>
        /// <returns>連結に対するボーナス倍率テーブル</returns>
        private static Dictionary<int, int> BuildLinkedBonusTable()
        {
            // 4個 0
            // 5個 2
            // 6個 3
            // 7個 4
            // 8個 5
            // 9個 6
            // 10個 7
            // 11個～ 10
            var table = new Dictionary<int, int>();
            table.Add(4, 0);
            table.Add(5, 2);
            table.Add(6, 3);
            table.Add(7, 4);
            table.Add(8, 5);
            table.Add(9, 6);
            table.Add(10, 7);
            table.Add(11, 10);
            return table;
        }

        /// <summary>
        /// 色数に対するボーナス倍率テーブルを作成します。
        /// </summary>
        /// <returns>色数に対するボーナス倍率テーブル</returns>
        private static Dictionary<int, int> BuildColorBonusTable()
        {
            // 1色 0
            // 2色 3
            // 3色 6
            // 4色 12
            // 5色 24
            var table = new Dictionary<int, int>();
            table.Add(1, 0);
            table.Add(2, 3);
            table.Add(3, 6);
            table.Add(4, 12);
            table.Add(5, 24);
            return table;
        }
    }
}