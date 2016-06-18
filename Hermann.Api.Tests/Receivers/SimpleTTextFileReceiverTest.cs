using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using Hermann.Api.Tests.Di;
using Hermann.Api.Receivers;

namespace Hermann.Api.Tests.Receivers
{
    /// <summary>
    /// TextReceiverテストクラス
    /// </summary>
    [TestClass]
    public class SimpleTTextFileReceiverTest
    {
        /// <summary>
        /// 001:基本の動作をテストします。
        /// </summary>
        [TestMethod]
        public void Receive基本パターン()
        {
            var container = TestDiProvider.GetContainer();
            var receiver = container.GetInstance<SimpleTextFileReceiver>();

            // [001]1P:右
            var context = receiver.Receive("../../resources/receivers/simple-text-file-receiver/test-field-in-001-001.txt");
            Assert.AreEqual(0x5ul, context[0]);
            Assert.AreEqual(0x00000006ul, context[1]);
            Assert.AreEqual(0x00000000ul, context[2]);
        }
    }
}
