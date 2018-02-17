using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// WinCountUpdaterのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class WinCountUpdaterTest
    {
        /// <summary>
        /// おじゃまスライム消し済マーク機能
        /// </summary>
        private WinCountUpdater Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WinCountUpdaterTest()
        {
            this.Updater = new WinCountUpdater();
        }

        /// <summary>
        /// 001:左から3列目が埋まっていない
        /// </summary>
        [TestMethod]
        public void 左から3列目が埋まっていない()
        {
            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/wincountupdater/test-field-out-001-001.txt",
                "../../resources/updaters/wincountupdater/test-field-in-001-001.txt",
                this.Updater.Update);

            // 002:2P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/wincountupdater/test-field-out-001-002.txt",
                "../../resources/updaters/wincountupdater/test-field-in-001-002.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 002:左から3列目が埋まっている
        /// </summary>
        [TestMethod]
        public void 左から3列目が埋まっている()
        {
            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/wincountupdater/test-field-out-002-001.txt",
                "../../resources/updaters/wincountupdater/test-field-in-002-001.txt",
                this.Updater.Update);

            // 002:2P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/wincountupdater/test-field-out-002-002.txt",
                "../../resources/updaters/wincountupdater/test-field-in-002-002.txt",
                this.Updater.Update);
        }
    }
}
