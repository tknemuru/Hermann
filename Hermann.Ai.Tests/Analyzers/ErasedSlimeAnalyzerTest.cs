using Hermann.Models;
using Hermann.Ai.Analyzers;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Hermann.Ai.Tests.Analyzers
{
    /// <summary>
    /// ErasedSlimeAnalyzerの単体テスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ErasedSlimeAnalyzerTest
    {
        /// <summary>
        /// 削除スライムの分析機能
        /// </summary>
        private ErasedSlimeAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErasedSlimeAnalyzerTest()
        {
            try
            {
                this.Analyzer = new ErasedSlimeAnalyzer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 001:削除されたスライムのみが抽出される
        /// </summary>
        [TestMethod]
        public void 削除されたスライムのみが抽出される()
        {
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/erasedslimeanalyzer/test-field-in-001-001.txt");
            var erasedContext = TestHelper.Receiver.Receive("../../resources/analyzers/erasedslimeanalyzer/test-field-in-001-001-erased.txt");
            var actual = this.Analyzer.Analyze(new ErasedSlimeAnalyzer.Param()
            {
                TargetContext = context,
                ErasedSlimes = erasedContext.SlimeFields[context.OperationPlayer.ToInt()][Slime.Erased],
            });
            var expected = TestHelper.Receiver.Receive("../../resources/analyzers/erasedslimeanalyzer/test-field-out-001-001.txt");
            TestHelper.AssertEqualsFieldContext(expected, actual);
        }
    }
}
