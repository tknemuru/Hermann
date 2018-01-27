using Hermann.Contexts;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers.Fields
{
    /// <summary>
    /// プレイヤの次にとるべき行動の分析機能を提供します。
    /// </summary>
    public class PlayerBehaviorAnalyzer : IPlayerFieldAnalyzable<PlayerBehavior>
    {
        /// <summary>
        /// プレイヤの次にとるべき行動を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>プレイヤの次にとるべき行動</returns>
        public PlayerBehavior Analyze(FieldContext context, Player.Index player)
        {
            var behavior = context.PlayerBehavior[(int)player];

            // プレイヤの行動が移動、かつ、設置残タイムが0以下だったら移動可能スライムを通常スライムに変換して連鎖開始
            if (behavior == PlayerBehavior.Move && context.BuiltRemainingTime[(int)player] <= 0)
            {
                return PlayerBehavior.ChangeMovableSlimes;
            }

            // 移動可能スライムを通常スライムに変換した後、おじゃまスライム算出後は、消済スライムにマーキング
            if (behavior == PlayerBehavior.ChangeMovableSlimes || behavior == PlayerBehavior.CalcObstructionSlime)
            {
                return PlayerBehavior.MarkErasingSlime;
            }

            // 消済スライムが1つ以上存在したらおじゃまスライムを算出
            if (behavior == PlayerBehavior.MarkErasingSlime && context.SlimeFields[(int)player][Slime.Erased].Any(f => f > 0))
            {
                return PlayerBehavior.CalcObstructionSlime;
            }

            // 設置が完了したらおじゃまスライム落下
            if (context.BuiltRemainingTime[(int)player] <= 0)
            {
                return PlayerBehavior.Drop;
            }

            // 通常移動
            return PlayerBehavior.Move;

            //if (context.Chain[(int)player] == 1)
            //{
            //    // 移動可能スライムを通常のスライムに変換する
            //    this.MovableSlimesUpdater.Update(context, player);
            //    // 重力で落とす
            //    this.Gravity.Update(context, player);

            //    context.Chain[(int)player]++;
            //}
            //else if (context.Chain[(int)player] % 2 == 0)
            //{
            //    // 消す対象のスライムを消済スライムとしてマーキングする
            //    this.SlimeErasingMarker.Update(context);

            //    // 連鎖数の更新
            //    if (context.SlimeFields[(int)player][Slime.Erased].Any(f => f > 0))
            //    {
            //        context.Chain[(int)player]++;
            //    }
            //    else
            //    {
            //        context.Chain[(int)player] = 0;
            //    }
            //}
            //else
            //{
            //    // おじゃまスライムを算出して加算
            //    this.ObstructionSlimeCalculator.Update(context, player);

            //    // 消済スライムを削除する
            //    this.SlimeEraser.Update(context);

            //    // 重力で落とす
            //    this.Gravity.Update(context, player);

            //    context.Chain[(int)player]++;
            //}

            //if (context.Chain[(int)player] > 0)
            //{
            //    // 連鎖数が1以上なら連鎖処理
            //    this.Chain(context, player);
            //}
            //else if (context.BuiltRemainingTime[(int)player] <= 0)
            //{
            //    // 設置残タイムが0未満
            //    this.Drop(context, player);
            //}
            //else
            //{
            //    // 設置残タイムの更新
            //    if (context.Ground[(int)player])
            //    {
            //        this.BuiltRemainingTimeUpdater.Update(context);
            //    }

            //    // それ以外は移動処理
            //    this.Move(context);
            //}
        }
    }
}
