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
            // 001:1P-右端列
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-001.txt",
                this.Updater.Update);

            // 001:1P-右から2番目列
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-002.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-002.txt",
                this.Updater.Update);
        }
    }
}
