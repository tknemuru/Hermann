using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Analyzers;
using Assets.Scripts.Containers;
using Hermann.Models;
using Hermann.Contexts;
using Assets.Scripts.Models;
using Hermann.Helpers;

namespace Assets.Scripts.Analyzers
{
    /// <summary>
    /// 効果音に関する分析機能を提供します。
    /// </summary>
    public class SoundEffectAnalyzer : IPlayerFieldParameterizedAnalyzable<SoundEffectDecorationContainer, SoundEffectDecorationContainer>
    {
        /// <summary>
        /// 単発おじゃまスライムの効果音をならす最大おじゃまスライム数
        /// </summary>
        private const int MaxSingleObstructionCount = 17;

        /// <summary>
        /// 攻撃効果音辞書
        /// </summary>
        private static Dictionary<int, SoundEffect> AttackEffectDic = BuildAttackEffectDic();

        /// <summary>
        /// 指定したプレイヤのフィールド状態を分析します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        /// <returns>分析結果</returns>
        public SoundEffectDecorationContainer Analyze(FieldContext context, Player.Index player, SoundEffectDecorationContainer container)
        {
            container.ClearRequired();

            var lastContext = container.LastFieldContext;
            var pIndex = (int)player;

            // おじゃまスライム数の記録
            if(context.FieldEvent[pIndex] == FieldEvent.MarkErasing)
            {
                container.ObstructionSlimeCount = ObstructionSlimeHelper.ObstructionsToCount(context.ObstructionSlimes[pIndex]);
            }

            // 攻撃
            if(lastContext.Chain[pIndex] < context.Chain[pIndex])
            {
                if (AttackEffectDic.ContainsKey(context.Chain[pIndex]))
                {
                    container.Required[AttackEffectDic[context.Chain[pIndex]]] = true;
                }
                else
                {
                    container.Required[SoundEffect.Attack3] = true;
                }
            }

            // 削除
            container.Required[SoundEffect.Erase] =
                lastContext.FieldEvent[pIndex] == FieldEvent.Erase &&
                context.FieldEvent[pIndex] == FieldEvent.DropSlimes;

            // 接地
            container.Required[SoundEffect.Ground] = !lastContext.Ground[pIndex] && context.Ground[pIndex];

            // 移動
            var isMove = false;
            MovableSlime.ForEach(movable =>
            {
                if(context.MovableSlimes[pIndex][(int)movable].Index != lastContext.MovableSlimes[pIndex][(int)movable].Index ||
                    context.MovableSlimes[pIndex][(int)movable].Position != lastContext.MovableSlimes[pIndex][(int)movable].Position)
                {
                    isMove = true;
                }
            });
            container.Required[SoundEffect.Move] =
                isMove &&
                context.OperationPlayer == player &&
                context.OperationDirection == Direction.Up;

            // 複数おじゃまスライム落下
            container.Required[SoundEffect.Obstructions] =
                lastContext.FieldEvent[pIndex] == FieldEvent.DropObstructions &&
                context.FieldEvent[pIndex] == FieldEvent.NextPreparation &&
                container.ObstructionSlimeCount > MaxSingleObstructionCount;

            // 相殺
            container.Required[SoundEffect.Offset] = HasOffset(lastContext, context, player);

            // 単発おじゃまスライム落下
            container.Required[SoundEffect.SingleObstruction] =
                lastContext.FieldEvent[pIndex] == FieldEvent.DropObstructions &&
                context.FieldEvent[pIndex] == FieldEvent.NextPreparation &&
                container.ObstructionSlimeCount > 0 &&
                container.ObstructionSlimeCount <= MaxSingleObstructionCount;

            return container;
        }

        /// <summary>
        /// 攻撃効果音辞書を作成します。
        /// </summary>
        /// <returns>攻撃効果音辞書</returns>
        private static Dictionary<int, SoundEffect> BuildAttackEffectDic()
        {
            return new Dictionary<int, SoundEffect>()
            {
                {1, SoundEffect.Attack1},
                {2, SoundEffect.Attack1},
                {3, SoundEffect.Attack2},
                {4, SoundEffect.Attack2},
            };
        }

        /// <summary>
        /// 相殺が発生したかどうかを取得します。
        /// </summary>
        /// <param name="last">前回のフィールド状態</param>
        /// <param name="context">今回のフィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>相殺が発生したかどうか</returns>
        private static bool HasOffset(FieldContext last, FieldContext context, Player.Index player)
        {
            var lastObsScore = ObstructionSlimeHelper.ObstructionsToScore(last.ObstructionSlimes[(int)player]);
            var obsScore = ObstructionSlimeHelper.ObstructionsToScore(context.ObstructionSlimes[(int)player]);
            return (obsScore < lastObsScore) &&
                last.FieldEvent[(int)player] == FieldEvent.MarkErasing &&
                context.FieldEvent[(int)player] == FieldEvent.Erase;
        }
    }
}
