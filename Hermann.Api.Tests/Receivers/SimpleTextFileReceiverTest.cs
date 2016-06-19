using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using Hermann.Api.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Collections;

namespace Hermann.Api.Tests.Receivers
{
    /// <summary>
    /// TextReceiverテストクラス
    /// </summary>
    [TestClass]
    public class SimpleTextFileReceiverTest
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
            Assert.AreEqual(Convert.ToUInt64("1000", 2), context[FieldContext.IndexCommand]);
            Assert.AreEqual(Convert.ToUInt64("01100000", 2) << 8, context[FieldContext.IndexOccupiedFieldUpper]);
            Assert.AreEqual(0ul, context[FieldContext.IndexOccupiedFieldLower]);
            Assert.AreEqual(Convert.ToUInt64("01100000", 2) << 8, context[FieldContext.IndexMovableFieldUpper]);
            Assert.AreEqual(0ul, context[FieldContext.IndexMovableFieldLower]);
        }
    }
}
