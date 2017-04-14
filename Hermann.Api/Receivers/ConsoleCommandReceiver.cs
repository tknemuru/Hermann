using Hermann.Contexts;
using Hermann.Collections;
using Hermann.Api.Commands;
using Hermann.Di;
using Hermann.Api.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// コンソールからHermannのネイティブコマンドを受信する機能を提供します。
    /// </summary>
    public class ConsoleCommandReceiver : CommandReceiver<NativeCommand, FieldContext>
    {
        /// <summary>
        /// ゲームロジック
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// フィールド状態の送信機能
        /// </summary>
        private FieldContextSender<string> Sender { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConsoleCommandReceiver()
        {
            this.Game = DiProvider.GetContainer().GetInstance<Game>();
            this.Sender = DiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
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
                    context = this.Start();
                    break;
                case Command.Move:
                    context = this.Move(context);
                    break;
                default:
                    throw new ArgumentException("コマンドが不正です。Command：" + command.Command);
            }

            return context;
        }

        /// <summary>
        /// ゲーム開始時の処理を行います。
        /// </summary>
        /// <returns>フィールド状態</returns>
        private FieldContext Start()
        {
            return this.Game.CreateInitialFieldContext();
        }

        /// <summary>
        /// スライムを動かす処理を行います。
        /// </summary>
        /// <returns>フィールド状態</returns>
        private FieldContext Move(FieldContext context)
        {
            return Player.Move(context);
        }
    }
}
