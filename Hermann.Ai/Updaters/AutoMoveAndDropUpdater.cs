using System;
using System.Collections.Generic;
using System.Diagnostics;
using Hermann.Ai.Di;
using Hermann.Contexts;
using Hermann.Models;
using Hermann.Updaters;

namespace Hermann.Ai.Updaters
{
    /// <summary>
    /// 自動移動機能を提供します。
    /// </summary>
    public class AutoMoveAndDropUpdater : IPlayerFieldParameterizedUpdatable<AutoMoveAndDropUpdater.Param>, IInjectable<Slime[]>
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 移動パターン
            /// </summary>
            /// <value>The pattern.</value>
            public IEnumerable<Direction> Pattern { get; set; }

            /// <summary>
            /// 自動移動中に発生した連鎖数
            /// </summary>
            /// <value>The max chain.</value>
            public int Chain { get; set; } = 0;
        }

        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// ゲーム
        /// </summary>
        /// <value>The game.</value>
        private Game Game { get; set; }

        /// <summary>
        /// 使用スライム
        /// </summary>
        private Slime[] UsingSlimes { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="usingSlimes">Using slimes.</param>
        public void Inject(Slime[] usingSlimes)
        {
            this.UsingSlimes = usingSlimes;
            this.Game = AiDiProvider.GetContainer().GetInstance<Game>();
            this.Game.Inject(usingSlimes);
            this.HasInjected = true;
        }

        public void Update(FieldContext context, Player.Index player, Param param)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");
            if (!(context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.None || context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.End))
            {
                return;
            }
#if DEBUG
            for (var i = 0; i < context.UsingSlimes.Length; i++)
            {
                Debug.Assert(context.UsingSlimes[i] == this.UsingSlimes[i], "使用スライムが不正です");
            }
#endif

            // 回転と横移動
            foreach (var p in param.Pattern)
            {
                context.OperationDirection = p;
                context = this.Game.Update(context);
            }

            // 最後まで落下させる
            while (!context.Ground[(int)player] && context.FieldEvent[(int)player] != FieldEvent.End)
            {
                context.OperationDirection = Direction.Down;
                context = this.Game.Update(context);
            }

            // 接地したら即設置完了にする
            context.BuiltRemainingTime[(int)player] = -1;
            context.OperationDirection = Direction.None;
            context = this.Game.Update(context);

            // イベント終了 or ゲーム終了まで更新する
            while (context.FieldEvent[(int)player] != FieldEvent.None && context.FieldEvent[(int)player] != FieldEvent.End)
            {
                context = this.Game.Update(context);
                param.Chain = Math.Max(param.Chain, context.Chain[player.ToInt()]);
            }
            Debug.Assert(context.FieldEvent[(int)player] == FieldEvent.None || context.FieldEvent[(int)player] == FieldEvent.End, "イベント発生中はありえません");
        }
    }
}
