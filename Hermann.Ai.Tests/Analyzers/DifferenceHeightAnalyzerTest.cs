using System;
using Hermann.Ai.Analyzers;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Ai.Test.Analyzers
{
    /// <summary>
    /// DifferenceHeightAnalyzerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class DifferenceHeightAnalyzerTest
    {
        /// <summary>
        /// 高低差の分析機能
        /// </summary>
        private DifferenceHeightAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DifferenceHeightAnalyzerTest()
        {
            this.Analyzer = new DifferenceHeightAnalyzer();
        }

        [TestMethod]
        public void 高低差を正しく導ける()
        {
            // 001:差あり
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/differenceheightanalyzer/test-field-in-001-001.txt");
            var actual = this.Analyzer.Analyze(context, context.OperationPlayer);
            Assert.AreEqual(11, actual);

            // 002:差なし
            context = TestHelper.Receiver.Receive("../../resources/analyzers/differenceheightanalyzer/test-field-in-001-002.txt");
            actual = this.Analyzer.Analyze(context, context.OperationPlayer);
            Assert.AreEqual(0, actual);
        }
    }
}
