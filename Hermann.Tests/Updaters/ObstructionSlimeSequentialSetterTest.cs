using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Updaters;
using Hermann.Tests.TestHelpers;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// ObstructionSlimeSequentialDropperのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ObstructionSlimeSequentialSetterTest
    {
        /// <summary>
        /// おじゃまスライム配置機能
        /// </summary>
        private ObstructionSlimeSequentialSetter Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObstructionSlimeSequentialSetterTest()
        {
            this.Updater = new ObstructionSlimeSequentialSetter();
        }

        /// <summary>
        /// 001:一つも配置させるおじゃまスライムが存在しない
        /// </summary>
        [TestMethod]
        public void 一つも配置させるおじゃまスライムが存在しない()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-001-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-001-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 002:一つだけ配置させるおじゃまスライムが存在する
        /// </summary>
        [TestMethod]
        public void 一つだけ配置させるおじゃまスライムが存在する()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-002-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-002-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 003:小スライム5個配置
        /// </summary>
        [TestMethod]
        public void 小スライム5個配置()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-003-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-003-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 004:大スライム1個配置
        /// </summary>
        [TestMethod]
        public void 大スライム1個配置()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-004-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-004-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 005:大スライム1個小スライム5個配置
        /// </summary>
        [TestMethod]
        public void 大スライム1個小スライム5個配置()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-005-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-005-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 006:大スライム3個小スライム5個配置
        /// </summary>
        [TestMethod]
        public void 大スライム3個小スライム5個配置()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-006-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-006-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 007:おじゃまスライム数が最大配置量を超えている
        /// </summary>
        [TestMethod]
        public void おじゃまスライム数が最大配置量を超えている()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-007-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-007-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 008:スライムが隠し領域まで積まれている
        /// </summary>
        [TestMethod]
        public void スライムが隠し領域まで積まれている()
        {
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-out-008-001.txt",
                "../../resources/updaters/obstructionslimesequentialsetter/test-field-in-008-001.txt",
                this.Updater.Update);
        }
    }
}
