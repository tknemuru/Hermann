using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Contexts;
using Hermann.Models;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;
using SimpleInjector;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// スライム移動機能のテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class SlimeMoverTest
    {
        /// <summary>
        /// スライム移動機能
        /// </summary>
        private SlimeMover SlimeMover { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlimeMoverTest()
        {
            this.SlimeMover = new SlimeMover();
        }

        /// <summary>
        /// 001:Moveで1つ目のフィールド単位において右に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において右に移動できる()
        {
            var param = new SlimeMover.Param();

            // 001:異なる色
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-001-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-001-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:同じ色
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-001-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-001-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 002:Moveで1つ目のフィールド単位において最右にいる場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において最右にいる場合は移動しない()
        {
            var param = new SlimeMover.Param();

            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-002-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-002-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 003:Moveで1つ目のフィールド単位において左に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において左に移動できる()
        {
            var param = new SlimeMover.Param();

            // 001:異なる色
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-003-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-003-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:同じ色
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-003-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-003-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 004:Moveで1つ目のフィールド単位において最左にいる場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において最左にいる場合は移動しない()
        {
            var param = new SlimeMover.Param();

            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-004-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-004-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 005:Moveで1つ目のフィールド単位において下に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において下に移動できる()
        {
            var param = new SlimeMover.Param();

            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-005-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-005-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 006:Moveで底辺を超える場合は底辺に着地する
        /// </summary>
        [TestMethod]
        public void Moveで底辺を超える場合は底辺に着地する()
        {
            var param = new SlimeMover.Param();

            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-006-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-006-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 007:Moveで1つ目のフィールド単位において右移動時に移動場所に他のスライムが存在する場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において右移動時に移動場所に他のスライムが存在する場合は移動しない()
        {
            var param = new SlimeMover.Param();

            // 001:横並び
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-007-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-007-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:縦並び
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-007-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-007-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 008:Moveで1つ目のフィールド単位において左移動時に移動場所に他のスライムが存在する場合は移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において左移動時に移動場所に他のスライムが存在する場合は移動しない()
        {
            var param = new SlimeMover.Param();

            // 001:横並び
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-008-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-008-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:縦並び
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-008-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-008-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 009:Moveで下移動時に移動場所に他のスライムが存在する場合はそれ以上下に移動しない
        /// </summary>
        [TestMethod]
        public void Moveで下移動時に移動場所に他のスライムが存在する場合はそれ以上下に移動しない()
        {
            var param = new SlimeMover.Param();

            // 001:横並び（2つめが他スライムとバッティングする）
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-009-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-009-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:縦並び
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-009-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-009-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 003:横並び（1つめが他スライムとバッティングする）
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-009-003.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-009-003.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 004:横並び（2つめが他スライムとバッティングする）
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-009-004.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-009-004.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 010:Moveで1つ目のフィールド単位において何もしないで自動的に下に移動する
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において何もしないで自動的に下に移動する()
        {
            var param = new SlimeMover.Param();

            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-010-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-010-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 011:Moveで1つ目のフィールド単位において何もしないで移動場所に他スライムがいる場合はそれ以上下に移動しない
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において何もしないで移動場所に他スライムがいる場合はそれ以上下に移動しない()
        {
            var param = new SlimeMover.Param();

            // 001:縦向き
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-011-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-011-001.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:横並び（1つめが他スライムとバッティングする）
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-011-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-011-002.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 003:横並び（2つめが他スライムとバッティングする）
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-011-003.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-011-003.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 012:Moveで2つ目のフィールド単位において2Pの移動ができる
        /// </summary>
        [TestMethod]
        public void Moveで2つ目のフィールド単位において2Pの移動ができる()
        {
            var param = new SlimeMover.Param();

            // 001:右
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-012-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-012-001.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 013:Moveで1つ目のフィールド単位において上に移動できる
        /// </summary>
        [TestMethod]
        public void Moveで1つ目のフィールド単位において上に移動できる()
        {
            var param = new SlimeMover.Param();

            // 001:右回転
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-013-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-013-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:下回転
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-013-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-013-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 003:左回転
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-013-003.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-013-003.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 004:上回転
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-013-004.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-013-004.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 014:Moveでフィールド単位をまたいで上に移動できる
        /// </summary>
        [TestMethod]
        public void Moveでフィールド単位をまたいで上に移動できる()
        {
            var param = new SlimeMover.Param();

            // 001:右回転
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-014-001.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-014-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 002:下回転
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-014-002.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-014-002.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 003:左回転
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-014-003.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-014-003.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 004:上回転
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-in-014-004.txt");
            this.SlimeMover.Update(context, context.OperationPlayer, param);
            expectedContext = TestHelper.Receiver.Receive("../../resources/updaters/slimemover/test-field-out-014-004.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 015:Moveで壁・スライムに挟まれて回転できない
        /// </summary>
        [TestMethod]
        public void Moveで壁スライムに挟まれて回転できない()
        {
            // 001:右際で右（両移動スライム塞ぎ）
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-015-001.txt",
                "../../resources/updaters/slimemover/test-field-in-015-001.txt",
                this.SlimeMover.Update,
                new SlimeMover.Param());

            // 002:右際で右（片移動スライム塞ぎ）
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-015-002.txt",
                "../../resources/updaters/slimemover/test-field-in-015-002.txt",
                this.SlimeMover.Update,
                new SlimeMover.Param());

            // 003:右際で右（片移動スライム塞ぎ逆パターン）
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-015-003.txt",
                "../../resources/updaters/slimemover/test-field-in-015-003.txt",
                this.SlimeMover.Update,
                new SlimeMover.Param());

            // 004:左際で左（両移動スライム塞ぎ）
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-015-004.txt",
                "../../resources/updaters/slimemover/test-field-in-015-004.txt",
                this.SlimeMover.Update,
                new SlimeMover.Param());

            // 005:左際で左（片移動スライム塞ぎ）
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-015-005.txt",
                "../../resources/updaters/slimemover/test-field-in-015-005.txt",
                this.SlimeMover.Update,
                new SlimeMover.Param());

            // 006:左際で左（片移動スライム塞ぎ逆パターン）
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-015-006.txt",
                "../../resources/updaters/slimemover/test-field-in-015-006.txt",
                this.SlimeMover.Update,
                new SlimeMover.Param());
        }

        /// <summary>
        /// 016:Moveで壁際ではスライドして回転できる
        /// </summary>
        [TestMethod]
        public void Moveで壁際ではスライドして回転できる()
        {
            var param = new SlimeMover.Param();

            // 001:右壁際で右
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-016-001.txt",
                "../../resources/updaters/slimemover/test-field-in-016-001.txt",
                this.SlimeMover.Update,
                param);

            // 002:底辺で下
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-016-002.txt",
                "../../resources/updaters/slimemover/test-field-in-016-002.txt",
                this.SlimeMover.Update,
                param);

            // 003:左壁際で左
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimemover/test-field-out-016-003.txt",
                "../../resources/updaters/slimemover/test-field-in-016-003.txt",
                this.SlimeMover.Update,
                param);
        }
    }
}
