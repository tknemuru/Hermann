using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Collections;
using SimpleInjector;

namespace Hermann.Tests.Collections
{
    /// <summary>
    /// プレイヤのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class PlayerTest
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        private  Container MyContainer { get; set; }

        /// <summary>
        /// 受信機能
        /// </summary>
        private Receiver MyReceiver { get; set; }

        /// <summary>
        /// 送信機能
        /// </summary>
        private Sender MySender { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayerTest()
        {
            this.MyContainer = TestDiProvider.GetContainer();
            this.MyReceiver = this.MyContainer.GetInstance<SimpleTextFileReceiver>();
            //this.MySender = this.MyContainer.GetInstance<SimpleTextSender>();
        }

        /// <summary>
        /// 001:Moveで上部において右に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで上部において右に移動できる()
        {
            var context = this.MyReceiver.Receive("../../resources/collections/player/test-field-in-001-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = this.MyReceiver.Receive("../../resources/collections/player/test-field-out-001-001.txt");

            CollectionAssert.AreEqual(expectedContext, actualContext);
        }

        /// <summary>
        /// 002:Moveで上部において最右にいる場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで上部において最右にいる場合は移動しない()
        {
            var context = this.MyReceiver.Receive("../../resources/collections/player/test-field-in-002-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = this.MyReceiver.Receive("../../resources/collections/player/test-field-out-002-001.txt");

            CollectionAssert.AreEqual(expectedContext, actualContext);
        }
    }
}
