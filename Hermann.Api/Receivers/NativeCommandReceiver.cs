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
using Hermann.Generators;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// ネイティブコマンドを受信する機能を提供します。
    /// </summary>
    public class NativeCommandReceiver : CommandReceiver<NativeCommand, FieldContext>
    {
        /// <summary>
        /// 使用スライムの生成機能
        /// </summary>
        private UsingSlimeGenerator UsingSlimeGenerator { get; set; }

        /// <summary>
        /// ゲームロジック
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NativeCommandReceiver()
        {
            this.UsingSlimeGenerator = DiProvider.GetContainer().GetInstance<UsingSlimeRandomGenerator>();
            this.Game = DiProvider.GetContainer().GetInstance<Game>();
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
                    this.Game.Inject(this.UsingSlimeGenerator.GetNext());
                    context = this.Game.Start();
                    break;
                case Command.Move:
                    context = this.Game.Update(context);
                    break;
                default:
                    throw new ArgumentException("コマンドが不正です。Command：" + command.Command);
            }

            return context;
        }
    }
}
