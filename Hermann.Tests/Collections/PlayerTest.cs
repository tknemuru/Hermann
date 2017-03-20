using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Contexts;
using Hermann.Collections;
using Hermann.Tests.TestHelpers;
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
        /// 001:Moveで1つ目のフィールド単位において右に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において右に移動できる()
        {
            // 001:異なる色
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-001-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-001-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);

            // 002:同じ色
            context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-001-002.txt");
            actualContext = Player.Move(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-001-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 002:Moveで1つ目のフィールド単位において最右にいる場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において最右にいる場合は移動しない()
        {
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-002-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-002-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 003:Moveで1つ目のフィールド単位において左に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において左に移動できる()
        {
            // 001:異なる色
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-003-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-003-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);

            // 002:同じ色
            context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-003-002.txt");
            actualContext = Player.Move(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-003-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 004:Moveで1つ目のフィールド単位において最左にいる場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において最左にいる場合は移動しない()
        {
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-004-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-004-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 005:Moveで1つ目のフィールド単位において下に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において下に移動できる()
        {
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-005-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-005-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 006:Moveで底辺を超える場合は底辺に着地する
        /// </summary>
        [TestMethod]
        public void Moveで底辺を超える場合は底辺に着地する()
        {
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-006-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-006-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }
    }
}
