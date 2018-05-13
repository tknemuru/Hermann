using Hermann.Ai.Serchers;
using Hermann.Contexts;
using Hermann.Ai.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Ai.Providers;
using System.Diagnostics;
using Hermann.Di;

namespace Hermann.Ai
{
    /// <summary>
    /// AIプレイヤ
    /// </summary>
    public class AiPlayer : IInjectable<AiPlayer.Config>
    {
        /// <summary>
        /// AIプレイヤのバージョン
        /// </summary>
        public enum Version
        {
            /// <summary>
            /// V1.0
            /// </summary>
            V1_0,

            /// <summary>
            /// V2.0
            /// </summary>
            V2_0,

            /// <summary>
            /// V3.0
            /// </summary>
            V3_0,
        }

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
            /// バージョン
            /// </summary>
            /// <value>The version.</value>
            public Version Version { get; set; }

            /// <summary>
            /// 使用スライム
            /// </summary>
            public Slime[] UsingSlime { get; set; }
        }

        /// <summary>
        /// 移動方向キュー
        /// </summary>
        private Queue<Direction> DirectionQueue { get; set; }

        /// <summary>
        /// キューの補給が必要かどうか
        /// </summary>
        private bool RequiredEnqueue { get; set; }

        /// <summary>
        /// 探索ロジック
        /// </summary>
        private ThinCompleteReading SearchLogic { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AiPlayer()
        {
            this.DirectionQueue = new Queue<Direction>();
            this.RequiredEnqueue = true;
        }

        /// <summary>
        /// 依存関係がある機能を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Inject(Config config)
        {
            switch(config.Version)
            {
                case Version.V1_0:
                case Version.V2_0:
                    this.SearchLogic = DiProvider.GetContainer().GetInstance<ThinCompleteReading>();
                    break;
                default:
                    throw new ArgumentException("バージョンが不正です");
            }

            this.SearchLogic.Inject(new ThinCompleteReading.Config() {
                Version = config.Version,
                UsingSlime = config.UsingSlime,
            });
            this.HasInjected = true;
        }

        /// <summary>
        /// 移動する方向を考え、結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>移動方向</returns>
        public Direction Think(FieldContext context)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            var direction = Direction.None;
            var me = context.OperationPlayer;

            if (context.FieldEvent[me.ToInt()] == FieldEvent.None)
            {
                if (this.RequiredEnqueue)
                {
                    var directions = this.SearchLogic.Search(context);
                    foreach (var d in directions)
                    {
                        this.DirectionQueue.Enqueue(d);
                    }
                    this.RequiredEnqueue = false;
                }

                if (this.DirectionQueue.Count() > 0)
                {
                    // スライムを動かす
                    direction = DirectionQueue.Dequeue();
                }
                else
                {
                    // 移動が終わったのでひたすら下移動
                    direction = Direction.Down;

                }
            }
            else
            {
                // キューの破棄
                this.DirectionQueue.Clear();
                this.RequiredEnqueue = true;
            }

            return direction;
        }
    }
}
