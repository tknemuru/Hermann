using System;
using System.Linq;
using System.Collections.Generic;
using Hermann.Ai.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Helpers;

namespace Hermann.Ai.Tests.Generators
{
    /// <summary>
    /// MergedFieldsGeneratorのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class MergedFieldsGeneratorTest
    {
        /// <summary>
        /// マージされたフィールド状態の生成機能
        /// </summary>
        private MergedFieldsGenerator Generator { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MergedFieldsGeneratorTest()
        {
            this.Generator = new MergedFieldsGenerator();
        }

        /// <summary>
        /// 001:MergedFieldsGeneratorのGetNextテスト
        /// </summary>
        [TestMethod]
        public void MergedFieldsGeneratorのGetNextテスト()
        {
            var units = new List<uint>();
            var unitStr = new List<string>();
            unitStr.Add("10000000");
            unitStr.Add("01000000");
            unitStr.Add("00100000");
            unitStr.Add("00010000");
            units.Add(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            unitStr = new List<string>();
            unitStr.Add("00001000");
            unitStr.Add("00000100");
            unitStr.Add("00000010");
            unitStr.Add("00000001");
            units.Add(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));

            var expected = new List<uint>();
            expected.Add(units.First());
            unitStr = new List<string>();
            unitStr.Add("01000000");
            unitStr.Add("00100000");
            unitStr.Add("00010000");
            unitStr.Add("00001000");
            expected.Add(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            unitStr = new List<string>();           
            unitStr.Add("00100000");
            unitStr.Add("00010000");
            unitStr.Add("00001000");
            unitStr.Add("00000100");
            expected.Add(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            unitStr = new List<string>();
            unitStr.Add("00010000");
            unitStr.Add("00001000");
            unitStr.Add("00000100");
            unitStr.Add("00000010");
            expected.Add(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            unitStr = new List<string>();         
            unitStr.Add("00001000");
            unitStr.Add("00000100");
            unitStr.Add("00000010");
            unitStr.Add("00000001");
            expected.Add(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));

            var actual = this.Generator.GetNext(units.ToArray()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
