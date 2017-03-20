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

        /// <summary>
        /// 007:Moveで1つ目のフィールド単位において右移動時に移動場所に他のスライムが存在する場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において右移動時に移動場所に他のスライムが存在する場合は移動しない()
        {
            // 001:横並び
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-007-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-007-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);

            // 002:縦並び
            context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-007-002.txt");
            actualContext = Player.Move(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-007-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 008:Moveで1つ目のフィールド単位において左移動時に移動場所に他のスライムが存在する場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において左移動時に移動場所に他のスライムが存在する場合は移動しない()
        {
            // 001:横並び
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-008-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-008-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);

            // 002:縦並び
            context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-008-002.txt");
            actualContext = Player.Move(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-008-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }

        /// <summary>
        /// 009:Moveで下移動時に移動場所に他のスライムが存在する場合はそれ以上下に移動しない
        /// </summary>
        [TestMethod]
        public void Moveで下移動時に移動場所に他のスライムが存在する場合はそれ以上下に移動しない()
        {
            // 001:横並び（2つめが他スライムとバッティングする）
            var context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-009-001.txt");
            var actualContext = Player.Move(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-009-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);

            // 002:縦並び
            context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-009-002.txt");
            actualContext = Player.Move(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-009-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);

            // 003:横並び（1つめが他スライムとバッティングする）
            context = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-in-009-003.txt");
            actualContext = Player.Move(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/collections/player/test-field-out-009-003.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, actualContext);
        }
    }
}
