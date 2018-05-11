using System;
using Hermann.Ai.Analyzers;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Ai.Test.Analyzers
{
    /// <summary>
    /// ChainAnalyzerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ChainAnalyzerTest
    {
        /// <summary>
        /// 高低差の分析機能
        /// </summary>
        private ChainAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChainAnalyzerTest()
        {
            this.Analyzer = new ChainAnalyzer();
        }

        /// <summary>
        /// 001:最大連鎖数を正しく導ける
        /// </summary>
        [TestMethod]
        public void 最大連鎖数を正しく導ける()
        {
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/chainanalyzer/test-field-in-001-001.txt");
            var actual = this.Analyzer.Analyze(context, context.OperationPlayer);
            Assert.AreEqual(5, actual);
        }
    }
}
