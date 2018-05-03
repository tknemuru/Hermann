using Hermann.Ai.Evaluators;
using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Helper;
using Hermann.Helpers;
using Hermann.Ai.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Serchers
{
    /// <summary>
    /// NegaMax法の探索機能を提供します。
    /// </summary>
    public class NegaMax : NegaMaxTemplate
    {
        /// <summary>
        /// 深さの制限
        /// </summary>
        private const int LimitDepth = 2;

        /// <summary>
        /// 移動可能な方向の分析機能
        /// </summary>
        private MovableDirectionAnalyzer MovableDirectionAnalyzer { get; set; }

        /// <summary>
        /// フィールド状態の評価機能
        /// </summary>
        private LinearRegressionEvaluator Evaluator { get; set; }

        /// <summary>
        /// ゲームロジック
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NegaMax()
            : base()
        {
            this.MovableDirectionAnalyzer = AiDiProvider.GetContainer().GetInstance<MovableDirectionAnalyzer>();
            this.Evaluator = AiDiProvider.GetContainer().GetInstance<LinearRegressionEvaluator>();
        }

        /// <summary>
        /// 依存関係がある機能を注入します。
        /// </summary>
        /// <param name="game">ゲーム</param>
        public void Injection(Game game)
        {
            this.Game = game;
        }

        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns>深さ制限に達したかどうか</returns>
        protected override bool IsLimit(int limit)
        {
            return (limit >= LimitDepth);
        }

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected override double GetEvaluate(FieldContext context)
        {
            StopWatchLogger.StartEventWatch("SearchBestPointer-GetEvaluate");
            var score = this.Evaluator.Evaluate(context);
            StopWatchLogger.StopEventWatch("SearchBestPointer-GetEvaluate");
            FileHelper.WriteLine($"score:{score} direction:{context.OperationDirection}");

            return (score * this.GetParity(context));
        }

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Direction> GetAllLeaf(FieldContext context)
        {
            return this.MovableDirectionAnalyzer.Analyze(context, context.OperationPlayer);
        }

        /// <summary>
        /// ソートをする場合はTrueを返す
        /// </summary>
        /// <returns></returns>
        protected override bool IsOrdering(int depth)
        {
            return false;
        }

        /// <summary>
        /// ソートする
        /// </summary>
        /// <param name="allLeaf"></param>
        /// <returns></returns>
        protected override IEnumerable<Direction> MoveOrdering(IEnumerable<Direction> allLeaf, FieldContext context)
        {
            return allLeaf;
        }

        /// <summary>
        /// キーの初期値を取得する
        /// </summary>
        /// <returns></returns>
        protected override Direction GetDefaultKey()
        {
            return Direction.None;
        }

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected override FieldContext SearchSetUp(FieldContext context, Direction direction)
        {
            //var lastContext = context.DeepCopy();
            var player = context.OperationPlayer;

            // スライムを動かす
            Debug.Assert(context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.None || context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.End, "イベント発生中はありえません");
            context.OperationDirection = direction;
            var updContext = this.Game.Update(context);

            // 接地していたら即設置完了にする
            if (updContext.Ground[(int)player])
            {
                updContext.BuiltRemainingTime[(int)player] = -1;
                updContext.OperationDirection = Direction.None;
                updContext = this.Game.Update(updContext);
            }

            // イベントが発生したらイベント終了まで更新する
            if(updContext.FieldEvent[(int)player] != FieldEvent.None)
            {
                while (updContext.FieldEvent[(int)player] != FieldEvent.None)
                {
                    updContext = this.Game.Update(updContext);
                }
            }
            Debug.Assert(updContext.FieldEvent[(int)player] == FieldEvent.None || context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.End, "イベント発生中はありえません");

            // ターンをまわす
            updContext.OperationPlayer = updContext.OperationPlayer.GetOppositeIndex();

            //return lastContext;
            return context;
        }

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected override FieldContext SearchTearDown(FieldContext lastContext)
        {
            return lastContext;
        }

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected override FieldContext PassSetUp(FieldContext context)
        {
            //var lastContext = context.DeepCopy();
            var player = context.OperationPlayer;

            // スライムを動かす
            Debug.Assert(context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.None, "イベント発生中はありえません");
            context.OperationDirection = Direction.None;
            var updContext = this.Game.Update(context);

            // 接地していたら即設置完了にする
            if (updContext.Ground[(int)player])
            {
                updContext.BuiltRemainingTime[(int)player] = -1;
                updContext.OperationDirection = Direction.None;
                updContext = this.Game.Update(updContext);
            }

            // イベントが発生したらイベント終了まで更新する
            if (updContext.FieldEvent[(int)player] != FieldEvent.None)
            {
                while (updContext.FieldEvent[(int)player] != FieldEvent.None)
                {
                    updContext = this.Game.Update(updContext);
                }
            }
            Debug.Assert(updContext.FieldEvent[(int)player] == FieldEvent.None, "イベント発生中はありえません");

            // ターンをまわす
            context.OperationPlayer = context.OperationPlayer.GetOppositeIndex();

            //return lastContext;
            return context;
        }

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected override FieldContext PassTearDown(FieldContext lastContext)
        {
            return lastContext;
        }

        /// <summary>
        /// パリティ
        /// </summary>
        private int GetParity(FieldContext context)
        {
            return (context.OperationPlayer == Player.Index.First) ? 1 : -1;
        }
    }
}
