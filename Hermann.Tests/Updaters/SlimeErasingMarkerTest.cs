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
            // 001:1P-右端列-赤
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-001.txt",
                this.Updater.Update);

            // 002:1P-右から2番目列-赤
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-002.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-002.txt",
                this.Updater.Update);

            // 003:1P-右端列-緑
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-003.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-003.txt",
                this.Updater.Update);

            // 004:1P-右端列-赤-1/2の境界
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-001-004.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-001-004.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 002：横4
        /// </summary>
        [TestMethod]
        public void 横4()
        {
            // 001:1P-2行目-2列目-赤
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-002-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-002-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 003：L字上1
        /// </summary>
        [TestMethod]
        public void L字上1()
        {
            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-003-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-003-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 004：複合
        /// </summary>
        [TestMethod]
        public void 複合()
        {
            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-004-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-004-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 005：おじゃまスライムは消済マークされない
        /// </summary>
        [TestMethod]
        public void おじゃまスライムは消済マークされない()
        {
            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/slimeerasingmarker/test-field-out-005-001.txt",
                "../../resources/updaters/slimeerasingmarker/test-field-in-005-001.txt",
                this.Updater.Update);
        }
    }
}
