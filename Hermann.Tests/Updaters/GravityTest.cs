using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;
using Hermann.Contexts;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// Gravityのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class GravityTest
    {
        /// <summary>
        /// 設置情報更新機能
        /// </summary>
        private Gravity Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GravityTest()
        {
            this.Updater = new Gravity();
        }

        /// <summary>
        /// 001:最底辺ユニット内で落下
        /// </summary>
        [TestMethod]
        public void 最底辺ユニット内で落下()
        {
            // 001:1P-赤-他スライム無し
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/gravity/test-field-out-001-001.txt",
                "../../resources/updaters/gravity/test-field-in-001-001.txt",
                this.Updater.Update,
                new Gravity.Param());

            // 002:1P-複数色-他スライム有り
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/gravity/test-field-out-001-002.txt",
                "../../resources/updaters/gravity/test-field-in-001-002.txt",
                this.Updater.Update,
                new Gravity.Param());
        }

        /// <summary>
        /// 002:ユニットをまたいで落下
        /// </summary>
        [TestMethod]
        public void ユニットをまたいで落下()
        {
            var param = new Gravity.Param
            {
                Strength = FieldContextConfig.HorizontalLineLength
            };

            // 001:1P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/gravity/test-field-out-002-001.txt",
                "../../resources/updaters/gravity/test-field-in-002-001.txt",
                this.Updater.Update,
                param);
        }

        /// <summary>
        /// 003:隠し領域で落下
        /// </summary>
        [TestMethod]
        public void 隠し領域で落下()
        {
            var param = new Gravity.Param
            {
                Strength = 99
            };

            // 001:2P
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/gravity/test-field-out-003-001.txt",
                "../../resources/updaters/gravity/test-field-in-003-001.txt",
                this.Updater.Update,
                param);
        }
    }
}
