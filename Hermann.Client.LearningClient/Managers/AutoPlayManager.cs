using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hermann.Ai;
using Hermann.Ai.Analyzers;
using Hermann.Ai.Providers;
using Hermann.Client.LearningClient.Di;
using Hermann.Client.LearningClient.Evaluators;
using Hermann.Client.LearningClient.Generators;
using Hermann.Client.LearningClient.Judgers;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Managers
{
    /// <summary>
    /// 自動対戦の管理機能・情報を提供します。
    /// </summary>
    public class AutoPlayManager : IInjectable<AutoPlayManager.Config>
    {
        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// 設定情報
        /// </summary>
        public class Config
        {
            /// <summary>
            /// AIプレイヤのバージョン
            /// </summary>
            public AiPlayer.Version?[] Versions { get; set; }

            /// <summary>
            /// 使用するスライム
            /// </summary>
            public Slime[] UsingSlime { get; set; }

            /// <summary>
            /// ログを記録する対象のAIプレイヤバージョン
            /// </summary>
            public AiPlayer.Version LoggingVersion { get; set; }
        }

        /// <summary>
        /// プレイ上限回数
        /// </summary>
        public const int LimitPlayCount = 10000;

        /// <summary>
        /// 結果を表示する時間（ミリ秒）
        /// </summary>
        public const int ResultDisplayMillSec = 1000;

        /// <summary>
        /// 表示するフレーム数
        /// </summary>
        public const int DisplayFrameCount = 16;

        /// <summary>
        /// ログ記録対象の最低連鎖数
        /// </summary>
        private static readonly Dictionary<AiPlayer.Version, int> RequiredWriteResultLogMinChain = new Dictionary<AiPlayer.Version, int>()
        {
            {
                AiPlayer.Version.V3_0,
                4
            },
        };

        /// <summary>
        /// 設定情報
        /// </summary>
        /// <value>My config.</value>
        private Config MyConfig { get; set; }

        /// <summary>
        /// 結果評価で使用するパラメータ
        /// </summary>
        /// <value>The result eval parameter.</value>
        private ResultEvaluator.Param ResultEvalParam { get; set; }

        /// <summary>
        /// AIプレイヤ
        /// </summary>
        private AiPlayer AiPlayer { get; set; }

        /// <summary>
        /// 削除スライム分析機能
        /// </summary>
        private ErasedSlimeAnalyzer ErasedSlimeAnalyzer { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Inject(Config config)
        {
            this.MyConfig = config;
            this.HasInjected = true;
            this.Initialize();
        }

        /// <summary>
        /// 初期化処理を行います。
        /// </summary>
        private void Initialize()
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            Player.ForEach(player =>
            {
                if (this.MyConfig.Versions[player.ToInt()] != null)
                {
                    this.AiPlayer = LearningClientDiProvider.GetContainer().GetInstance<AiPlayer>();
                    this.AiPlayer.Inject(new AiPlayer.Config()
                    {
                        Version = this.MyConfig.Versions[player.ToInt()].Value,
                        UsingSlime = this.MyConfig.UsingSlime,
                    });
                }

            });
            this.ResultEvalParam = LearningClientDiProvider.GetContainer().GetInstance<ResultEvaluator.Param>();
            this.ErasedSlimeAnalyzer = LearningClientDiProvider.GetContainer().GetInstance<ErasedSlimeAnalyzer>();
        }

        /// <summary>
        /// フィールド表示が必要かどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if display was requireded, <c>false</c> otherwise.</returns>
        /// <param name="context">フィールド状態</param>
        /// <param name="frameCount">フレーム数</param>
        public bool RequiredDisplay(FieldContext context, int frameCount)
        {
            return context.OperationPlayer == Player.Index.First && frameCount % DisplayFrameCount == 0;
        }

        /// <summary>
        /// スライム移動が必要かどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if move was requireded, <c>false</c> otherwise.</returns>
        /// <param name="context">フィールド状態</param>
        /// <param name="frameCount">フレーム数</param>
        public bool RequiredMove(FieldContext context, int frameCount)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            var ret = true;
            if (this.MyConfig.Versions[context.OperationPlayer.ToInt()] == null) {
                // AIを利用しない場合
                ret = LearningClientDiProvider.GetContainer().GetInstance<RequiredMoveFrameCountJudger>().Judge(frameCount);
            }

            return ret;
        }

        /// <summary>
        /// 次に動かす方向を取得します。
        /// </summary>
        /// <returns>次に動かす方向</returns>
        /// <param name="context">フィールド状態</param>
        public Direction GetNext(FieldContext context)
        {
            if (this.MyConfig.Versions[context.OperationPlayer.ToInt()] == null)
            {
                // AIを利用しない自動対戦
                return LearningClientDiProvider.GetContainer().GetInstance<MoveDirectionRandomGenerator>().GetNext(context);
            }

            return this.AiPlayer.Think(context);
        }

        /// <summary>
        /// 状態ログを書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if write state log was requireded, <c>false</c> otherwise.</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public bool RequiredWriteStateLog(FieldContext lastContext, FieldContext context)
        {
            var ret = false;
            var player = context.OperationPlayer;
            var lastEv = lastContext.FieldEvent[player.ToInt()];
            var ev = context.FieldEvent[player.ToInt()];
            var lastChain = lastContext.Chain[player.ToInt()];
            var chain = context.Chain[player.ToInt()];

            switch (this.MyConfig.LoggingVersion)
            {
                case AiPlayer.Version.V1_0:
                    ret = (ev == FieldEvent.MarkErasing || ev == FieldEvent.NextPreparation);
                    break;
                case AiPlayer.Version.V2_0:
                    //ret = (lastEv == FieldEvent.NextPreparation && ev == FieldEvent.None);
                    ret = (ev == FieldEvent.MarkErasing || ev == FieldEvent.NextPreparation) ||
                        (lastEv == FieldEvent.NextPreparation && ev == FieldEvent.None);
                    break;
                case AiPlayer.Version.V3_0:
                    ret = (lastChain >= RequiredWriteResultLogMinChain[this.MyConfig.LoggingVersion] && chain == 0);
                    if (ret)
                    {
                        FileHelper.WriteLine("----- write state log target -----");
                        FileHelper.WriteLine("----- last context -----");
                        FileHelper.WriteLine(DebugHelper.FieldToString(lastContext));
                        FileHelper.WriteLine("----- context -----");
                        FileHelper.WriteLine(DebugHelper.FieldToString(context));
                    }
                    break;
                default:
                    throw new ArgumentException("バージョンが不正です");
            }

            return ret;
        }

        /// <summary>
        /// 状態ログの入力情報を取得します。
        /// </summary>
        /// <returns>状態ログの入力情報</returns>
        /// <param name="param">削除スライム分析機能のパラメータ</param>
        /// <param name="context">フィールド状態</param>
        public double[] GetStateLogInput(ErasedSlimeAnalyzer.Param param, FieldContext context)
        {
            var player = context.OperationPlayer;
            var _context = context;
            switch (this.MyConfig.LoggingVersion)
            {
                case AiPlayer.Version.V1_0:
                case AiPlayer.Version.V2_0:
                    break;
                case AiPlayer.Version.V3_0:
                    FileHelper.WriteLine("----- write state log input -----");
                    //_context = this.ErasedSlimeAnalyzer.Analyze(param);
                    //FileHelper.WriteLine(DebugHelper.FieldToString(_context));
                    _context = param.TargetContext;
                    break;
                default:
                    throw new ArgumentException("バージョンが不正です");
            }
            FileHelper.WriteLine(DebugHelper.FieldToString(_context));
            return LearningClientDiProvider.GetContainer().GetInstance<InputDataProvider>().
                                           GetVector(this.MyConfig.LoggingVersion, _context).ToArray();
        }

        /// <summary>
        /// 結果ログを書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if write result log was requireded, <c>false</c> otherwise.</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public bool RequiredWriteResultLog(FieldContext lastContext, FieldContext context)
        {
            var ret = false;
            var player = context.OperationPlayer;
            var ev = context.FieldEvent[player.ToInt()];
            var chain = context.Chain[player.ToInt()];

            switch (this.MyConfig.LoggingVersion)
            {
                case AiPlayer.Version.V1_0:
                case AiPlayer.Version.V2_0:
                    ret = (ev == FieldEvent.End);
                    break;
                case AiPlayer.Version.V3_0:
                    if (lastContext == null)
                    {
                        // 前回のフィールド状態がまだ記録されていないので結果は書き込まない
                        ret = false;
                    }
                    else 
                    {
                        var lastChain = lastContext.Chain[player.ToInt()];
                        ret = (lastChain >= RequiredWriteResultLogMinChain[this.MyConfig.LoggingVersion] && chain == 0);
                    }
                    break;
                default:
                    throw new ArgumentException("バージョンが不正です");
            }

            return ret;
        }

        /// <summary>
        /// 結果ログの入力情報を取得します。
        /// </summary>
        /// <returns>結果ログの入力情報</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public double GetResutlLogInput(FieldContext lastContext, FieldContext context)
        {
            var ret = 0.0d;
            this.ResultEvalParam.LastContext = lastContext;
            this.ResultEvalParam.Context = context;

            switch (this.MyConfig.LoggingVersion)
            {
                case AiPlayer.Version.V1_0:
                case AiPlayer.Version.V2_0:
                    this.ResultEvalParam.Parity = false;
                    ret = LearningClientDiProvider.GetContainer().GetInstance<ResultWinEvaluator>().Evaluate(this.ResultEvalParam);
                    break;
                case AiPlayer.Version.V3_0:
                    this.ResultEvalParam.Parity = false;
                    ret = LearningClientDiProvider.GetContainer().GetInstance<ResultScoreEvaluator>().Evaluate(this.ResultEvalParam);
                    break;
                default:
                    throw new ArgumentException("バージョンが不正です");
            }

            return ret;
        }
    }
}
