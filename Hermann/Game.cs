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
using Hermann.Updaters.Times;
using Hermann.Initializers;

namespace Hermann
{
    /// <summary>
    /// ゲームロジックを提供します。
    /// </summary>
    public sealed class Game
    {
        /// <summary>
        /// スライム移動機能
        /// </summary>
        private SlimeMover SlimeMover { get; set; }

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
        ///フィールド状態の初期化機能
        /// </summary>
        private IFieldContextInitializable FieldContextInitializer { get; set; }

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
        /// おじゃまスライム配置機能
        /// </summary>
        private ObstructionSlimeSetter ObstructionSlimeSetter { get; set; }

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
            this.SlimeMover = DiProvider.GetContainer().GetInstance<SlimeMover>();
            this.TimeUpdater = DiProvider.GetContainer().GetInstance<ITimeUpdatable>();
            this.GroundUpdater = DiProvider.GetContainer().GetInstance<GroundUpdater>();
            this.BuiltingUpdater = DiProvider.GetContainer().GetInstance<MovableSlimesUpdater>();
            this.SlimeErasingMarker = DiProvider.GetContainer().GetInstance<SlimeErasingMarker>();
            this.WinCountUpdater = DiProvider.GetContainer().GetInstance<WinCountUpdater>();
            this.SlimeEraser = DiProvider.GetContainer().GetInstance<SlimeEraser>();
            this.Gravity = DiProvider.GetContainer().GetInstance<Gravity>();
            this.BuiltRemainingTimeUpdater = DiProvider.GetContainer().GetInstance<IBuiltRemainingTimeUpdatable>();
            this.RotationDirectionUpdater = DiProvider.GetContainer().GetInstance<RotationDirectionUpdater>();
            this.UsingSlimeGenerator = DiProvider.GetContainer().GetInstance<UsingSlimeGenerator>();
            this.MovableSlimesUpdater = DiProvider.GetContainer().GetInstance<MovableSlimesUpdater>();
            this.ObstructionSlimeCalculator = DiProvider.GetContainer().GetInstance<ObstructionSlimeCalculator>();
            this.ObstructionSlimeSetter = DiProvider.GetContainer().GetInstance<ObstructionSlimeSetter>();
            this.ScoreCalculator = DiProvider.GetContainer().GetInstance<ScoreCalculator>();
            this.ObstructionSlimeErasingMarker = DiProvider.GetContainer().GetInstance<ObstructionSlimeErasingMarker>();

            this.NextSlimeGenerator = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
            this.NextSlimeGenerator.UsingSlime = this.UsingSlimeGenerator.GetNext();
            this.NextSlimeUpdater = DiProvider.GetContainer().GetInstance<NextSlimeUpdater>();
            this.NextSlimeUpdater.Injection(this.NextSlimeGenerator);
            this.FieldContextInitializer = DiProvider.GetContainer().GetInstance<IFieldContextInitializable>();
            this.FieldContextInitializer.Injection(this.NextSlimeGenerator, this.MovableSlimesUpdater, this.NextSlimeUpdater);
        }

        /// <summary>
        /// ゲームの初期処理を行います。
        /// </summary>
        /// <returns>フィールド状態</returns>
        public FieldContext Start()
        {
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

            switch (context.FieldEvent[(int)player])
            {
                case FieldEvent.None:
                    this.Move(context);
                    break;
                case FieldEvent.StartChain:
                    this.StartChain(context, player);
                    break;
                case FieldEvent.MarkErasing:
                    this.MarkErasing(context, player);
                    break;
                case FieldEvent.SetObstructions:
                    this.SetObstructions(context, player);
                    break;
                case FieldEvent.Erase:
                    this.Erase(context, player);
                    break;
                case FieldEvent.DropSlimes:
                    this.DropSlimes(context, player);
                    break;
                case FieldEvent.DropObstructions:
                    this.DropObstructions(context, player);
                    break;
                case FieldEvent.NextPreparation:
                    this.NextPreparation(context, player);
                    break;
                default:
                    throw new ArgumentException("イベントが不正です");
            }

            return context;
        }

        /// <summary>
        /// スライムを移動します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        private void Move(FieldContext context)
        {
            var player = context.OperationPlayer;
            var moveParam = DiProvider.GetContainer().GetInstance<SlimeMover.Param>();

            // 設置残タイムの更新
            if (context.Ground[(int)player])
            {
                this.BuiltRemainingTimeUpdater.Update(context);
            }
            else
            {
                // 一度接地した後、移動で接地が解除される場合があるので、設置残タイムのリセットが必要
                this.BuiltRemainingTimeUpdater.Reset(context);
            }

            // 移動実行
            this.SlimeMover.Update(context, player, moveParam);

            if (moveParam.ResultState == SlimeMover.ResultState.Success)
            {
                // 回転が成功した場合は回転方向を変更する
                if (context.OperationDirection == Direction.Up)
                {
                    this.RotationDirectionUpdater.Update(context);
                }

                // 下への移動が成功した場合は得点計算を行う
                if (context.OperationDirection == Direction.Down)
                {
                    this.ScoreCalculator.Update(context, player, new ScoreCalculator.Param(moveParam.ResultDistance));
                }
            }

            Player.ForEach((p) =>
                {
                    // 接地の更新
                    this.GroundUpdater.Update(context, p);
                });

            if (context.BuiltRemainingTime[(int)player] <= 0)
            {
                // 連鎖開始
                context.FieldEvent[(int)player] = FieldEvent.StartChain;
            }
        }

        /// <summary>
        /// 連鎖処理を開始します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void StartChain(FieldContext context, Player.Index player)
        {
            // 移動可能スライムを通常のスライムに変換する
            this.MovableSlimesUpdater.Update(context, player, MovableSlimesUpdater.Option.BeforeDropObstruction);

            context.FieldEvent[(int)player] = FieldEvent.DropSlimes;
        }

        /// <summary>
        /// 消済にマーキングします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void MarkErasing(FieldContext context, Player.Index player)
        {
            // 消す対象のスライムを消済スライムとしてマーキングする
            var param = new SlimeErasingMarker.Param();
            this.SlimeErasingMarker.Update(context, player, param);

            // 隣接するおじゃまスライムを消済スライムとしてマーキングする
            this.ObstructionSlimeErasingMarker.Update(context, player);

            if (context.SlimeFields[(int)player][Slime.Erased].Any(f => f > 0))
            {
                // 連鎖数の更新
                context.Chain[(int)player]++;

                // 得点を計算
                this.ScoreCalculator.Update(context, player, new ScoreCalculator.Param(param.MaxLinkedCount, param.ColorCount, param.AllErased));

                // おじゃまスライムを算出して加算
                this.ObstructionSlimeCalculator.Update(context, player);

                context.FieldEvent[(int)player] = FieldEvent.Erase;
            }
            else
            {
                // 連鎖終了
                context.Chain[(int)player] = 0;

                var opposite = player.GetOppositeIndex();
                if(context.FieldEvent[(int)opposite] == FieldEvent.None ||
                    context.FieldEvent[(int)opposite] == FieldEvent.NextPreparation)
                {
                    context.FieldEvent[(int)player] = FieldEvent.SetObstructions;
                }
                else
                {
                    // 相手の連鎖が継続している場合は、おじゃまスライムを落とさない
                    context.FieldEvent[(int)player] = FieldEvent.NextPreparation;
                }
            }
        }

        /// <summary>
        /// 消済スライムを削除します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void Erase(FieldContext context, Player.Index player)
        {
            // 消済スライムを削除する
            this.SlimeEraser.Update(context);

            context.FieldEvent[(int)player] = FieldEvent.DropSlimes;
        }

        /// <summary>
        /// スライムの落下処理を行います。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="player"></param>
        private void DropSlimes(FieldContext context, Player.Index player)
        {
            var param = new Gravity.Param();

            // 重力で落とす
            this.Gravity.Update(context, player, param);

            // 落下処理完了
            if (param.ResultState == Gravity.ResultState.NotMoved)
            {
                context.FieldEvent[(int)player] = FieldEvent.MarkErasing;
            }
        }

        /// <summary>
        /// おじゃまスライムをフィールドに配置します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void SetObstructions(FieldContext context, Player.Index player)
        {
            // 自分自身のフィールドにおじゃまスライムを配置する
            this.ObstructionSlimeSetter.Update(context, player);

            context.FieldEvent[(int)player] = FieldEvent.DropObstructions;
        }

        /// <summary>
        /// おじゃまスライムの落下処理を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void DropObstructions(FieldContext context, Player.Index player)
        {
            var param = new Gravity.Param();

            // 自分自身のおじゃまスライムを落とす
            this.Gravity.Update(context, player, param);

            // 落下処理完了
            if (param.ResultState == Gravity.ResultState.NotMoved)
            {
                context.FieldEvent[(int)player] = FieldEvent.NextPreparation;
            }
        }

        /// <summary>
        /// 次のスライムを動かす準備を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void NextPreparation(FieldContext context, Player.Index player)
        {
            // 移動可能スライムを初期位置に移動する
            this.MovableSlimesUpdater.Update(context, player, MovableSlimesUpdater.Option.AfterDropObstruction);

            // 勝敗を決定する
            this.WinCountUpdater.Update(context, player);

            // 接地状態を更新する
            this.GroundUpdater.Update(context, player);

            // 回転方向を初期化する
            context.RotationDirection[(int)player] = FieldContextConfig.InitialDirection;

            // 設置残タイム
            this.BuiltRemainingTimeUpdater.Reset(context);

            // 新しいNextスライムを用意
            this.NextSlimeUpdater.Update(context, player);

            context.FieldEvent[(int)player] = FieldEvent.None;
        }
    }
}
