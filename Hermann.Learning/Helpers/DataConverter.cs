using Hermann.Contexts;
using Hermann.Learning.Models;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning.Helpers
{
    /// <summary>
    /// 学習に関連するデータの変換機能を提供します。
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// 状態ログ項目のインデックス
        /// </summary>
        private enum StateIndex
        {
            /// <summary>
            /// 書き込み対象
            /// </summary>
            LogWriteTarget = 0,

            /// <summary>
            /// 操作プレイヤ
            /// </summary>
            OperationPlayer = 1,
        }

        /// <summary>
        /// 結果ログ項目のインデックス
        /// </summary>
        private enum ResultIndex
        {
            /// <summary>
            /// 書き込み対象
            /// </summary>
            LogWriteTarget = 0,

            /// <summary>
            /// 勝ちプレイヤ
            /// </summary>
            WinPlayer = 1,
        }

        /// <summary>
        /// 状態を学習用uint配列に変換します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>フィールドの状態を表した学習用uint配列</returns>
        public static List<uint> ConvertContextToArray(FieldContext context)
        {
            var list = new List<uint>();
            list.Add((uint)context.OperationPlayer);
            list.Add((uint)(context.Time / 10000));
            foreach (var slime in context.UsingSlimes)
            {
                list.Add((uint)slime);
            }

            Player.ForEach(player =>
            {
                var iplayer = (int)player;
                list.Add((uint)context.RotationDirection[iplayer]);
                list.Add((uint)context.Chain[iplayer]);
                list.Add(ToUint(context.Ground[iplayer]));
                foreach (var movable in context.MovableSlimes[iplayer])
                {
                    list.Add((uint)movable.Index);
                    list.Add((uint)movable.Position);
                    list.Add((uint)movable.Slime);
                }
                foreach (var next in context.NextSlimes[iplayer])
                {
                    foreach (var slime in next)
                    {
                        list.Add((uint)slime);
                    }
                }
                foreach (var obs in context.ObstructionSlimes[iplayer])
                {
                    list.Add((uint)obs.Value);
                }
                list.Add((uint)context.Score[iplayer]);
                foreach (var slimeFields in context.SlimeFields[iplayer])
                {
                    foreach (var field in slimeFields.Value)
                    {
                        list.Add(field);
                    }
                }
                list.Add((uint)context.UsedScore[iplayer]);
            });

            return list;
        }

        /// <summary>
        /// ログの変換を行います。
        /// </summary>
        /// <param name="logs">ログ</param>
        /// <returns>変換されたログデータ</returns>
        public static LearningData ConvertLogToLearningData(IEnumerable<string> logs)
        {
            var inputCount = 0;
            var players = new List<int>();
            var inputs = new List<double[]>();
            var outputs = new List<double[]>();

            foreach (var log in logs)
            {
                var status = log.Split(',');
                var target = (LogWriteTarget)int.Parse(status[(int)StateIndex.LogWriteTarget]);
                switch (target)
                {
                    case LogWriteTarget.State:
                        inputs.Add(status.Skip((int)StateIndex.OperationPlayer + 1).Select(s => double.Parse(s)).ToArray());
                        inputCount++;
                        break;
                    case LogWriteTarget.WinResult:
                        var winPlayer = (Player.Index)int.Parse(status[(int)ResultIndex.WinPlayer]);
                        var result = (winPlayer == Player.Index.First) ? new double[] { 1.0, 0.0 } : new double[] { 0.0, 1.0 };

                        // 入力データ数分割り増し
                        outputs = outputs.Concat(Enumerable.Range(0, inputCount).Select(i => result)).ToList();
                        inputCount = 0;
                        break;
                    default:
                        throw new ArgumentException("書き込み対象が不正です");
                }
            }

            Debug.Assert(inputs.Count() == outputs.Count(), "入力と出力の数が一致しません");

            return new LearningData()
            {
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray(),
            };
        }

        /// <summary>
        /// uintに変換します。
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>uintに変換した値</returns>
        private static uint ToUint(bool value)
        {
            return value ? 1u : 0u;
        }

    }
}
