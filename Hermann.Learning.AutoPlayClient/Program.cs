using Hermann.Analyzers;
using Hermann.Api.Commands;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Learning.AutoPlayClient.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hermann.Learning.AutoPlayClient
{
    class Program
    {
        /// <summary>
        /// プレイ上限回数
        /// </summary>
        private const int LimitPlayCount = 20;

        /// <summary>
        /// スライムを動かす頻度
        /// </summary>
        private const int MoveFrameRate = 8;

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
        static Program()
        {
            MovableDirectionAnalyzer = AutoPlayClientDiProvider.GetContainer().GetInstance<MovableDirectionAnalyzer>();
            Receiver = AutoPlayClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
            Sender = AutoPlayClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
            RandomGen = new Random();
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var count = 0;
            while(count < LimitPlayCount)
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
                    WirteLog(ex.ToString());
                    WirteLog(Sender.Send(context));
                    break;
                }
            }
            Console.Write("Press any key to quit ..");
            Console.ReadKey();
        }

        private static FieldContext StartGame()
        {
            LastWinCount = new[] { 0, 0 };
            var command = AutoPlayClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            return Receiver.Receive(command);
        }

        private static void PlayGame(FieldContext context)
        {
            var frameCount = 0;
            while (!IsEnd(context))
            {
                Move(context, frameCount);
                frameCount++;
            }
        }

        private static void Move(FieldContext context, int frameCount)
        {
            var player = context.OperationPlayer;
            //var requiredMove = (frameCount % MoveFrameRate == 0) &&
            //    (!context.Ground[(int)player] || (context.Ground[(int)player] && frameCount % GroundMoveFrameRate == 0));
            //var requiredMove = (!context.Ground[(int)player] || (context.Ground[(int)player] && frameCount % GroundMoveFrameRate == 0));
            var requiredMove = !context.Ground[(int)player];
            //var requiredMove = (frameCount % MoveFrameRate == 0);

            if (requiredMove)
            {
                // スライムを動かす
                context.OperationDirection = GetNext(context);
                WirteLog(Sender.Send(context));
                //context.OperationPlayer = context.OperationPlayer.GetOppositeIndex();
            } else
            {
                // 移動方向無
                context.OperationDirection = Direction.None;
            }

            var c = AutoPlayClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            c.Command = Command.Move;
            c.Context = context;
            context = Receiver.Receive(c);
            LogWriter.WriteState(context);
            context.OperationPlayer = context.OperationPlayer.GetOppositeIndex();
        }

        private static Direction GetNext(FieldContext context)
        {
            var directions = MovableDirectionAnalyzer.Analyze(context, context.OperationPlayer).ToArray();
            if(directions.Count() <= 0)
            {
                return Direction.None;
            }

            var index = RandomGen.Next(directions.Count());
            return directions[index];
        }

        /// <summary>
        /// 勝負の結果が出たかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>勝負の結果が出たかどうか</returns>
        private static bool IsEnd(FieldContext context)
        {
            var firstWin = LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First];
            var secondWin = LastWinCount[(int)Player.Index.Second] != context.WinCount[(int)Player.Index.Second];
            return firstWin || secondWin;
        }

        private static Player.Index GetWinPlayer(FieldContext context)
        {
            var firstWin = LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First];
            return firstWin ? Player.Index.First : Player.Index.Second;
        }

        private static void WriteResult(FieldContext context, int count)
        {
            var win = GetWinPlayer(context);
            LogWriter.WriteWinResult(win);

            WirteLog(Sender.Send(context));
            WirteLog($"count:{count}");
            WirteLog($"win:{win}");
        }

        private static void WirteLog(string log)
        {
            Console.WriteLine(log);
            FileHelper.WriteLine(log);
        }
    }
}
