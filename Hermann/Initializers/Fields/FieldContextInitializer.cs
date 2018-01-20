using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Helpers;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Initializers.Fields
{
    /// <summary>
    ///フィールド状態の初期化機能を提供します。
    /// </summary>
    public class FieldContextInitializer : IFieldInitializable
    {
        /// <summary>
        ///フィールド状態の初期化を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public void Initialize(FieldContext context)
        {
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
            context.UsingSlimes = DiProvider.GetContainer().GetInstance<UsingSlimeGenerator>().GetNext();

            // NEXTスライム
            var nextSlimeGen = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
            nextSlimeGen.UsingSlime = context.UsingSlimes;
            Player.ForEach((player) =>
            {
                NextSlime.ForEach((unit) =>
                {
                    context.NextSlimes[(int)player][(int)unit] = nextSlimeGen.GetNext();
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
                var movableSlimes = nextSlimeGen.GetNext();
                FieldContextHelper.SetMovableSlimeInitialPosition(context, player, movableSlimes);
            });
        }
    }
}
