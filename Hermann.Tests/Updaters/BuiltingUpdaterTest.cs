using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// BuiltingUpdaterのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class BuiltingUpdaterTest
    {
        /// <summary>
        /// 設置情報更新機能
        /// </summary>
        private BuiltingUpdater Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BuiltingUpdaterTest()
        {
            this.Updater = new BuiltingUpdater();
        }

        /// <summary>
        /// 001:最底辺に設置する
        /// </summary>
        [TestMethod]
        public void 最底辺に設置する()
        {
            // 001:1P-横-左端
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/builtingupdater/test-field-out-001-001.txt",
                "../../resources/updaters/builtingupdater/test-field-in-001-001.txt",
                this.Updater.Update);

            // 002:2P-縦-右端
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/builtingupdater/test-field-out-001-002.txt",
                "../../resources/updaters/builtingupdater/test-field-in-001-002.txt",
                this.Updater.Update);
        }
    }
}
