using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Ai.Models;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Helpers
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
            /// 評価点
            /// </summary>
            Value = 1,
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
                        inputs.Add(status.Skip((int)StateIndex.LogWriteTarget + 1).Select(s => double.Parse(s)).ToArray());
                        inputCount++;
                        break;
                    case LogWriteTarget.WinResult:
                        var winPlayer = (Player.Index)int.Parse(status[(int)ResultIndex.Value]);
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
        /// ログの変換を行います。
        /// </summary>
        /// <param name="logs">ログ</param>
        /// <returns>変換されたログデータ</returns>
        public static Learning2DData ConvertLogToLearning2DData(IEnumerable<string> logs)
        {
            var players = new List<int>();
            var inputs = new List<double[]>();
            var outputs = new List<double>();
            var tempInputs = new List<double[]>();
            var saveCount = 0;
            var trashCount = 0;

            foreach (var log in logs)
            {
                var status = log.Split(',');
                var target = (LogWriteTarget)int.Parse(status[(int)StateIndex.LogWriteTarget]);
                switch (target)
                {
                    case LogWriteTarget.State:
                        tempInputs.Add(status.Skip((int)StateIndex.LogWriteTarget + 1).Select(s => double.Parse(s)).ToArray());
                        break;
                    case LogWriteTarget.WinResult:
                        var result = double.Parse(status[(int)ResultIndex.Value]);
                        if (requiredSave(result))
                        {
                            saveCount++;
                            inputs = inputs.Concat(tempInputs).ToList();
                            // 入力データ数分割り増し
                            outputs = outputs.Concat(Enumerable.Range(0, tempInputs.Count()).Select(i => result)).ToList();
                        } else
                        {
                            trashCount++;
                        }
                        Console.WriteLine($"saveCount:{saveCount}");
                        Console.WriteLine($"trashCount:{trashCount}");
                        tempInputs = new List<double[]>();
                        break;
                    default:
                        throw new ArgumentException("書き込み対象が不正です");
                }
            }

            Debug.Assert(inputs.Count() == outputs.Count(), "入力と出力の数が一致しません");

            return new Learning2DData()
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

        /// <summary>
        /// 記録対象かどうかを判定します。
        /// </summary>
        /// <param name="result">結果値</param>
        /// <returns>記録対象かどうか</returns>
        private static bool requiredSave(double result)
        {
            return true;
        }
    }
}
