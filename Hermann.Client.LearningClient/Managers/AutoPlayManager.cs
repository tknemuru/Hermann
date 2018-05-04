using System;
using System.Linq;
using Hermann.Ai.Providers;
using Hermann.Client.LearningClient.Di;
using Hermann.Client.LearningClient.Evaluators;
using Hermann.Client.LearningClient.Generators;
using Hermann.Client.LearningClient.Judgers;
using Hermann.Contexts;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Managers
{
    /// <summary>
    /// 自動対戦の管理機能・情報を提供します。
    /// </summary>
    public class AutoPlayManager
    {
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
        /// 状態ログの書き込み要否判定で使用するパラメータ
        /// </summary>
        private RequiredWriteStateLogJudger.Param StateLogJudgeParam { get; set; }

        /// <summary>
        /// 結果ログの書き込み要否判定で使用するパラメータ
        /// </summary>
        /// <value>The result log judge parameter.</value>
        private RequiredWriteResultLogWinJudger.Param ResultLogJudgeParam { get; set; }

        /// <summary>
        /// 結果評価で使用するパラメータ
        /// </summary>
        /// <value>The result eval parameter.</value>
        private ResultEvaluator.Param ResultEvalParam { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoPlayManager()
        {
            this.StateLogJudgeParam = LearningClientDiProvider.GetContainer().GetInstance<RequiredWriteStateLogJudger.Param>();
            this.ResultLogJudgeParam = LearningClientDiProvider.GetContainer().GetInstance<RequiredWriteResultLogWinJudger.Param>();
            this.ResultEvalParam = LearningClientDiProvider.GetContainer().GetInstance<ResultEvaluator.Param>();
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
        /// <param name="frameCount">フレーム数</param>
        public bool RequiredMove(int frameCount)
        {
            return LearningClientDiProvider.GetContainer().GetInstance<RequiredMoveFrameCountJudger>().Judge(frameCount);
        }

        /// <summary>
        /// 次に動かす方向を取得します。
        /// </summary>
        /// <returns>次に動かす方向</returns>
        /// <param name="context">フィールド状態</param>
        public Direction GetNext(FieldContext context)
        {
            return LearningClientDiProvider.GetContainer().GetInstance<MoveDirectionRandomGenerator>().GetNext(context);
        }

        /// <summary>
        /// 状態ログを書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if write state log was requireded, <c>false</c> otherwise.</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public bool RequiredWriteStateLog(FieldContext lastContext, FieldContext context)
        {
            this.StateLogJudgeParam.LastContext = lastContext;
            this.StateLogJudgeParam.Context = context;
            return LearningClientDiProvider.GetContainer().GetInstance<RequiredWriteStateLogEventJudger>().Judge(this.StateLogJudgeParam);
        }

        /// <summary>
        /// 状態ログの入力情報を取得します。
        /// </summary>
        /// <returns>状態ログの入力情報</returns>
        /// <param name="context">フィールド状態</param>
        public double[] GetStateLogInput(FieldContext context)
        {
            return LearningClientDiProvider.GetContainer().GetInstance<InputDataProvider>().
                                           GetVector(InputDataProvider.Vector.Main, context).ToArray();
        }

        /// <summary>
        /// 結果ログを書き込む必要があるかどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if write result log was requireded, <c>false</c> otherwise.</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public bool RequiredWriteResultLog(FieldContext lastContext, FieldContext context)
        {            
            this.ResultLogJudgeParam.LastContext = lastContext;
            this.ResultLogJudgeParam.Context = context;
            return LearningClientDiProvider.GetContainer().GetInstance<RequiredWriteResultLogWinJudger>().Judge(this.ResultLogJudgeParam);
        }

        /// <summary>
        /// 結果ログの入力情報を取得します。
        /// </summary>
        /// <returns>結果ログの入力情報</returns>
        /// <param name="lastContext">前回のフィールド状態</param>
        /// <param name="context">フィールド状態</param>
        public double GetResutlLogInput(FieldContext lastContext, FieldContext context)
        {
            this.ResultEvalParam.LastContext = lastContext;
            this.ResultEvalParam.Context = context;
            return LearningClientDiProvider.GetContainer().GetInstance<ResultScoreDiffEvaluator>().Evaluate(this.ResultEvalParam);
        }
    }
}
