using Hermann.Models;
using Hermann.Contexts;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 移動可能スライムの更新機能を提供します。
    /// </summary>
    public sealed class MovableSlimesUpdater : IPlayerFieldParameterizedUpdatable<MovableSlimesUpdater.Option>
    {
        /// <summary>
        /// 更新オプション
        /// </summary>
        public enum Option
        {
            /// <summary>
            /// 初期状態にします。
            /// </summary>
            Initial,

            /// <summary>
            /// おじゃまスライム落下前の状態にします。
            /// </summary>
            BeforeDropObstruction,

            /// <summary>
            /// おじゃまスライム落下後の状態にします。
            /// </summary>
            AfterDropObstruction,
        }

        /// <summary>
        /// 移動可能スライムを更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="option">更新オプション</param>
        public void Update(FieldContext context, Player.Index player, Option option)
        {
            switch (option)
            {
                case Option.Initial:
                    this.Initialize(context, player, FieldContextConfig.MaxHiddenUnitIndex, FieldContextConfig.MovableSlimeInitialShiftAfterDroped);
                    break;
                case Option.BeforeDropObstruction:
                    this.Initialize(context, player, FieldContextConfig.MinHiddenUnitIndex, FieldContextConfig.MovableSlimeInitialShiftBeforeDroped);
                    break;
                case Option.AfterDropObstruction:
                    this.UpdateAfterDropObstruction(context, player);
                    break;
                default:
                    throw new ArgumentException("オプションが不正です。" + option);
            }
        }

        /// <summary>
        /// 移動可能スライムをおじゃまスライム落下前の状態に更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void Initialize(FieldContext context, Player.Index player, int index, int position)
        {
            var slimes = context.NextSlimes[(int)player][(int)NextSlime.Index.First];
            var movables = context.MovableSlimes[(int)player];

            // Nextスライムを移動可能スライムにセットする
            // フィールド上には移動可能スライムと同じ位置に通常のスライムが配置済なので、移動可能スライムの情報を書き換えれば通常のスライムに変わることになる
            MovableSlime.ForEach((unitIndex) =>
            {
                movables[(int)unitIndex].Slime = slimes[(int)unitIndex];
                movables[(int)unitIndex].Index = index;
                movables[(int)unitIndex].Position = position + ((int)unitIndex * FieldContextConfig.OneLineBitCount);
                context.MovableSlimes[(int)player][(int)unitIndex] = movables[(int)unitIndex];

                // フィールドにも反映させる
                context.SlimeFields[(int)player][movables[(int)unitIndex].Slime][movables[(int)unitIndex].Index] |= 1u << movables[(int)unitIndex].Position;
            });
        }

        /// <summary>
        /// 移動可能スライムをおじゃまスライム落下後の状態に更新します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        private void UpdateAfterDropObstruction(FieldContext context, Player.Index player)
        {
            var movables = context.MovableSlimes[(int)player];

            // Nextスライムを移動可能スライムにセットする
            // フィールド上には移動可能スライムと同じ位置に通常のスライムが配置済なので、移動可能スライムの情報を書き換えれば通常のスライムに変わることになる
            MovableSlime.ForEach((unitIndex) =>
            {
                // フィールド上から消す
                context.SlimeFields[(int)player][movables[(int)unitIndex].Slime][movables[(int)unitIndex].Index] &= ~(1u << movables[(int)unitIndex].Position);

                movables[(int)unitIndex].Index = FieldContextConfig.MaxHiddenUnitIndex;
                movables[(int)unitIndex].Position = FieldContextConfig.MovableSlimeInitialShiftAfterDroped + ((int)unitIndex * FieldContextConfig.OneLineBitCount);
                context.MovableSlimes[(int)player][(int)unitIndex] = movables[(int)unitIndex];

                // フィールドにも反映させる
                context.SlimeFields[(int)player][movables[(int)unitIndex].Slime][movables[(int)unitIndex].Index] |= 1u << movables[(int)unitIndex].Position;
            });
        }
    }
}
