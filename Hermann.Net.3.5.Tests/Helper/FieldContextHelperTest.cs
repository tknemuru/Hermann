using System;
using Hermann.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Helper
{
    /// <summary>
    /// FieldContextHelperのテスト実行機能を提供します。
    /// </summary>
    [TestClass]
    public class FieldContextHelperTest
    {
        /// <summary>
        /// 001:GetLineIndexで隠し行を含まない行数を取得する
        /// </summary>
        [TestMethod]
        public void GetLineIndexで隠し行を含まない行数を取得する()
        {
            var actual = -1;

            actual = FieldContextHelper.GetLineIndex(2, 0);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetLineIndex(2, 7);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetLineIndex(2, 24);
            Assert.AreEqual(3, actual);
            actual = FieldContextHelper.GetLineIndex(2, 31);
            Assert.AreEqual(3, actual);

            actual = FieldContextHelper.GetLineIndex(4, 0);
            Assert.AreEqual(8, actual);
            actual = FieldContextHelper.GetLineIndex(4, 7);
            Assert.AreEqual(8, actual);
            actual = FieldContextHelper.GetLineIndex(4, 24);
            Assert.AreEqual(11, actual);
            actual = FieldContextHelper.GetLineIndex(4, 31);
            Assert.AreEqual(11, actual);
        }

        /// <summary>
        /// 002:GetLineIndexで隠し行を含む行数を取得する
        /// </summary>
        [TestMethod]
        public void GetLineIndexで隠し行を含む行数を取得する()
        {
            var actual = -1;

            actual = FieldContextHelper.GetLineIndex(0, 0, true);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetLineIndex(0, 31, true);
            Assert.AreEqual(3, actual);

            actual = FieldContextHelper.GetLineIndex(4, 0, true);
            Assert.AreEqual(16, actual);
            actual = FieldContextHelper.GetLineIndex(4, 31, true);
            Assert.AreEqual(19, actual);
        }

        /// <summary>
        /// 003:GetColumnIndexで隠し列を含まない列数を取得する
        /// </summary>
        [TestMethod]
        public void GetColumnIndexで隠し列を含まない列数を取得する()
        {
            var actual = -1;

            actual = FieldContextHelper.GetColumnIndex(2);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetColumnIndex(7);
            Assert.AreEqual(5, actual);
            actual = FieldContextHelper.GetColumnIndex(26);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetColumnIndex(31);
            Assert.AreEqual(5, actual);
        }

        /// <summary>
        /// 004:GetColumnIndexで隠し列を含む列数を取得する
        /// </summary>
        [TestMethod]
        public void GetColumnIndexで隠し列を含む列数を取得する()
        {
            var actual = -1;

            actual = FieldContextHelper.GetColumnIndex(0, true);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetColumnIndex(7, true);
            Assert.AreEqual(7, actual);
            actual = FieldContextHelper.GetColumnIndex(24, true);
            Assert.AreEqual(0, actual);
            actual = FieldContextHelper.GetColumnIndex(31, true);
            Assert.AreEqual(7, actual);
        }
    }
}
