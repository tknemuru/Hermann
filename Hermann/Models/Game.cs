﻿using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Updaters;
using Hermann.Helpers;
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

namespace Hermann.Models
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
        /// 使用スライム生成機能
        /// </summary>
        private UsingSlimeGenerator UsingSlimeGen { get; set; }

        /// <summary>
        /// Nextスライム生成機能
        /// </summary>
        private NextSlimeGenerator NextSlimeGen { get; set; }

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
        private TimeUpdater TimeUpdater { get; set; }

        /// <summary>
        /// 接地更新機能
        /// </summary>
        private GroundUpdater GroundUpdater { get; set; }

        /// <summary>
        /// 設置に関するフィールドの更新機能
        /// </summary>
        private BuiltingUpdater BuiltingUpdater { get; set; }

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
        private BuiltRemainingTimeUpdater BuiltRemainingTimeUpdater { get; set; }

        /// <summary>
        /// 回転方向更新機能
        /// </summary>
        private RotationDirectionUpdater RotationDirectionUpdater { get; set; }

        /// <summary>
        /// 回転方向初期化機能
        /// </summary>
        private RotationDirectionInitializer RotationDirectionInitializer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Game()
        {
            this.NoneDirectionUpdateInterval = 1000;
            this.NoneDirectionUpdateCount = 0;
            this.Player = DiProvider.GetContainer().GetInstance<Player>();
            this.NextSlimeGen = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
            this.UsingSlimeGen = DiProvider.GetContainer().GetInstance<UsingSlimeGenerator>();
            this.TimeUpdater = DiProvider.GetContainer().GetInstance<TimeUpdater>();
            this.GroundUpdater = DiProvider.GetContainer().GetInstance<GroundUpdater>();
            this.BuiltingUpdater = DiProvider.GetContainer().GetInstance<BuiltingUpdater>();
            this.SlimeErasingMarker = DiProvider.GetContainer().GetInstance<SlimeErasingMarker>();
            this.WinCountUpdater = DiProvider.GetContainer().GetInstance<WinCountUpdater>();
            this.SlimeEraser = DiProvider.GetContainer().GetInstance<SlimeEraser>();
            this.Gravity = DiProvider.GetContainer().GetInstance<Gravity>();
            //this.NextSlimeUpdater = DiProvider.GetContainer().GetInstance<NextSlimeUpdater>();
            this.BuiltRemainingTimeUpdater = DiProvider.GetContainer().GetInstance<BuiltRemainingTimeUpdater>();
            this.RotationDirectionUpdater = DiProvider.GetContainer().GetInstance<RotationDirectionUpdater>();
            this.RotationDirectionInitializer = DiProvider.GetContainer().GetInstance<RotationDirectionInitializer>();
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

            var context = this.CreateInitialFieldContext();

            // 購読登録
            this.Subscribe(context);

            return context;
        }

        /// <summary>
        /// ゲーム状態の更新処理を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>更新されたフィールド状態</returns>
        public FieldContext Update(FieldContext context)
        {
            // プレイヤの操作による移動
            if (context.OperationDirection != Direction.None)
            {
                context = this.Player.Move(context);

                // 上に移動の場合は回転方向を変更する
                if (context.OperationDirection == Direction.Up)
                {
                    this.RotationDirectionUpdater.Update(context);
                }
            }

            // 方向：無での更新
            var count = this.NoneDirectionUpdateCount;
            this.NoneDirectionUpdateCount = 0;
            for (var i = 0; i < count; i++)
            {
                Player.ForEach((player) =>
                {
                    context.OperationPlayer = player;
                    context.OperationDirection = Direction.None;
                    context = this.Player.Move(context);
                });
            }

            // 設置残タイムの更新
            Player.ForEach((player) =>
            {
                context.OperationPlayer = player;
                this.BuiltRemainingTimeUpdater.Update(context);
            });            

            // 時間の更新
            this.TimeUpdater.Update(context);

            return context;
        }

        /// <summary>
        /// 購読登録を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        private void Subscribe(FieldContext context)
        {
            this.Player.Notifier.Subscribe(n =>
                {
                    // 移動毎に接地判定を行う
                    this.GroundUpdater.Update(context);
                });

            // 設置完了時
            this.BuiltRemainingTimeUpdater.Notifier.Where(n => n == BuiltRemainingTimeUpdater.Notification.HasBuilt).Subscribe(n =>
            {
                // 1.移動可能スライムを通常のスライムに変換する
                this.BuiltingUpdater.Update(context);

                // 2.接地状態を更新する
                this.GroundUpdater.Update(context);

                // 3.消す対象のスライムを消済スライムとしてマーキングする
                this.SlimeErasingMarker.Update(context);

                // 4.回転方向を初期化する
                this.RotationDirectionInitializer.Update(context);

                this.SlimeEraser.Update(context);

                this.NextSlimeUpdater.Update(context);

                this.Gravity.Update(context);

                // 4.勝敗を決定する
                this.WinCountUpdater.Update(context);
            });
        }

        /// <summary>
        /// 初期状態のフィールドを作成します。
        /// </summary>
        /// <returns>初期状態のフィールド</returns>
        private FieldContext CreateInitialFieldContext()
        {
            var context = DiProvider.GetContainer().GetInstance<FieldContext>();

            // 操作方向
            context.OperationDirection = Direction.None;

            // 回転方向
            context.RotationDirection = new[] { Direction.Right, Direction.Right };

            // 経過時間
            context.Time = 0;

            // 接地
            context.Ground = new[] { false, false };

            // 設置残タイム
            context.BuiltRemainingTime = new[] { 0L, 0L };

            // 得点
            context.Score = new[] { 0L, 0L };

            // 連鎖
            context.Chain = new[] { 0, 0 };

            // 相殺
            context.Offset = new[] { false, false };

            // 全消
            context.AllErase = new[] { false, false };

            // 勝数
            context.WinCount = new[] { 0, 0 };

            // 使用スライム
            context.UsingSlimes = this.UsingSlimeGen.GetNext();

            // NEXTスライム
            this.NextSlimeGen.UsingSlime = context.UsingSlimes;
            this.NextSlimeUpdater = new NextSlimeUpdater(this.NextSlimeGen);
            Player.ForEach((player) =>
            {
                NextSlime.ForEach((unit) =>
                {
                    context.NextSlimes[(int)player][(int)unit] = this.NextSlimeGen.GetNext();
                });
            });

            // フィールド
            Func<uint[]> createInitialField = () =>
            {
                var field = Enumerable.Range(0, FieldContextConfig.FieldUnitCount).Select(i => 0u);
                return field.ToArray();
            };
            var fieldSlimes = ExtensionSlime.Slimes.ToArray();
            Player.ForEach((player) =>
            {
                for (var slimeIndex = 0; slimeIndex < fieldSlimes.Length; slimeIndex++)
                {
                    context.SlimeFields[(int)player].Add(fieldSlimes[slimeIndex], createInitialField());
                }
            });

            // 移動可能なスライム
            Player.ForEach((player) =>
            {
                var movableSlimes = this.NextSlimeGen.GetNext();
                FieldContextHelper.SetMovableSlimeInitialPosition(context, player, movableSlimes);
            });

            return context;
        }
    }
}