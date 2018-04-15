using System;
using System.Linq;
using Hermann.Analyzers;
using Hermann.Tests.Di;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Analyzers
{
    [TestClass]
    public class MovableDirectionAnalyzerTest
    {
        /// <summary>
        /// 移動可能な方向の分析機能
        /// </summary>
        private MovableDirectionAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MovableDirectionAnalyzerTest()
        {
            TestDiProvider.GetContainer();
            this.Analyzer = new MovableDirectionAnalyzer();
        }

        /// <summary>
        /// 001:分析前と状態が変わらない
        /// </summary>
        [TestMethod]
        public void 分析前と状態が変わらない()
        {
            var actual = TestHelper.Receiver.Receive("../../resources/analyzers/movabledirectionanalyzer/test-field-in-001-001.txt");
            var expected = actual.DeepCopy();
            this.Analyzer.Analyze(actual, actual.OperationPlayer);
            TestHelper.AssertEqualsFieldContext(expected, actual);
        }

        /// <summary>
        /// 002:イベント発生中は移動不可能
        /// </summary>
        [TestMethod]
        public void イベント発生中は移動不可能()
        {
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/movabledirectionanalyzer/test-field-in-002-001.txt");
            var actual = this.Analyzer.Analyze(context, context.OperationPlayer);
            Assert.AreEqual(0, actual.Count());
        }
    }
}
