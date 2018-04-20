using Hermann.Contexts;
using Hermann.Helper;
using Hermann.Helpers;
using Hermann.Learning.Analyzers;
using Hermann.Learning.Di;
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
        /// 削除できる可能性のあるスライムの分析機能
        /// </summary>
        private static readonly ErasedPotentialSlimeAnalyzer ErasedPotentialSlimeAnalyzer =
            LearningClientDiProvider.GetContainer().GetInstance<ErasedPotentialSlimeAnalyzer>();

        /// <summary>
        /// 高低差分析機能
        /// </summary>
        private static readonly DifferenceHeightAnalyzer DifferenceHeightAnalyzer =
            LearningClientDiProvider.GetContainer().GetInstance<DifferenceHeightAnalyzer>();

        /// <summary>
        /// 危険なインデックス（左から三番目の上四つ）
        /// </summary>
        private static readonly int[] DangerIndexes = new int[] { 5, 13, 21, 29 };

        /// <summary>
        /// 危険なユニット（上から二つ）
        /// </summary>
        private static readonly int[] DangerUnits = new int[] { 1, 2 };

        /// <summary>
        /// 上部ユニット
        /// </summary>
        private static readonly int[] UpperUnits = new int[] { 1 };

        /// <summary>
        /// 上部インデックス
        /// </summary>
        private static readonly int[] UpperIndexes = Enumerable.Range(0, FieldContextConfig.FieldUnitBitCount).Select(i => i).ToArray();

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
        /// 状態を学習用配列に変換します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        /// <returns>フィールドの状態を表した学習用uint配列</returns>
        public static List<int> ConvertContextToArray(FieldContext context)
        {
            var list = new List<int>();
            var param = new ErasedPotentialSlimeAnalyzer.Param();
            param.ErasedSlimes = context.UsingSlimes;

            Player.ForEach(player =>
            {
                var erasedPotentialCount = 0;
                var slimeCount = 0;
                foreach (var slime in context.UsingSlimes)
                {
                    param.TargetSlime = slime;
                    // 他の色を消すと消える個数
                    erasedPotentialCount += ErasedPotentialSlimeAnalyzer.Analyze(context, player, param);

                    // フィールドのスライム数
                    slimeCount += SlimeCountHelper.GetSlimeCount(context, player, slime);
                }
                list.Add(erasedPotentialCount);
                list.Add(slimeCount);

                // フィールドのおじゃまスライム数
                var obstructionCount = SlimeCountHelper.GetSlimeCount(context, player, Slime.Obstruction);
                list.Add(obstructionCount);

                // 予告おじゃまスライム数
                var noticeObstruction = ObstructionSlimeHelper.ObstructionsToCount(context.ObstructionSlimes[(int)player]);
                list.Add(noticeObstruction);

                // 高低差
                //var hDiff = DifferenceHeightAnalyzer.Analyze(context, player);
                //list.Add(hDiff);

                // 上部スライム数
                var upperCount = 0;
                foreach (var u in UpperUnits)
                {
                    foreach (var i in UpperIndexes)
                    {
                        upperCount += FieldContextHelper.ExistsSlime(context, player, FieldContextConfig.MaxHiddenUnitIndex + u, i) ? 1 : 0;
                    }
                }
                list.Add(upperCount);

                // 左から3番目のスライム数
                var dangerCount = 0;
                foreach(var u in DangerUnits)
                {
                    foreach (var i in DangerIndexes)
                    {
                        dangerCount += FieldContextHelper.ExistsSlime(context, player, FieldContextConfig.MaxHiddenUnitIndex + u, i) ? 1 : 0;
                    }
                }
                list.Add(dangerCount);
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
            //return (Math.Abs(result) > 2.0d);
            return true;
        }
    }
}
