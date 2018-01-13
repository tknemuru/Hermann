using Hermann.Contexts;
using Hermann.Models;
using Hermann.Api.Commands;
using Hermann.Client.ConsoleClient.Writers;
using Hermann.Api.Receivers;
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
            // 初期フィールド状態の取得
            NoneDirectionUpdateFrameCount = 0;
            Receiver = ConsoleClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
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
            // 移動方向無コマンドの実行
            Context.OperationDirection = Direction.None;
            var c = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            c.Command = Command.Move;
            c.Context = Context;
            Context = Receiver.Receive(c);

            // 入力を受け付けたコマンドの実行
            if (HasSetKeyInfo.Value)
            {
                HasSetKeyInfo.TurnOff();
                Context.OperationPlayer = KeyMap.GetPlayer(KeyInfo.Value.Key);
                Context.OperationDirection = KeyMap.GetDirection(KeyInfo.Value.Key);
                c = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
                c.Command = Command.Move;
                c.Context = Context;
                Context = Receiver.Receive(c);
            }

            // 画面描画
            FieldContextWriter.Write(Context);
        }
    }
}
