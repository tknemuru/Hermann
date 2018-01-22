using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.TestHelpers;
using Hermann.Models;
using Hermann.Updaters;
using Hermann.Helpers;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// GroundUpdaterのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class GroundUpdaterTest
    {
        /// <summary>
        /// 接地情報更新機能
        /// </summary>
        private GroundUpdater Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GroundUpdaterTest()
        {
            this.Updater = new GroundUpdater();
        }

        /// <summary>
        /// 001:最底辺に接地する
        /// </summary>
        [TestMethod]
        public void 最底辺に接地する()
        {
            // 001:1P-横-左端
            var context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-001.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { true, false }, context.Ground);

            // 002:1P-横-右端
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-002.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { true, false }, context.Ground);

            // 001:2P-縦-左端
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-003.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { false, true }, context.Ground);

            // 002:2P-縦-右端
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-004.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { false, true }, context.Ground);
        }

        /// <summary>
        /// 002:接地していない
        /// </summary>
        [TestMethod]
        public void 接地していない()
        {
            // 001:最底辺
            var context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-002-001.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { false, false }, context.Ground);

            // 002:最底辺以外
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-002-002.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { false, false }, context.Ground);

            // 003:設置されたスライムが存在する
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-002-003.txt");
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { false, false }, context.Ground);
        }

        /// <summary>
        /// 003:他スライムに接地する
        /// </summary>
        [TestMethod]
        public void 他スライムに接地する()
        {
            // 001:両スライムとも接地している
            var context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-003-001.txt");
            this.Updater.Update(context, context.OperationPlayer);
            context.OperationPlayer = Player.Index.Second;
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { true, true }, context.Ground);

            // 002:片方のスライムが接地している
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-003-002.txt");
            this.Updater.Update(context, context.OperationPlayer);
            context.OperationPlayer = Player.Index.Second;
            this.Updater.Update(context, context.OperationPlayer);
            CollectionAssert.AreEqual(new[] { true, true }, context.Ground);
        }
    }
}
