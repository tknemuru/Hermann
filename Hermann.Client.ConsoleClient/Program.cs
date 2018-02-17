using Hermann.Contexts;
using Hermann.Models;
using Hermann.Api.Commands;
using Hermann.Client.ConsoleClient.Writers;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Client.ConsoleClient.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;
using Hermann.Updaters;
using Hermann.Helpers;

namespace Hermann.Client.ConsoleClient
{
    /// <summary>
    /// プログラム
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 1フレーム毎の間隔（ミリ秒）
        /// </summary>
        private const int FrameInterval = 256;

        /// <summary>
        /// 方向：無で更新するフレーム最大回数
        /// </summary>
        private const int NoneDirectionUpdateFrameMaxCount = 4;

        /// <summary>
        /// フィールド状態
        /// </summary>
        private static FieldContext Context;

        /// <summary>
        /// コマンドの受信機能
        /// </summary>
        private static CommandReceiver<NativeCommand, FieldContext> Receiver;

        /// <summary>
        /// フィールドの送信機能
        /// </summary>
        private static FieldContextSender<string> Sender;

        /// <summary>
        /// 方向：無で更新するフレーム回数
        /// </summary>
        private static int NoneDirectionUpdateFrameCount { get; set; }

        /// <summary>
        /// 入力されたコンソールキー情報
        /// </summary>
        private static ReactiveProperty<ConsoleKeyInfo> KeyInfo { get; set; }

        /// <summary>
        /// コンソールキー情報がセットされているかどうか
        /// </summary>
        private static BooleanNotifier HasSetKeyInfo { get; set; }

        /// <summary>
        /// 無移動のプレイヤ
        /// </summary>
        private static Player.Index NoneMovePlayer { get; set; }

        /// <summary>
        /// 前回の勝ち数
        /// </summary>
        private static int[] LastWinCount { get; set; }

        /// <summary>
        /// Mainメソッド
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // 初期処理
            Start();

            // 擬似的なフレーム処理
            while (true)
            {
                Thread.Sleep(FrameInterval);

                // 更新処理
                Update();
            }
        }

        /// <summary>
        /// 初期処理を行います。
        /// </summary>
        private static void Start()
        {
            NoneMovePlayer = Player.Index.First;
            LastWinCount = new[] { 0, 0 };

            // 初期フィールド状態の取得
            NoneDirectionUpdateFrameCount = 0;
            Receiver = ConsoleClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
            Sender = ConsoleClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
            var command = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            Context = Receiver.Receive(command);

            // キー変更イベントの購読
            KeyInfo = new ReactiveProperty<ConsoleKeyInfo>();
            HasSetKeyInfo = new BooleanNotifier(false);

            // キーの入力読み込みタスクを開始
            Task.Run(() =>
            {
                while (true)
                {
                    KeyInfo.Value = Console.ReadKey();
                    if (KeyMap.ContainsKey(KeyInfo.Value.Key))
                    {
                        HasSetKeyInfo.TurnOn();
                    }
                }
            });
        }

        /// <summary>
        /// フレーム毎に実行される更新処理を行います。
        /// </summary>
        private static void Update()
        {
            if (IsEnd(Context))
            {
                Console.WriteLine(string.Format("{0}Pの勝ちです。", (int)GetWinPlayer(Context) + 1));
                return;
            }

            // 移動方向無コマンドの実行
            Context.OperationDirection = Direction.None;
            var c = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            c.Command = Command.Move;
            c.Context = Context.DeepCopy();
            c.Context.OperationPlayer = NoneMovePlayer;
            NoneMovePlayer = Player.GetOppositeIndex(NoneMovePlayer);
            FileHelper.WriteLine("----- 移動方向無コマンドの実行 -----");
            FileHelper.WriteLine(Sender.Send(Context));
            Context = Receiver.Receive(c);

            // 入力を受け付けたコマンドの実行
            if (HasSetKeyInfo.Value)
            {
                HasSetKeyInfo.TurnOff();
                Context.OperationPlayer = KeyMap.GetPlayer(KeyInfo.Value.Key);
                Context.OperationDirection = KeyMap.GetDirection(KeyInfo.Value.Key);
                c = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
                c.Command = Command.Move;
                c.Context = Context.DeepCopy();
                FileHelper.WriteLine("----- 入力を受け付けたコマンドの実行 -----");
                FileHelper.WriteLine(Sender.Send(Context));
                Context = Receiver.Receive(c);
            }

            // 画面描画
            FieldContextWriter.Write(Context);
        }

        /// <summary>
        /// 勝負の結果が出たかどうかを判定します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>勝負の結果が出たかどうか</returns>
        private static bool IsEnd(FieldContext context)
        {
            return (LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First] ||
                LastWinCount[(int)Player.Index.Second] != context.WinCount[(int)Player.Index.Second]);
        }

        /// <summary>
        /// TODO:勝ったプレイヤを取得します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>勝ったプレイヤ</returns>
        private static Player.Index GetWinPlayer(FieldContext context)
        {
            if (LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First])
            {
                return Player.Index.First;
            }
            else
            {
                return Player.Index.Second;
            }
        }
    }
}
