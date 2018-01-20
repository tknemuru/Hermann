using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.TestHelpers;
using Hermann.Updaters;
using Hermann.Generators;
using Hermann.Models;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// NextSlimeUpdaterのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class NextSlimeUpdaterTest
    {
        /// <summary>
        /// 設置情報更新機能
        /// </summary>
        private NextSlimeUpdater Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NextSlimeUpdaterTest()
        {
            var nextSlimeGen = new NextSlimeAssignedGenerator(new[] { Slime.Red, Slime.Purple });
            nextSlimeGen.UsingSlime = new[] { Slime.Red, Slime.Purple, Slime.Blue, Slime.Green };
            this.Updater = new NextSlimeUpdater();
            this.Updater.Injection(nextSlimeGen);
        }

        /// <summary>
        /// 001:NEXTスライムがスライドされる
        /// </summary>
        [TestMethod]
        public void NEXTスライムがスライドされる()
        {
            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/nextslimeupdater/test-field-out-001-001.txt",
                "../../resources/updaters/nextslimeupdater/test-field-in-001-001.txt",
                this.Updater.Update);
        }
    }
}
