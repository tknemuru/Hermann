using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Collections;

namespace Hermann.Tests.Collections
{
    /// <summary>
    /// プレイヤのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class PlayerTest
    {
        /// <summary>
        /// 001:Moveの基本的なテスト
        /// </summary>
        [TestMethod]
        public void Moveの基本的なテスト()
        {
            var container = TestDiProvider.GetContainer();
            var receiver = container.GetInstance<SimpleTextFileReceiver>();
            var sender = container.GetInstance<SimpleTextSender>();

            var context = receiver.Receive("../../resources/collections/player/test-field-in-001-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = receiver.Receive("../../resources/collections/player/test-field-out-001-001.txt");

            CollectionAssert.AreEqual(expectedContext, actualContext);
        }
    }
}
