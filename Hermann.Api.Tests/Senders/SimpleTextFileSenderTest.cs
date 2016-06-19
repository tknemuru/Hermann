using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Api.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Helpers;

namespace Hermann.Api.Tests.Senders
{
    /// <summary>
    /// SimpleTextFileSenderのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class SimpleTextFileSenderTest
    {
        /// <summary>
        /// 001:基本の動作をテストします。
        /// </summary>
        [TestMethod]
        public void Sender基本パターン()
        {
            var container = TestDiProvider.GetContainer();
            var receiver = container.GetInstance<SimpleTextFileReceiver>();
            var sender = container.GetInstance<SimpleTextSender>();

            // [001]1P:右
            var contextIn = receiver.Receive("../../resources/senders/simple-text-file-sender/test-field-in-001-001.txt");
            var contextOut = sender.Send(contextIn);

            // expected
            var expected = FileHelper.ReadTextLines("../../resources/senders/simple-text-file-sender/test-field-out-001-001.txt");
            var sb = new StringBuilder();
            foreach (var line in expected)
            {
                sb.AppendLine(line);
            }

            Assert.AreEqual(sb.ToString(), contextOut);
        }
    }
}
