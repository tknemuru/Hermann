using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Linq;
using Hermann.Ai.Calculators;
using Hermann.Ai.Helpers;
using Hermann.Ai.Providers;
using Hermann.Api.Senders;
using Hermann.Api.Commands;
using Hermann.Api.Receivers;
using Hermann.Client.LearningClient.Di;
using Hermann.Contexts;
using Hermann.Models;
using Hermann.Analyzers;

namespace Hermann.Client.LearningClient.Excecuters
{
    /// <summary>
    /// 自動対戦機能を提供します。
    /// </summary>
    public class AutoPlayExecuter : IExecutable
    {
        /// <summary>
        /// プレイ上限回数
        /// </summary>
        private const int LimitPlayCount = 10000;

        /// <summary>
        /// スライムを動かす頻度
        /// </summary>
        private const int MoveFrameRate = 6;

        /// <summary>
        /// 接地時に動かす頻度
        /// </summary>
        private const int GroundMoveFrameRate = 128;

        /// <summary>
        /// 前回の勝ち数
        /// </summary>
        private static int[] LastWinCount { get; set; }

        /// <summary>
        /// コマンドの受信機能
        /// </summary>
        private static CommandReceiver<NativeCommand, FieldContext> Receiver;

        /// <summary>
        /// フィールドの送信機能
        /// </summary>
        private static FieldContextSender<string> Sender;

        /// <summary>
        /// 移動可能方向の分析機能
        /// </summary>
        private static MovableDirectionAnalyzer MovableDirectionAnalyzer;

        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private static Random RandomGen;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static AutoPlayExecuter()
        {
            MovableDirectionAnalyzer = LearningClientDiProvider.GetContainer().GetInstance<MovableDirectionAnalyzer>();
            Receiver = LearningClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
            Sender = LearningClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
            RandomGen = new Random();
        }

        /// <summary>
        /// 自動対戦を実行します。
        /// </summary>
        /// <param name="args">パラメータ</param>
        public void Execute(string[] args)
        {
            var count = 0;
            while (count < LimitPlayCount)
            {
                var context = StartGame();
                try
                {
                    PlayGame(context);
                    WriteResult(context, count);
                    Thread.Sleep(1000);
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

        private static FieldContext StartGame()
        {
            LastWinCount = new[] { 0, 0 };
            var command = LearningClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            return Receiver.Receive(command);
        }

        private static void PlayGame(FieldContext context)
        {
            var frameCount = new int[] { 0, 0 };
            while (context.FieldEvent[(int)context.OperationPlayer] != FieldEvent.End)
            {
                Move(context, frameCount);
                frameCount[(int)context.OperationPlayer]++;
            }
        }

        private static void Move(FieldContext context, int[] frameCount)
        {
            var player = context.OperationPlayer;
            var requiredMove = true;

            var c = LearningClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            if (requiredMove)
            {
                // スライムを動かす
                if (context.OperationPlayer == Player.Index.First && frameCount[(int)player] % 16 == 0)
                {
                    LogWriter.WirteLog(Sender.Send(context));
                }
                if (frameCount[(int)player] % 16 == 0 || frameCount[(int)player] % 15 == 0)
                {
                    context.OperationDirection = GetNext(context);
                }
                else
                {
                    requiredMove = false;
                }
                c.Command = Command.Move;
                //c.Command = Command.AiMove;
            }
            else
            {
                // 移動方向無
                context.OperationDirection = Direction.None;
                c.Command = Command.Move;
            }
            c.Context = context;
            context = Receiver.Receive(c);
            if (requiredMove &&
                (context.FieldEvent[(int)player] == FieldEvent.MarkErasing || context.FieldEvent[(int)player] == FieldEvent.NextPreparation))
            //if (requiredMove)
            {
                LogWriter.WriteState(LearningClientDiProvider.GetContainer().GetInstance<InputDataProvider>().
                    GetVector(InputDataProvider.Vector.Main, context).ToArray());
            }
            context.OperationPlayer = context.OperationPlayer.GetOppositeIndex();
        }

        private static Direction GetNext(FieldContext context)
        {
            var directions = MovableDirectionAnalyzer.Analyze(context, context.OperationPlayer).ToArray();
            if (directions.Count() <= 0)
            {
                return Direction.None;
            }

            var index = RandomGen.Next(directions.Count());
            return directions[index];
        }

        private static Player.Index GetWinPlayer(FieldContext context)
        {
            var firstWin = LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First];
            return firstWin ? Player.Index.First : Player.Index.Second;
        }

        private static void WriteResult(FieldContext context, int count)
        {
            LogWriter.WirteLog(Sender.Send(context));

            var win = GetWinPlayer(context);
            var score = LearningClientDiProvider.GetContainer().GetInstance<ResultEvaluationCalculator>().
                Evaluate(context, win);
            LogWriter.WriteWinResult(score);
            LogWriter.WirteLog($"count:{count}");
            LogWriter.WirteLog($"win:{win}");
        }
     }
}
