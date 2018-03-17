using System;
using Hermann.Analyzers.Fields;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Analyzers.Fields
{
    /// <summary>
    /// SlimeJoinStateAnalyzerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class SlimeJoinStateAnalyzerTest
    {
        /// <summary>
        /// スライムの結合状態の分析機能
        /// </summary>
        private SlimeJoinStateAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlimeJoinStateAnalyzerTest()
        {
            this.Analyzer = new SlimeJoinStateAnalyzer();
        }

        /// <summary>
        /// 001:スライムの結合状態が分析できる
        /// </summary>
        [TestMethod]
        public void スライムの結合状態が分析できる()
        {
            // 001:上下左右結合以外
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/fields/slimejoinstateanalyzer/test-field-in-001-001.txt");
            var status = this.Analyzer.Analyze(context, context.OperationPlayer);

            // 初期状態
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(1, 2, true)]);

            // 初期状態（移動可能スライム）
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(2, 2, true)]);
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(2, 3, true)]);

            // 下結合
            Assert.AreEqual(SlimeJoinState.Down, status[TestHelper.GetShift(1, 4, true)]);

            // 上結合
            Assert.AreEqual(SlimeJoinState.Up, status[TestHelper.GetShift(2, 4, true)]);

            // 上下結合
            Assert.AreEqual(SlimeJoinState.UpDown, status[TestHelper.GetShift(4, 2, true)]);

            // 右結合
            Assert.AreEqual(SlimeJoinState.Right, status[TestHelper.GetShift(4, 4, true)]);

            // 下右結合
            Assert.AreEqual(SlimeJoinState.DownRight, status[TestHelper.GetShift(3, 3, true)]);

            // 上右結合
            Assert.AreEqual(SlimeJoinState.UpRight, status[TestHelper.GetShift(7, 1, true)]);

            // 上下右結合
            Assert.AreEqual(SlimeJoinState.UpDownRight, status[TestHelper.GetShift(7, 4, true)]);

            // 左結合
            Assert.AreEqual(SlimeJoinState.Left, status[TestHelper.GetShift(4, 6, true)]);

            // 下左結合
            Assert.AreEqual(SlimeJoinState.DownLeft, status[TestHelper.GetShift(9, 2, true)]);

            // 上左結合
            Assert.AreEqual(SlimeJoinState.UpLeft, status[TestHelper.GetShift(10, 4, true)]);

            // 上下左
            Assert.AreEqual(SlimeJoinState.UpDownLeft, status[TestHelper.GetShift(10, 6, true)]);

            // 右左結合
            Assert.AreEqual(SlimeJoinState.RightLeft, status[TestHelper.GetShift(4, 5, true)]);

            // 下右左結合
            Assert.AreEqual(SlimeJoinState.DownRightLeft, status[TestHelper.GetShift(11, 2, true)]);

            // 上右左結合
            Assert.AreEqual(SlimeJoinState.UpRightLeft, status[TestHelper.GetShift(12, 4, true)]);

            // 002:上下左右結合
            context = TestHelper.Receiver.Receive("../../resources/analyzers/fields/slimejoinstateanalyzer/test-field-in-001-002.txt");
            status = this.Analyzer.Analyze(context, context.OperationPlayer);

            // 上下右左結合
            Assert.AreEqual(SlimeJoinState.UpDownRightLeft, status[TestHelper.GetShift(4, 3, true)]);
        }
        /// <summary>
        /// 002:消済スライムの結合状態が分析できる
        /// </summary>
        [TestMethod]
        public void 消済スライムの結合状態が分析できる()
        {
            // 001:上下左右結合以外
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/fields/slimejoinstateanalyzer/test-field-in-002-001.txt");
            var status = this.Analyzer.Analyze(context, context.OperationPlayer);

            // 初期状態
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(1, 2, true)]);

            // 初期状態（移動可能スライム）
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(2, 2, true)]);
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(2, 3, true)]);

            // 下結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(1, 4, true)]);

            // 上結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(2, 4, true)]);

            // 上下結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(4, 2, true)]);

            // 右結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(4, 4, true)]);

            // 下右結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(3, 3, true)]);

            // 上右結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(7, 1, true)]);

            // 上下右結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(7, 4, true)]);

            // 左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(4, 6, true)]);

            // 下左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(9, 2, true)]);

            // 上左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(10, 4, true)]);

            // 上下左
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(10, 6, true)]);

            // 右左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(4, 5, true)]);

            // 下右左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(11, 2, true)]);

            // 上右左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(12, 4, true)]);

            // 002:上下左右結合
            context = TestHelper.Receiver.Receive("../../resources/analyzers/fields/slimejoinstateanalyzer/test-field-in-002-002.txt");
            status = this.Analyzer.Analyze(context, context.OperationPlayer);

            // 上下右左結合
            Assert.AreEqual(SlimeJoinState.Default, status[TestHelper.GetShift(4, 3, true)]);
        }
    }
}
