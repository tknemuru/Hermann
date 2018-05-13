using Accord.Math;
using Hermann.Ai.Analyzers;
using Hermann.Ai.Models;
using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Helpers;
using Hermann.Ai.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Di;

namespace Hermann.Ai.Generators
{
    /// <summary>
    /// フィールド特徴の生成機能を提供します。
    /// </summary>
    public class FieldFeatureGenerator : IGeneratable<FieldContext, SparseVector<double>>
    {
        /// <summary>
        /// 特徴
        /// </summary>
        public enum Feature
        {
            /// <summary>
            /// 他の色を消すと消える個数
            /// </summary>
            ErasedPotentialCount,

            /// <summary>
            /// フィールドのスライム数
            /// </summary>
            SlimeCount,

            /// <summary>
            /// フィールドのおじゃまスライム数
            /// </summary>
            ObstructionCount,

            /// <summary>
            /// 予告おじゃまスライム数
            /// </summary>
            NoticeObstruction,

            /// <summary>
            /// 高低差
            /// </summary>
            HeightDiff,

            /// <summary>
            /// 上部スライム数
            /// </summary>
            UpperCount,

            /// <summary>
            /// 左から3番目のスライム数
            /// </summary>
            DangerCount,

            /// <summary>
            /// 起こりうる連鎖回数
            /// </summary>
            Chain,
        }

        /// <summary>
        /// 設定情報
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 特徴を生成対象とするかどうか
            /// </summary>
            public Dictionary<Feature, bool> TargetFeatue { get; set; }

            /// <summary>
            /// 両方のプレイヤを対象にするかどうか
            /// </summary>
            /// <value><c>true</c> if both player; otherwise, <c>false</c>.</value>
            public bool BothPlayer { get; set; } = true;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Config()
            {
                this.TargetFeatue = ((IEnumerable<Feature>)Enum.GetValues(typeof(Feature))).ToDictionary(f => f, f => false);
            }
        }

        /// <summary>
        /// 削除できる可能性のあるスライムの分析機能
        /// </summary>
        private ErasedPotentialSlimeAnalyzer ErasedPotentialSlimeAnalyzer =
            DiProvider.GetContainer().GetInstance<ErasedPotentialSlimeAnalyzer>();

        /// <summary>
        /// 高低差分析機能
        /// </summary>
        private DifferenceHeightAnalyzer DifferenceHeightAnalyzer =
            DiProvider.GetContainer().GetInstance<DifferenceHeightAnalyzer>();

        /// <summary>
        /// 連鎖回数分析機能
        /// </summary>
        private ChainAnalyzer ChainAnalyzer =
            DiProvider.GetContainer().GetInstance<ChainAnalyzer>();

        /// <summary>
        /// 危険なインデックス（左から三番目の上四つ）
        /// </summary>
        private int[] DangerIndexes = new int[] { 5, 13, 21, 29 };

        /// <summary>
        /// 危険なユニット（上から二つ）
        /// </summary>
        private int[] DangerUnits = new int[] { 1, 2 };

        /// <summary>
        /// 上部ユニット
        /// </summary>
        private int[] UpperUnits = new int[] { 1 };

        /// <summary>
        /// 上部インデックス
        /// </summary>
        private static readonly int[] UpperIndexes = Enumerable.Range(0, FieldContextConfig.FieldUnitBitCount).Select(i => i).ToArray();

        /// <summary>
        /// 設定情報
        /// </summary>
        private Config OwnConfig { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Injection(Config config)
        {
            this.OwnConfig = config;
        }

        /// <summary>
        /// フィールドの特徴を生成します。
        /// </summary>
        /// <param name="context"></param>
        /// <returns>フィールドの特徴</returns>
        public SparseVector<double> GetNext(FieldContext context)
        {
            var vector = new SparseVector<double>(0.0d);
            var param = new ErasedPotentialSlimeAnalyzer.Param();
            param.ErasedSlimes = context.UsingSlimes;

            var players = new[] { Player.Index.First, Player.Index.Second };
            foreach (var player in players)
            {
                if (!this.OwnConfig.BothPlayer && player != context.OperationPlayer)
                {
                    continue;
                }

                var erasedPotentialCount = 0;
                var slimeCount = 0;
                foreach (var slime in context.UsingSlimes)
                {
                    param.TargetSlime = slime;
                    if (this.OwnConfig.TargetFeatue[Feature.ErasedPotentialCount])
                    {
                        // 他の色を消すと消える個数
                        erasedPotentialCount += ErasedPotentialSlimeAnalyzer.Analyze(context, player, param);
                    }

                    if (this.OwnConfig.TargetFeatue[Feature.SlimeCount])
                    {
                        // フィールドのスライム数
                        slimeCount += SlimeCountHelper.GetSlimeCount(context, player, slime);
                    }
                }
                if (this.OwnConfig.TargetFeatue[Feature.ErasedPotentialCount])
                {
                    vector.Add(erasedPotentialCount);
                }

                if (this.OwnConfig.TargetFeatue[Feature.SlimeCount])
                {
                    vector.Add(slimeCount);
                }

                if (this.OwnConfig.TargetFeatue[Feature.ObstructionCount])
                {
                    // フィールドのおじゃまスライム数
                    var obstructionCount = SlimeCountHelper.GetSlimeCount(context, player, Slime.Obstruction);
                    vector.Add(obstructionCount);
                }

                if (this.OwnConfig.TargetFeatue[Feature.NoticeObstruction])
                {
                    // 予告おじゃまスライム数
                    var noticeObstruction = ObstructionSlimeHelper.ObstructionsToCount(context.ObstructionSlimes[(int)player]);
                    vector.Add(noticeObstruction);
                }

                if (this.OwnConfig.TargetFeatue[Feature.HeightDiff])
                {
                    // 高低差
                    var hDiff = DifferenceHeightAnalyzer.Analyze(context, player);
                    vector.Add(hDiff);
                }

                if (this.OwnConfig.TargetFeatue[Feature.UpperCount])
                {
                    // 上部スライム数
                    var upperCount = 0;
                    foreach (var u in UpperUnits)
                    {
                        foreach (var i in UpperIndexes)
                        {
                            upperCount += FieldContextHelper.ExistsSlime(context, player, FieldContextConfig.MaxHiddenUnitIndex + u, i) ? 1 : 0;
                        }
                    }
                    vector.Add(upperCount);
                }

                if (this.OwnConfig.TargetFeatue[Feature.DangerCount])
                {
                    // 左から3番目のスライム数
                    var dangerCount = 0;
                    foreach (var u in DangerUnits)
                    {
                        foreach (var i in DangerIndexes)
                        {
                            dangerCount += FieldContextHelper.ExistsSlime(context, player, FieldContextConfig.MaxHiddenUnitIndex + u, i) ? 1 : 0;
                        }
                    }
                    vector.Add(dangerCount);
                }

                if (this.OwnConfig.TargetFeatue[Feature.Chain])
                {
                    // 起こりうる最大連鎖数
                    var chain = 0;
                    if(player == context.OperationPlayer)
                    {
                        chain = this.ChainAnalyzer.Analyze(context, player);
                    }
                    vector.Add(chain);
                }
            };

            return vector;
        }
    }
}
