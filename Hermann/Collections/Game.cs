using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// コンストラクタ
        /// </summary>
        public Game()
        {
            this.NextSlimeGen = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
            this.UsingSlimeGen = DiProvider.GetContainer().GetInstance<UsingSlimeGenerator>();
        }

        /// <summary>
        /// 初期状態のフィールドを作成します。
        /// </summary>
        /// <returns>初期状態のフィールド</returns>
        public FieldContext CreateInitialFieldContext()
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
                    context.SlimeFields[player].Add(fieldSlimes[slimeIndex], createInitialField());
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
                    context.SlimeFields[player][movable.Slime][movable.Index] |= 1u << movable.Position;
                }
            }

            return context;
        }
    }
}
