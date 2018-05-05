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
using Hermann.Ai;
using Hermann.Ai.Analyzers;

namespace Hermann.Client.LearningClient.Excecuters
{
    /// <summary>
    /// 自動対戦機能を提供します。
    /// </summary>
    public class AutoPlayExecuter : IExecutable
    {
        /// <summary>
        /// 勝ち数
        /// </summary>
        private int[] WinCount { get; set; }

        /// <summary>
        /// コマンドの受信機能
        /// </summary>
        private CommandReceiver<NativeCommand, FieldContext> Receiver { get; set; }

        /// <summary>
        /// フィールドの送信機能
        /// </summary>
        private FieldContextSender<string> Sender { get; set; }

        /// <summary>
        /// 自動対戦管理機能
        /// </summary>
        private AutoPlayManager Manager { get; set; }

        /// <summary>
        /// 削除スライム分析機能パラメータ
        /// </summary>
        private ErasedSlimeAnalyzer.Param[] ErasedSlimeAnalyzerParams { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoPlayExecuter()
        {
            this.Receiver = LearningClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
            this.Sender = LearningClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
            this.Manager = LearningClientDiProvider.GetContainer().GetInstance<AutoPlayManager>();
            this.WinCount = new[] { 0, 0 };
        }

        /// <summary>
        /// 自動対戦を実行します。
        /// </summary>
        /// <param name="args">パラメータ</param>
        public void Execute(string[] args)
        {
            var count = 1;
            while (count < AutoPlayManager.LimitPlayCount)
            {
                try
                {
                    var context = StartGame();
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
            var command = LearningClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            var context = Receiver.Receive(command);

            this.Manager.Inject(new AutoPlayManager.Config()
            {
                Versions = new AiPlayer.Version?[]
                {
                    null,
                    AiPlayer.Version.V1_0,
                },
                UsingSlime = context.UsingSlimes,
                LoggingVersion = AiPlayer.Version.V1_0,
            });

            this.ErasedSlimeAnalyzerParams = new[]
            {
                LearningClientDiProvider.GetContainer().GetInstance<ErasedSlimeAnalyzer.Param>(),
                LearningClientDiProvider.GetContainer().GetInstance<ErasedSlimeAnalyzer.Param>(),
            };

            return context;
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

            // 削除スライム分析機能パラメータの更新
            if (context.FieldEvent[player.ToInt()] == FieldEvent.MarkErasing &&
               context.Chain[player.ToInt()] == 0)
            {
                this.ErasedSlimeAnalyzerParams[player.ToInt()].TargetContext = context.DeepCopy();
            }
            else if (context.FieldEvent[player.ToInt()] == FieldEvent.Erase)
            {
                this.ErasedSlimeAnalyzerParams[player.ToInt()].ErasedSlimes =
                FieldContextHelper.MergeSlimeFields(this.ErasedSlimeAnalyzerParams[player.ToInt()].TargetContext.SlimeFields[player.ToInt()][Slime.Erased],
                                                    context.SlimeFields[player.ToInt()][Slime.Erased]);
            }

            // 画面表示
            if (Manager.RequiredDisplay(context, frameCount[player.ToInt()]))
            {
                LogWriter.WirteLog(Sender.Send(context));
            }

            // コマンドの生成
            var c = LearningClientDiProvider.GetContainer().GetInstance<NativeCommand>();

            // スライムを動かすかどうかの判定
            if (Manager.RequiredMove(context, frameCount[player.ToInt()]))
            {
                // スライムを動かす
                context.OperationDirection = Manager.GetNext(context);
            }

            c.Command = Command.Move;
            c.Context = context;
            context = Receiver.Receive(c);

            // 状態の書き込み
            if (Manager.RequiredWriteStateLog(this.ErasedSlimeAnalyzerParams[player.ToInt()].TargetContext, context))
            {
                FileHelper.WriteLine("----- Write state log -----");
                var input = Manager.GetStateLogInput(this.ErasedSlimeAnalyzerParams[player.ToInt()], context);
                LogWriter.WriteState(input);
            }

            // 結果の書き込み
            if (Manager.RequiredWriteResultLog(this.ErasedSlimeAnalyzerParams[player.ToInt()].TargetContext, context))
            {
                var input = Manager.GetResutlLogInput(this.ErasedSlimeAnalyzerParams[player.ToInt()].TargetContext, context);
                this.WriteResult(input, this.ErasedSlimeAnalyzerParams[player.ToInt()].TargetContext, context, gameCount);
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

            if (win != null)
            {
                this.WinCount[win.Value.ToInt()]++;
                Player.ForEach(player =>
                {
                    var winRate = Math.Floor(((double)this.WinCount[player.ToInt()] / (double)count) * 100.0d);
                    LogWriter.WirteLog($"{player.GetName()} win count:{this.WinCount[player.ToInt()]} win rate:{winRate}%");
                });
                LogWriter.WirteLog($"count:{count}");
                LogWriter.WirteLog($"win:{win}");

            }
        }
     }
}
