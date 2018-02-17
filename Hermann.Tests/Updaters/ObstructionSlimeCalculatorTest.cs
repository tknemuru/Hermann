using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.TestHelpers;
using Hermann.Updaters;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// ObstructionSlimeCalculatorのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ObstructionSlimeCalculatorTest
    {
        /// <summary>
        /// おじゃまスライム計算機能
        /// </summary>
        private ObstructionSlimeCalculator Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObstructionSlimeCalculatorTest()
        {
            this.Updater = new ObstructionSlimeCalculator();
        }

        /// <summary>
        /// 001:おじゃまスライム無から新たに追加
        /// </summary>
        [TestMethod]
        public void おじゃまスライム無から新たに追加()
        {
            // 001:小スライム5個
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimecalculator/test-field-out-001-001.txt",
                "../../resources/updaters/obstructionslimecalculator/test-field-in-001-001.txt",
                this.Updater.Update);
        }

        /// <summary>
        /// 002:おじゃまスライム有から新たに追加
        /// </summary>
        [TestMethod]
        public void おじゃまスライム有から新たに追加()
        {
            // 001:小スライム5個 + 小スライム5個 => 大スライム1個 + 小スライム4個
            TestHelper.AssertEqualsFieldContext(
                "../../resources/updaters/obstructionslimecalculator/test-field-out-002-001.txt",
                "../../resources/updaters/obstructionslimecalculator/test-field-in-002-001.txt",
                this.Updater.Update);
        }
    }
}
