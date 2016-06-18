using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using Hermann.Tests.Di;
using Hermann.Receivers;

namespace Hermann.Tests.Receivers
{
    /// <summary>
    /// TextReceiverテストクラス
    /// </summary>
    [TestClass]
    public class TextFileReceiverTest
    {
        /// <summary>
        /// 基本の動作をテストします。
        /// </summary>
        [TestMethod]
        public void 基本パターン001()
        {
            var container = TestDiProvider.GetContainer();
            var receiver = container.GetInstance<TextFileReceiver>();

            // [001]1P:右
            var context = receiver.Receive("../../resources/receivers/text-file-receiver/test-field-in-0001.txt");
            Assert.AreEqual(0x5ul, context[0]);
            Assert.AreEqual(0x00000006ul, context[1]);
            Assert.AreEqual(0x00000000ul, context[2]);
        }
    }
}
