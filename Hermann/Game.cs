using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Updaters;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;
using Reactive.Bindings.Extensions;
using Hermann.Updaters.Times;
using Hermann.Analyzers.Fields;
using Hermann.Initializers.Fields;

namespace Hermann
{
    /// <summary>
    /// ゲームロジックを提供します。
    /// </summary>
    public sealed class Game
    {
        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// プレイヤ
        /// </summary>
        private Player Player { get; set; }

        /// <summary>
        /// 方向：無で更新する間隔（ミリ秒）
        /// </summary>
        private int NoneDirectionUpdateInterval { get; set; }

        /// <summary>
        /// 方向：無で更新する回数
        /// </summary>
        private int NoneDirectionUpdateCount { get; set; }

        /// <summary>
        /// 時間更新機能
        /// </summary>
        private ITimeUpdatable TimeUpdater { get; set; }

        /// <summary>
        /// 接地更新機能
        /// </summary>
        private GroundUpdater GroundUpdater { get; set; }

        /// <summary>
        /// 設置に関するフィールドの更新機能
        /// </summary>
        private MovableSlimesUpdater BuiltingUpdater { get; set; }

        /// <summary>
        /// 消去対象のスライムを消済スライムとしてマーキングする機能
        /// </summary>
        private SlimeErasingMarker SlimeErasingMarker { get; set; }

        /// <summary>
        /// 勝数の更新機能
        /// </summary>
        private WinCountUpdater WinCountUpdater { get; set; }

        /// <summary>
        /// スライム消去機能
        /// </summary>
        private SlimeEraser SlimeEraser { get; set; }

        /// <summary>
        /// 重力
        /// </summary>
        private Gravity Gravity { get; set; }

        /// <summary>
        /// NEXTスライム更新機能
        /// </summary>
        private NextSlimeUpdater NextSlimeUpdater { get; set; }

        /// <summary>
        /// 設置残タイム更新機能
        /// </summary>
        private IBuiltRemainingTimeUpdatable BuiltRemainingTimeUpdater { get; set; }

        /// <summary>
        /// 回転方向更新機能
        /// </summary>
        private RotationDirectionUpdater RotationDirectionUpdater { get; set; }

        /// <summary>
        /// 回転方向初期化機能
        /// </summary>
        private RotationDirectionInitializer RotationDirectionInitializer { get; set; }

        /// <summary>
        /// 移動可能スライムの状態の分析機能
        /// </summary>
        private MovableSlimeStateAnalyzer MovableSlimeStateAnalyzer { get; set; }

        /// <summary>
        /// 設置残タイムの初期化機能
        /// </summary>
        private BuiltRemainingTimeInitializer BuiltRemainingTimeInitializer { get; set; }

        /// <summary>
        ///フィールド状態の初期化機能
        /// </summary>
        private FieldContextInitializer FieldContextInitializer { get; set; }

        /// <summary>
        /// 移動可能スライムの更新機能
        /// </summary>
        private MovableSlimesUpdater MovableSlimesUpdater { get; set; }

        /// <summary>
        /// NEXTスライムの生成機能
        /// </summary>
        private NextSlimeGenerator NextSlimeGenerator { get; set; }

        /// <summary>
        /// 使用スライムの生成機能
        /// </summary>
        private UsingSlimeGenerator UsingSlimeGenerator { get; set; }

        /// <summary>
        /// おじゃまスライム数の計算機能
        /// </summary>
        private ObstructionSlimeCalculator ObstructionSlimeCalculator { get; set; }

        /// <summary>
        /// おじゃまスライム落下機能
        /// </summary>
        private ObstructionSlimeRandomDropper ObstructionSlimeDropper { get; set; }

        /// <summary>
        ///　得点計算機能
        /// </summary>
        private ScoreCalculator ScoreCalculator { get; set; }

        /// <summary>
        /// おじゃまスライム消去機能
        /// </summary>
        private ObstructionSlimeErasingMarker ObstructionSlimeErasingMarker { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Game()
        {
            this.NoneDirectionUpdateInterval = 1000;
            this.NoneDirectionUpdateCount = 0;
            this.Player = DiProvider.GetContainer().GetInstance<Player>();
            this.TimeUpdater = DiProvider.GetContainer().GetInstance<ITimeUpdatable>();
            this.GroundUpdater = DiProvider.GetContainer().GetInstance<GroundUpdater>();
            this.BuiltingUpdater = DiProvider.GetContainer().GetInstance<MovableSlimesUpdater>();
            this.SlimeErasingMarker = DiProvider.GetContainer().GetInstance<SlimeErasingMarker>();
            this.WinCountUpdater = DiProvider.GetContainer().GetInstance<WinCountUpdater>();
            this.SlimeEraser = DiProvider.GetContainer().GetInstance<SlimeEraser>();
            this.Gravity = DiProvider.GetContainer().GetInstance<Gravity>();
            this.BuiltRemainingTimeUpdater = DiProvider.GetContainer().GetInstance<IBuiltRemainingTimeUpdatable>();
            this.RotationDirectionUpdater = DiProvider.GetContainer().GetInstance<RotationDirectionUpdater>();
            this.RotationDirectionInitializer = DiProvider.GetContainer().GetInstance<RotationDirectionInitializer>();
            this.MovableSlimeStateAnalyzer = DiProvider.GetContainer().GetInstance<MovableSlimeStateAnalyzer>();
            this.BuiltRemainingTimeInitializer = DiProvider.GetContainer().GetInstance<BuiltRemainingTimeInitializer>();
            this.UsingSlimeGenerator = DiProvider.GetContainer().GetInstance<UsingSlimeGenerator>();
            this.MovableSlimesUpdater = DiProvider.GetContainer().GetInstance<MovableSlimesUpdater>();
            this.ObstructionSlimeCalculator = DiProvider.GetContainer().GetInstance<ObstructionSlimeCalculator>();
            this.ObstructionSlimeDropper = DiProvider.GetContainer().GetInstance<ObstructionSlimeRandomDropper>();
            this.ScoreCalculator = DiProvider.GetContainer().GetInstance<ScoreCalculator>();
            this.ObstructionSlimeErasingMarker = DiProvider.GetContainer().GetInstance<ObstructionSlimeErasingMarker>();

            this.NextSlimeGenerator = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
            this.NextSlimeGenerator.UsingSlime = this.UsingSlimeGenerator.GetNext();
            this.NextSlimeUpdater = DiProvider.GetContainer().GetInstance<NextSlimeUpdater>();
            this.NextSlimeUpdater.Injection(this.NextSlimeGenerator);
            this.FieldContextInitializer = DiProvider.GetContainer().GetInstance<FieldContextInitializer>();
            this.FieldContextInitializer.Injection(this.NextSlimeGenerator, this.MovableSlimesUpdater, this.NextSlimeUpdater);
        }

        /// <summary>
        /// ゲームの初期処理を行います。
        /// </summary>
        /// <returns>フィールド状態</returns>
        public FieldContext Start()
        {
            // 方向：無で更新する回数カウントイベント
            Observable.Interval(TimeSpan.FromMilliseconds(this.NoneDirectionUpdateInterval))
                .Subscribe(_ =>
                {
                    this.NoneDirectionUpdateCount++;
                });

            var context = DiProvider.GetContainer().GetInstance<FieldContext>();
            this.FieldContextInitializer.Initialize(context);

            return context;
        }

        /// <summary>
        /// ゲーム状態の更新処理を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>更新されたフィールド状態</returns>
        public FieldContext Update(FieldContext context)
        {
            var player = context.OperationPlayer;

            // 時間の更新
            this.TimeUpdater.Update(context);

            if (context.Chain[(int)player] > 0)
            {
                // 連鎖数が1以上なら連鎖処理
                this.Chain(context, player);
            }
            else if (context.BuiltRemainingTime[(int)player] <= 0)
            {
                // 設置残タイムが0未満
                this.Drop(context, player);
            }
            else
            {
                // 設置残タイムの更新
                if (context.Ground[(int)player])
                {
                    this.BuiltRemainingTimeUpdater.Update(context);
                }

                // それ以外は移動処理
                this.Move(context);
            }

            return context;
        }

        /// <summary>
        /// スライムを移動します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        private void Move(FieldContext context)
        {
            // プレイヤの操作による移動
            if (context.OperationDirection != Direction.None)
            {
                this.Player.Update(context, context.OperationPlayer);

                // 回転が成功した場合は回転方向を変更する
                if (context.OperationDirection == Direction.Up && this.Player.Notifier.Value == Player.MoveResult.Success)
                {
                    this.RotationDirectionUpdater.Update(context);
                }
            }

            // 方向：無での更新
            var count = this.NoneDirectionUpdateCount;
            this.NoneDirectionUpdateCount = 0;
            for (var i = 0; i < count; i++)
            {
                // 移動
                Player.ForEach((p) =>
                {
                    if (context.BuiltRemainingTime[(int)p] > 0)
                    {
                        context.OperationDirection = Direction.None;

                        // 移動
                        this.Player.Update(context, p);
                    }
                });
            }

            Player.ForEach((player) =>
                {
                    // 接地の更新
                    this.GroundUpdater.Update(context, player);
                });

            if (this.MovableSlimeStateAnalyzer.Analyze(context, context.OperationPlayer) == MovableSlimeStateAnalyzer.Status.HasBuilt)
            {
                // 連鎖開始
                context.Chain[(int)context.OperationPlayer]++;
            }
        }

        /// <summary>
        /// 連鎖処理を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void Chain(FieldContext context, Player.Index player)
        {
            if (context.Chain[(int)player] == 1)
            {
                // 移動可能スライムを通常のスライムに変換する
                this.MovableSlimesUpdater.Update(context, player, MovableSlimesUpdater.Option.BeforeDropObstruction);
                // 重力で落とす
                this.Gravity.Update(context, player);

                context.Chain[(int)player]++;
            }
            else if (context.Chain[(int)player] % 2 == 0)
            {
                // 消す対象のスライムを消済スライムとしてマーキングする
                this.SlimeErasingMarker.Update(context);

                // 隣接するおじゃまスライムを消済スライムとしてマーキングする
                this.ObstructionSlimeErasingMarker.Update(context, player);

                // 連鎖数の更新
                if (context.SlimeFields[(int)player][Slime.Erased].Any(f => f > 0))
                {
                    context.Chain[(int)player]++;
                }
                else
                {
                    context.Chain[(int)player] = 0;
                }
            }
            else
            {
                // 得点を計算
                this.ScoreCalculator.Update(context, player);

                // おじゃまスライムを算出して加算
                this.ObstructionSlimeCalculator.Update(context, player);

                // 消済スライムを削除する
                this.SlimeEraser.Update(context);

                // 重力で落とす
                this.Gravity.Update(context, player);

                context.Chain[(int)player]++;
            }
        }

        /// <summary>
        /// おじゃまスライムの落下処理を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void Drop(FieldContext context, Player.Index player)
        {
            if (ObstructionSlimeHelper.ExistsObstructionSlime(context.ObstructionSlimes[(int)player]))
            {
                // 自分自身のおじゃまスライムを落とす
                this.ObstructionSlimeDropper.Update(context, player);
                this.Gravity.Update(context, player);
            }

            // 移動可能スライムを初期位置に移動する
            this.MovableSlimesUpdater.Update(context, player, MovableSlimesUpdater.Option.AfterDropObstruction);

            // 勝敗を決定する
            this.WinCountUpdater.Update(context, player);

            // 接地状態を更新する
            this.GroundUpdater.Update(context, player);

            // 回転方向を初期化する
            this.RotationDirectionInitializer.Update(context);

            // 設置残タイム
            this.BuiltRemainingTimeInitializer.Initialize(context);

            // 新しいNextスライムを用意
            this.NextSlimeUpdater.Update(context, player);
        }
    }
}
