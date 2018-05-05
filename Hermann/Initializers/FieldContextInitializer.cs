using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Helpers;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Initializers
{
    /// <summary>
    ///フィールド状態の初期化機能を提供します。
    /// </summary>
    public class FieldContextInitializer : IFieldContextInitializable, IInjectable<FieldContextInitializer.Config>
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
            /// Nextスライム生成機能
            /// </summary>
            public NextSlimeGenerator NextSlimeGen { get; set; }

            /// <summary>
            /// 移動可能スライム更新機能
            /// </summary>
            public MovableSlimesUpdater MovableSlimeUp { get; set; }

            /// <summary>
            /// NEXTスライム更新機能
            /// </summary>
            public NextSlimeUpdater NextSlimeUp { get; set; }
        }

        /// <summary>
        /// 設定情報
        /// </summary>
        /// <value>My config.</value>
        private Config MyConfig { get; set; }

        /// <summary>
        /// 依存する機能を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Inject(Config config)
        {
            this.MyConfig = config;
            this.HasInjected = true;
        }

        /// <summary>
        ///フィールド状態の初期化を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Initialize(FieldContext context)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            // 操作方向
            context.OperationDirection = Direction.None;

            // イベント
            context.FieldEvent = new[] { FieldEvent.None, FieldEvent.None };

            // 回転方向
            context.RotationDirection = new[] { FieldContextConfig.InitialDirection, FieldContextConfig.InitialDirection };

            // 経過時間
            context.Time = 0;

            // 接地
            context.Ground = new[] { false, false };

            // 設置残タイム
            context.BuiltRemainingTime = new[] { FieldContextConfig.MaxBuiltRemainingTime, FieldContextConfig.MaxBuiltRemainingTime };

            // 使用済得点
            context.UsedScore = new[] { 0L, 0L };

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
            context.UsingSlimes = this.MyConfig.NextSlimeGen.UsingSlime;

            // NEXTスライム
            NextSlime.ForEach((unit) =>
            {
                var movables = this.MyConfig.NextSlimeGen.GetNext();
                Player.ForEach((player) =>
                {
                    context.NextSlimes[(int)player][(int)unit] = movables;
                });
            });

            // おじゃまスライム
            Player.ForEach((player) =>
            {
                foreach (var ob in Enum.GetValues(typeof(ObstructionSlime)))
                {
                    context.ObstructionSlimes[(int)player].Add((ObstructionSlime)ob, 0);
                }
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
                MovableSlime.ForEach((unitIndex) =>
                {
                    context.MovableSlimes[(int)player][(int)unitIndex] = DiProvider.GetContainer().GetInstance<MovableSlime>();
                });
                this.MyConfig.MovableSlimeUp.Update(context, player, MovableSlimesUpdater.Option.Initial);
                this.MyConfig.NextSlimeUp.Update(context, player);
            });
        }
    }
}
