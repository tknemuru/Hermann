using Assets.Scripts.Di;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Initializers;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Initializers
{
    /// <summary>
    /// フィールド状態をシンプルテキストを用いて初期化します。
    /// </summary>
    public class FieldContextSimpleTextInitializer
    {
        /// <summary>
        /// シンプルテキストのファイルパス
        /// </summary>
        private static readonly string SimpleTextFilePath = Directory.GetCurrentDirectory() + "/Assets/Scripts/Initializers/resources/field-context.txt";

        /// <summary>
        /// 受信機能
        /// </summary>
        private FieldContextReceiver<string> Receiver { get; set; }

        public FieldContextSimpleTextInitializer()
        {
            this.Receiver = NativeClientDiProvider.GetContainer().GetInstance<FieldContextReceiver<string>>();
        }

        /// <summary>
        ///フィールド状態の初期化を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        public FieldContext Initialize(FieldContext context)
        {
            context = this.Receiver.Receive(SimpleTextFilePath);
            return context;
        }

        /// <summary>
        /// 依存する機能を注入します。
        /// </summary>
        /// <param name="nextSlimeGen">Nextスライム生成機能</param>
        /// <param name="movableUp">移動可能スライム更新機能</param>
        /// <param name="nextSlimeUp">NEXTスライム更新機能</param>
        public void Injection(NextSlimeGenerator nextSlimeGen, MovableSlimesUpdater movableUp, NextSlimeUpdater nextSlimeUp)
        {
        }
    }
}
