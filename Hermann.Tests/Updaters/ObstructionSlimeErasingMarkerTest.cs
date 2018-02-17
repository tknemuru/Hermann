using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// ObstructionSlimeErasingMarkerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ObstructionSlimeErasingMarkerTest
    {
        /// <summary>
        /// おじゃまスライム消し済マーク機能
        /// </summary>
        private ObstructionSlimeErasingMarker Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObstructionSlimeErasingMarkerTest()
        {
            this.Updater = new ObstructionSlimeErasingMarker();
        }

        /// <summary>
        /// 001:消し済マークが一つも存在しない
        /// </summary>
        [TestMethod]
        public void 消し済マークが一つも存在しない()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-out-001-001.txt",
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-in-001-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 002:消し済マークが存在しているが消し済対象のおじゃまスライムがいない
        /// </summary>
        [TestMethod]
        public void 消し済マークが存在しているが消し済対象のおじゃまスライムがいない()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-out-002-001.txt",
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-in-002-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 003:消し済マークが存在していて消し済対象のおじゃまスライムがいる
        /// </summary>
        [TestMethod]
        public void 消し済マークが存在していて消し済対象のおじゃまスライムがいる()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-out-003-001.txt",
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-in-003-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 004:消し済マークが際の部分に存在している
        /// </summary>
        [TestMethod]
        public void 消し済マークが際の部分に存在している()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-out-004-001.txt",
                "../../resources/updaters/obstructionslimeerasingmarker/test-field-in-004-001.txt",
                this.Updater.Update);
        }
    }
}
