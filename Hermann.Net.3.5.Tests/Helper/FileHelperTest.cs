using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Helpers.Tests
{
    /// <summary>
    /// FileHelperのテスト実行機能を提供します。
    /// </summary>
    [TestClass]
    public class FileHelperTest
    {
        /// <summary>
        /// 001:単純なファイルが読み込めることをテストします。
        /// </summary>
        [TestMethod]
        public void ReadTextLinesで単純なファイルが読み込める()
        {
            var lines = FileHelper.ReadTextLines("../../resources/helpers/file-helper/test-field-in-001-001.txt").ToArray();
            Assert.AreEqual("0", lines[0]);
            Assert.AreEqual("右", lines[1]);
        }
    }
}
