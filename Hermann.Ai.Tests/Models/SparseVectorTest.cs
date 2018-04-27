using System;
using Hermann.Ai.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Ai.Tests.Models
{
    /// <summary>
    /// SparseVectorのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class SparseVectorTest
    {
        /// <summary>
        /// 001:Addテスト
        /// </summary>
        [TestMethod]
        public void Addテスト()
        {
            var x = new SparseVector<double>(0.0d);
            var y = new SparseVector<double>(0.0d);
            x.Add(1.0d);
            x.Add(2.0d);
            y.Add(3.0d);
            y.Add(4.0d);
            y.Add(5.0d);
            x.Add(y);

            Assert.AreEqual(5, x.Length);
            var expected = 1.0d;
            foreach(var d in x)
            {
                Assert.AreEqual(expected, d);
                expected += 1.0d;
            }
        }
    }
}
