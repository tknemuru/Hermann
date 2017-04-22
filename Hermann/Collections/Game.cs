using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;

namespace Hermann.Collections
{
    /// <summary>
    /// ゲームロジックを提供します。
    /// </summary>
    public sealed class Game
    {
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
        /// 接地更新機能
        /// </summary>
        private GroundUpdater GroundUpdater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Game()
        {
            this.NoneDirectionUpdateInterval = 1000;
            this.NoneDirectionUpdateCount = 0;
            this.NextSlimeGen = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
            this.UsingSlimeGen = DiProvider.GetContainer().GetInstance<UsingSlimeGenerator>();
            this.GroundUpdater = DiProvider.GetContainer().GetInstance<GroundUpdater>();
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
            context.SlimeFields.Subscribe(f =>
            {
                this.GroundUpdater.Update(context);
            });

            return context;
        }

        /// <summary>
        /// ゲーム状態の更新処理を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>更新されたフィールド状態</returns>
        public FieldContext Update(FieldContext context)
        {
            // 方向：無での更新
            var count = this.NoneDirectionUpdateCount;
            this.NoneDirectionUpdateCount = 0;
            for (var i = 0; i < count; i++)
            {
                for (var player = 0; player < Player.Length; player++)
                {
                    context.OperationPlayer = player;
                    context.OperationDirection = Direction.None;
                    context = Player.Move(context);
                }
            }

            // プレイヤの操作による移動
            if (context.OperationDirection != Direction.None)
            {
                context = Player.Move(context);
            }

            return context;
        }

        /// <summary>
        /// 購読登録を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        private void Subscribe(FieldContext context)
        {
            // 接地判定
            context.SlimeFields.Subscribe(f =>
            {
                this.GroundUpdater.Update(context);
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

            // 経過時間
            context.Time = 0;

            // 接地
            context.Ground = new[] { false, false };

            // 設置残タイム
            context.BuiltRemainingTime = new[] { 0, 0 };

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
            for (var player = 0; player < Player.Length; player++)
            {
                for (var unit = 0; unit < NextSlime.Length; unit++)
                {
                    context.NextSlimes[player][unit] = this.NextSlimeGen.GetNext();
                }
            }

            // フィールド
            Func<uint[]> createInitialField = () =>
            {
                var field = Enumerable.Range(0, FieldContextConfig.FieldUnitCount).Select(i => 0u);
                return field.ToArray();
            };
            var fieldSlimes = ExtensionSlime.Slimes.ToArray();
            for (var player = 0; player < Player.Length; player++)
            {
                for (var slimeIndex = 0; slimeIndex < fieldSlimes.Length; slimeIndex++)
                {
                    context.SlimeFields.Value[player].Add(fieldSlimes[slimeIndex], createInitialField());
                }
            }

            // 移動可能なスライム
            for (var player = 0; player < Player.Length; player++)
            {
                var movableSlimes = this.NextSlimeGen.GetNext();
                for (var unitIndex = 0; unitIndex < MovableSlime.Length; unitIndex++)
                {
                    var movable = new MovableSlime();
                    movable.Slime = movableSlimes[unitIndex];
                    movable.Index = FieldContextConfig.HiddenUnitIndex;
                    movable.Position = FieldContextConfig.MovableSlimeInitialShift + (unitIndex * FieldContextConfig.OneLineBitCount);
                    context.MovableSlimes[player][unitIndex] = movable;

                    // フィールドにも反映させる
                    context.SlimeFields.Value[player][movable.Slime][movable.Index] |= 1u << movable.Position;
                }
            }

            return context;
        }
    }
}
