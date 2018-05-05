using System;
using Hermann.Ai.Analyzers;
using Hermann.Helpers;
using Hermann.Models;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Ai.Test.Analyzers
{
    /// <summary>
    /// ErasedPotentialSlimeAnalyzerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class ErasedPotentialSlimeAnalyzerTest
    {
        /// <summary>
        /// 移動可能な方向の分析機能
        /// </summary>
        private ErasedPotentialSlimeAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErasedPotentialSlimeAnalyzerTest()
        {
            try
            {
                this.Analyzer = new ErasedPotentialSlimeAnalyzer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 001:全部消しで削除可能性スライムの分析結果が正しく得られる
        /// </summary>
        [TestMethod]
        public void 全部消しで削除可能性スライムの分析結果が正しく得られる()
        {
            // 001:削除対象有り
            var param = new ErasedPotentialSlimeAnalyzer.Param();
            param.TargetSlime = Slime.Red;
            var context = TestHelper.Receiver.Receive("../../resources/analyzers/erasedpotentialslimeanalyzer/test-field-in-001-001.txt");
            var actual = this.Analyzer.Analyze(context, context.OperationPlayer, param);
            Assert.AreEqual(9, actual);

            // 002:削除対象無し
            param = new ErasedPotentialSlimeAnalyzer.Param();
            param.TargetSlime = Slime.Red;
            context = TestHelper.Receiver.Receive("../../resources/analyzers/erasedpotentialslimeanalyzer/test-field-in-001-002.txt");
            actual = this.Analyzer.Analyze(context, context.OperationPlayer, param);
            Assert.AreEqual(0, actual);
        }
    }
}
