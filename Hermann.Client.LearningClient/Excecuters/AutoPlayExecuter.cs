using System;
using System.Threading;
using System.Linq;
using Hermann.Ai.Helpers;
using Hermann.Api.Senders;
using Hermann.Api.Commands;
using Hermann.Api.Receivers;
using Hermann.Client.LearningClient.Di;
using Hermann.Contexts;
using Hermann.Models;
using Hermann.Client.LearningClient.Managers;
using Hermann.Helpers;

namespace Hermann.Client.LearningClient.Excecuters
{
    /// <summary>
    /// 自動対戦機能を提供します。
    /// </summary>
    public class AutoPlayExecuter : IExecutable
    {
        /// <summary>
        /// 前回の勝ち数
        /// </summary>
        private int[] LastWinCount { get; set; }

        /// <summary>
        /// 前回のフィールド状態
        /// </summary>
        private FieldContext[] LastContext { get; set; }

        /// <summary>
        /// コマンドの受信機能
        /// </summary>
        private CommandReceiver<NativeCommand, FieldContext> Receiver { get; set; }

        /// <summary>
        /// フィールドの送信機能
        /// </summary>
        private static FieldContextSender<string> Sender { get; set; }

        /// <summary>
        /// 自動対戦管理機能
        /// </summary>
        private static AutoPlayManager Manager { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoPlayExecuter()
        {
            Receiver = LearningClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
            Sender = LearningClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
            Manager = LearningClientDiProvider.GetContainer().GetInstance<AutoPlayManager>();
        }

        /// <summary>
        /// 自動対戦を実行します。
        /// </summary>
        /// <param name="args">パラメータ</param>
        public void Execute(string[] args)
        {
            var count = 0;
            while (count < AutoPlayManager.LimitPlayCount)
            {
                var context = StartGame();
                try
                {
                    PlayGame(context, count);
                    Thread.Sleep(AutoPlayManager.ResultDisplayMillSec);
                    count++;
                }
                catch (Exception ex)
                {
                    LogWriter.WirteLog(ex.ToString());
                    break;
                }
            }
            Console.Write("Press any key to quit ..");
            Console.ReadKey();
        }

        /// <summary>
        /// ゲームを開始します。
        /// </summary>
        /// <returns>フィールド初期状態</returns>
        private FieldContext StartGame()
        {
            this.LastWinCount = new[] { 0, 0 };
            this.LastContext = new FieldContext[Player.Length];
            var command = LearningClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            return Receiver.Receive(command);
        }

        /// <summary>
        /// ゲームを実施します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="gameCount">ゲームカウント</param>
        private void PlayGame(FieldContext context, int gameCount)
        {
            var frameCount = new int[] { 0, 0 };
            while (context.FieldEvent[(int)context.OperationPlayer] != FieldEvent.End)
            {
                Move(context, frameCount, gameCount);
                frameCount[(int)context.OperationPlayer]++;
            }
        }

        /// <summary>
        /// スライムを動かします。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="frameCount">フレームカウント</param>
        /// <param name="gameCount">ゲームカウント</param>
        private void Move(FieldContext context, int[] frameCount, int gameCount)
        {
            var player = context.OperationPlayer;

            // 前回フィールド状態の更新
            this.LastContext[player.ToInt()] = context.DeepCopy();

            // 画面表示
            if (Manager.RequiredDisplay(context, frameCount[player.ToInt()]))
            {
                LogWriter.WirteLog(Sender.Send(context));
            }

            // コマンドの生成
            var c = LearningClientDiProvider.GetContainer().GetInstance<NativeCommand>();

            // スライムを動かすかどうかの判定
            if (Manager.RequiredMove(frameCount[player.ToInt()]))
            {
                // スライムを動かす
                context.OperationDirection = Manager.GetNext(context);
            }
            else
            {
                // 移動方向無
                context.OperationDirection = Direction.None;
            }
            c.Command = Command.Move;
            c.Context = context;
            context = Receiver.Receive(c);

            // 状態の書き込み
            if (Manager.RequiredWriteStateLog(this.LastContext[player.ToInt()], context))
            {
                var input = Manager.GetStateLogInput(context);
                LogWriter.WriteState(input);
            }

            // 結果の書き込み
            if (Manager.RequiredWriteResultLog(this.LastContext[player.ToInt()], context))
            {
                var input = Manager.GetResutlLogInput(this.LastContext[context.OperationPlayer.ToInt()], context);
                this.WriteResult(input, this.LastContext[player.ToInt()], context, gameCount);
            }

            // プレイヤを交換
            context.OperationPlayer = context.OperationPlayer.GetOppositeIndex();
        }

        /// <summary>
        /// 結果を書き込みます。
        /// </summary>
        /// <param name="score">評価値</param>
        /// <param name="context">フィールド状態</param>
        /// <param name="count">プレイ回数</param>
        private void WriteResult(double score, FieldContext lastContext, FieldContext context, int count)
        {
            var win = FieldContextHelper.GetWinPlayer(lastContext, context);
            LogWriter.WriteWinResult(score);
            LogWriter.WirteLog($"count:{count}");
            LogWriter.WirteLog($"win:{win}");
        }
     }
}
