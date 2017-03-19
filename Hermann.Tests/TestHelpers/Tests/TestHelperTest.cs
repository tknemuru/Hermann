using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.TestHelpers.Tests
{
    /// <summary>
    /// TestHelperクラスのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class TestHelperTest
    {
        /// <summary>
        /// 001:行指定のGetShiftテスト
        /// </summary>
        [TestMethod]
        public void 行指定のGetShiftテスト()
        {
            // 1つめのユニット
            Assert.AreEqual(0, TestHelper.GetShift(1));
            Assert.AreEqual(24, TestHelper.GetShift(4));

            // 2つめのユニット
            Assert.AreEqual(0, TestHelper.GetShift(5));
            Assert.AreEqual(24, TestHelper.GetShift(8));

            // 3つめのユニット
            Assert.AreEqual(0, TestHelper.GetShift(9));
            Assert.AreEqual(24, TestHelper.GetShift(12));
        }

        /// <summary>
        /// 002:行・列指定のGetShiftテスト
        /// </summary>
        [TestMethod]
        public void 行・列指定のGetShiftテスト()
        {
            // 1行目の右端
            Assert.AreEqual(2, TestHelper.GetShift(1, 6));
            
            // 1行目の左端
            Assert.AreEqual(7, TestHelper.GetShift(1, 1));

            // 4行目の右端
            Assert.AreEqual(2 + 24, TestHelper.GetShift(4, 6));

            // 4行目の左端
            Assert.AreEqual(7 + 24, TestHelper.GetShift(4, 1));

            // 5行目の右端
            Assert.AreEqual(2, TestHelper.GetShift(5, 6));

            // 5行目の左端
            Assert.AreEqual(7, TestHelper.GetShift(5, 1));

            // 8行目の右端
            Assert.AreEqual(2 + 24, TestHelper.GetShift(8, 6));

            // 8行目の左端
            Assert.AreEqual(7 + 24, TestHelper.GetShift(8, 1));

            // 9行目の右端
            Assert.AreEqual(2, TestHelper.GetShift(9, 6));

            // 9行目の左端
            Assert.AreEqual(7, TestHelper.GetShift(9, 1));

            // 12行目の右端
            Assert.AreEqual(2 + 24, TestHelper.GetShift(12, 6));

            // 12行目の左端
            Assert.AreEqual(7 + 24, TestHelper.GetShift(12, 1));
        }

        /// <summary>
        /// 003:GetFieldUnitIndexテスト
        /// </summary>
        [TestMethod]
        public void GetFieldUnitIndexテスト()
        {
            Assert.AreEqual(0, TestHelper.GetFieldUnitIndex(1));
            Assert.AreEqual(0, TestHelper.GetFieldUnitIndex(4));
            Assert.AreEqual(1, TestHelper.GetFieldUnitIndex(5));
            Assert.AreEqual(1, TestHelper.GetFieldUnitIndex(8));
            Assert.AreEqual(2, TestHelper.GetFieldUnitIndex(9));
            Assert.AreEqual(2, TestHelper.GetFieldUnitIndex(12));
        }

        /// <summary>
        /// 004:GetFieldテスト
        /// </summary>
        [TestMethod]
        public void GetFieldテスト()
        {
            // 1行目の右端
            Assert.AreEqual(Convert.ToUInt32("00000100", 2), TestHelper.GetField(1, 6));

            // 1行目の左端
            Assert.AreEqual(Convert.ToUInt32("10000000", 2), TestHelper.GetField(1, 1));

            // 4行目の右端
            Assert.AreEqual(Convert.ToUInt32("00000100", 2) << 24, TestHelper.GetField(4, 6));

            // 4行目の左端
            Assert.AreEqual(Convert.ToUInt32("10000000", 2) << 24, TestHelper.GetField(4, 1));

            // 5行目の右端
            Assert.AreEqual(Convert.ToUInt32("00000100", 2), TestHelper.GetField(5, 6));

            // 5行目の左端
            Assert.AreEqual(Convert.ToUInt32("10000000", 2), TestHelper.GetField(5, 1));

            // 8行目の右端
            Assert.AreEqual(Convert.ToUInt32("00000100", 2) << 24, TestHelper.GetField(8, 6));

            // 8行目の左端
            Assert.AreEqual(Convert.ToUInt32("10000000", 2) << 24, TestHelper.GetField(8, 1));

            // 9行目の右端
            Assert.AreEqual(Convert.ToUInt32("00000100", 2), TestHelper.GetField(9, 6));

            // 9行目の左端
            Assert.AreEqual(Convert.ToUInt32("10000000", 2), TestHelper.GetField(9, 1));

            // 12行目の右端
            Assert.AreEqual(Convert.ToUInt32("00000100", 2) << 24, TestHelper.GetField(12, 6));

            // 12行目の左端
            Assert.AreEqual(Convert.ToUInt32("10000000", 2) << 24, TestHelper.GetField(12, 1));
        }
    }
}
