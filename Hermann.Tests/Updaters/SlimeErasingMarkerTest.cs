using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// SlimeErasingMarkerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class SlimeErasingMarkerTest
    {
        /// <summary>
        /// 設置情報更新機能
        /// </summary>
        private SlimeErasingMarker Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlimeErasingMarkerTest()
        {
            this.Updater = new SlimeErasingMarker();
        }

        /// <summary>
        /// 001：縦4
        /// </summary>
        [TestMethod]
        public void 縦4()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:1P-右端列-赤
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-001.txt",
                this.Updater.Update,
                param);

            // 002:1P-右から2番目列-赤
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-002.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-002.txt",
                this.Updater.Update,
                param);

            // 003:1P-右端列-緑
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-003.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-003.txt",
                this.Updater.Update,
                param);

            // 004:1P-右端列-赤-1/2の境界
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-004.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-004.txt",
                this.Updater.Update,
                param);
        }

        /// <summary>
        /// 002：横4
        /// </summary>
        [TestMethod]
        public void 横4()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:1P-2行目-2列目-赤
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-002-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-002-001.txt",
                this.Updater.Update,
                param);
        }

        /// <summary>
        /// 003：L字上1
        /// </summary>
        [TestMethod]
        public void L字上1()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-003-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-003-001.txt",
                this.Updater.Update,
                param);
        }

        /// <summary>
        /// 004：複合
        /// </summary>
        [TestMethod]
        public void 複合()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-004-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-004-001.txt",
                this.Updater.Update,
                param);
        }

        /// <summary>
        /// 005：おじゃまスライムは消済マークされない
        /// </summary>
        [TestMethod]
        public void おじゃまスライムは消済マークされない()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-005-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-005-001.txt",
                this.Updater.Update,
                param);
        }

        /// <summary>
        /// 006：最大連結数が適切に算出される
        /// </summary>
        [TestMethod]
        public void 最大連結数が適切に算出される()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:連結数が1以上
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimeerasingmarker/test-field-in-006-001.txt");
            var player = context.OperationPlayer;
            this.Updater.Update(context, player, param);
            Assert.AreEqual(7, param.MaxLinkedCount);

            // 002:連結数が0
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimeerasingmarker/test-field-in-006-002.txt");
            player = context.OperationPlayer;
            param = new SlimeErasingMarker.Param();
            this.Updater.Update(context, player, param);
            Assert.AreEqual(0, param.MaxLinkedCount);
        }

        /// <summary>
        /// 007：色数が適切に算出される
        /// </summary>
        [TestMethod]
        public void 色数が適切に算出される()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:色数1以上
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimeerasingmarker/test-field-in-007-001.txt");
            var player = context.OperationPlayer;
            this.Updater.Update(context, player, param);
            Assert.AreEqual(3, param.ColorCount);

            // 002:色数0
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimeerasingmarker/test-field-in-007-002.txt");
            player = context.OperationPlayer;
            param = new SlimeErasingMarker.Param();
            this.Updater.Update(context, player, param);
            Assert.AreEqual(0, param.ColorCount);
        }

        /// <summary>
        /// 008：全消しが適切に判断される
        /// </summary>
        [TestMethod]
        public void 全消しが適切に判断される()
        {
            var param = new SlimeErasingMarker.Param();

            // 001:全消し有
            var context = TestHelper.Receiver.Receive("../../resources/updaters/slimeerasingmarker/test-field-in-008-001.txt");
            var player = context.OperationPlayer;
            this.Updater.Update(context, player, param);
            Assert.AreEqual(true, param.AllErased);

            // 002:全消し無
            context = TestHelper.Receiver.Receive("../../resources/updaters/slimeerasingmarker/test-field-in-008-002.txt");
            param = new SlimeErasingMarker.Param();
            this.Updater.Update(context, player, param);
            Assert.AreEqual(false, param.AllErased);
        }
    }
}
