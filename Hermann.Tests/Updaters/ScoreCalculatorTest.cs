using System;
using Hermann.Tests.TestHelpers;
using Hermann.Updaters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Updaters
{
    /// <summary>
    /// ScoreCalculatorのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ScoreCalculatorTest
    {
        /// <summary>
        /// 計算機能
        /// </summary>
        private ScoreCalculator Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ScoreCalculatorTest()
        {
            this.Updater = new ScoreCalculator();
        }

        /// <summary>
        /// 001:ボーナス点のみ加算
        /// </summary>
        [TestMethod]
        public void ボーナス点のみ加算()
        {
            var context = TestHelper.Receiver.Receive("../../resources/updaters/scorecalculator/test-field-in-001-001.txt");
            var player = context.OperationPlayer;
            var param = new ScoreCalculator.Param(4);
            this.Updater.Update(context, player, param);
            Assert.AreEqual(4, param.ResultAddScore);
        }

        /// <summary>
        /// 002:連鎖で最大加算
        /// </summary>
        [TestMethod]
        public void 連鎖で最大加算()
        {
            var context = TestHelper.Receiver.Receive("../../resources/updaters/scorecalculator/test-field-in-002-001.txt");
            var player = context.OperationPlayer;
            var param = new ScoreCalculator.Param(15, 5, true);
            this.Updater.Update(context, player, param);
            Assert.AreEqual(23940, param.ResultAddScore);
        }

        /// <summary>
        /// 003:連鎖で最小加算
        /// </summary>
        [TestMethod]
        public void 連鎖で最小加算()
        {
            var context = TestHelper.Receiver.Receive("../../resources/updaters/scorecalculator/test-field-in-003-001.txt");
            var player = context.OperationPlayer;
            var param = new ScoreCalculator.Param(4, 1, false);
            this.Updater.Update(context, player, param);
            Assert.AreEqual(40, param.ResultAddScore);
        }
    }
}
