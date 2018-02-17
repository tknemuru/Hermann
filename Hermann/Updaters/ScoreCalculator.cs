using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 得点計算機能を提供します。
    /// </summary>
    public sealed class ScoreCalculator : IPlayerFieldUpdatable
    {
        /// <summary>
        /// 基本点のベース比率
        /// </summary>
        private const int BaseRate = 10;

        /// <summary>
        /// 連鎖数の補正割合
        /// </summary>
        private const int ChainModRate = 2;

        /// <summary>
        /// 連鎖に対する倍率テーブル
        /// </summary>
        private static readonly Dictionary<int, int> MagnificationTable = BuildMagnificationTable();

        /// <summary>
        /// 得点を計算します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        public void Update(FieldContext context, Player.Index player)
        {
            // 消したスライム数を算出
            var erasedCount = 0;
            for(var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
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

            // 連鎖数
            var chain = context.Chain[(int)player] / ChainModRate;

            // 倍率
            var magnification = MagnificationTable[chain];

            context.Score[(int)player] += baseScore * magnification;
        }

        /// <summary>
        /// 連鎖に対する倍率テーブルを作成します。
        /// </summary>
        /// <returns>連鎖に対する倍率テーブル</returns>
        private static Dictionary<int, int> BuildMagnificationTable()
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
    }
}
