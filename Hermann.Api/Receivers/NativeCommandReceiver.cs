using Hermann.Contexts;
using Hermann.Models;
using Hermann.Api.Commands;
using Hermann.Di;
using Hermann.Api.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Ai;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// ネイティブコマンドを受信する機能を提供します。
    /// </summary>
    public class NativeCommandReceiver : CommandReceiver<NativeCommand, FieldContext>
    {
        /// <summary>
        /// ゲームロジック
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// AIプレイヤ
        /// </summary>
        private AiPlayer AiPlayer { get; set; }

        /// <summary>
        /// AIの移動方向キュー
        /// </summary>
        private Queue<Direction>[] AiDirectionQueue { get; set; }

        /// <summary>
        /// キューの補給が必要かどうか
        /// </summary>
        private bool[] RequiredEnqueue { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NativeCommandReceiver()
        {
            this.Game = DiProvider.GetContainer().GetInstance<Game>();
            this.AiPlayer = DiProvider.GetContainer().GetInstance<AiPlayer>();
            this.AiPlayer.Injection(this.Game);
            this.AiDirectionQueue = new Queue<Direction>[Player.Length];
            this.RequiredEnqueue = new bool[Player.Length];
            Player.ForEach(player =>
            {
                this.AiDirectionQueue[(int)player] = new Queue<Direction>();
                this.RequiredEnqueue[(int)player] = true;
            });

        }

        /// <summary>
        /// コマンドを受信しフィールド状態の文字列を返却します。
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <returns>フィールド状態の文字列</returns>
        public override FieldContext Receive(NativeCommand command)
        {
            var context = command.Context;

            switch (command.Command)
            {
                case Command.Start:
                    context = this.Game.Start();
                    break;
                case Command.Move:
                    context = this.Game.Update(context);
                    break;
                case Command.AiMove:
                    var player = (int)context.OperationPlayer;

                    if(context.FieldEvent[player] == FieldEvent.None)
                    {
                        if (this.RequiredEnqueue[player])
                        {
                            var directions = AiPlayer.Think(context);
                            foreach (var d in directions)
                            {
                                AiDirectionQueue[player].Enqueue(d);
                            }
                            this.RequiredEnqueue[player] = false;
                        }

                        if(this.AiDirectionQueue[player].Count() > 0)
                        {
                            // スライムを動かす
                            context.OperationDirection = AiDirectionQueue[player].Dequeue();
                        } else
                        {
                            // 移動が終わったのでひたすら下移動
                            context.OperationDirection = Direction.Down;

                        }
                    } else
                    {
                        // キューの破棄
                        AiDirectionQueue[player].Clear();
                        this.RequiredEnqueue[player] = true;
                    }
                    context = this.Game.Update(context);
                    break;
                default:
                    throw new ArgumentException("コマンドが不正です。Command：" + command.Command);
            }

            return context;
        }
    }
}
