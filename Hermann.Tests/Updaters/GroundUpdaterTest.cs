using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.TestHelpers;
using Hermann.Collections;
using Hermann.Updaters;

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
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { true, false });

            // 002:1P-横-右端
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-002.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { true, false });

            // 001:2P-縦-左端
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-003.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { false, true });

            // 002:2P-縦-右端
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-001-004.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { false, true });
        }

        /// <summary>
        /// 002:接地していない
        /// </summary>
        [TestMethod]
        public void 接地していない()
        {
            // 001:最底辺
            var context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-002-001.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { false, false });

            // 002:最底辺以外
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-002-002.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { false, false });

            // 003:設置されたスライムが存在する
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-002-003.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { false, false });
        }

        /// <summary>
        /// 003:他スライムに接地する
        /// </summary>
        [TestMethod]
        public void 他スライムに接地する()
        {
            // 001:両スライムとも接地している
            var context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-003-001.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { true, true });

            // 002:片方のスライムが接地している
            context = TestHelper.Receiver.Receive("../../resources/updaters/groundupdater/test-field-in-003-002.txt");
            this.Updater.Update(context);
            CollectionAssert.AreEqual(context.Ground, new[] { true, true });
        }
    }
}
