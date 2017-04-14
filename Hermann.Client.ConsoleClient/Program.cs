using Hermann.Contexts;
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
        private const int FrameInterval = 512;

        /// <summary>
        /// フィールド状態
        /// </summary>
        private static FieldContext Context;

        /// <summary>
        /// コマンドの受信機能
        /// </summary>
        private static CommandReceiver<NativeCommand, FieldContext> Receiver;

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
            Receiver = ConsoleClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
            var command = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            Context = Receiver.Receive(command);
        }

        /// <summary>
        /// フレーム毎に実行される更新処理を行います。
        /// </summary>
        private static void Update()
        {
            var command = ConsoleClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Move;
            command.Context = Context;
            Context = Receiver.Receive(command);
            FieldContextWriter.Write(Context);
        }
    }
}
