using System;
using System.Collections.Generic;
using System.Linq;
using Hermann.Ai.Generators;
using Hermann.Ai.Models;
using Hermann.Ai.Providers;
using Hermann.Helpers;
using Hermann.Ai.Di;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Di;
using Hermann.Environments;

namespace Hermann.Ai.Tests.Generators
{
    /// <summary>
    /// PatternGeneratorのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class PatternGeneratorTest
    {
        /// <summary>
        /// 底上げインデックス量
        /// </summary>
        private const int RaisedIndex = 16;

        /// <summary>
        /// マージされたフィールド状態の生成機能
        /// </summary>
        private PatternGenerator Generator { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PatternGeneratorTest()
        {
            this.Generator = new PatternGenerator();
        }

        /// <summary>
        /// 001:PatternGeneratorのGetNextテスト
        /// </summary>
        [TestMethod]
        public void PatternGeneratorのGetNextテスト()
        {
            EnvConfig.Unity = false;
            var patterns = new List<PatternDefinition>();
            var stairsOneLeft = new PatternProvider().Get(Pattern.StairsOneLeft);
            var stairsTwoRight = new PatternProvider().Get(Pattern.StairsTwoRight);
            patterns.Add(stairsOneLeft);
            patterns.Add(stairsTwoRight);
            this.Generator.Inject(new PatternGenerator.Config()
            {
                Patterns = patterns,
            });

            var stairsOneLeftColors1P = new Dictionary<int, double>();
            var stairsOneLeftObs1P = new Dictionary<int, double>();
            var stairsOneLeftColors2P = new Dictionary<int, double>();
            var stairsOneLeftObs2P = new Dictionary<int, double>();

            // 1P-紫-StairsOneLeft
            var unitStr = new List<string>();
            unitStr.Add("10000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            var key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors1P, key);

            // 1P-赤-StairsOneLeft
            unitStr = new List<string>();
            unitStr.Add("10000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors1P, key);
            this.AddCount(stairsOneLeftColors1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors1P, key);

            // 1P-おじゃま-StairsOneLeft
            unitStr = new List<string>();
            unitStr.Add("10000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftObs1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftObs1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftObs1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftObs1P, key);

            // 2P-青-StairsOneLeft
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);

            // 2P-紫-StairsOneLeft
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);

            // 2P-赤-StairsOneLeft
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftColors2P, key);

            // 2P-おじゃま-StairsOneLeft
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            key = stairsOneLeft.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsOneLeftObs2P, key);

            var stairsTwoRightColors1P = new Dictionary<int, double>();
            var stairsTwoRightObs1P = new Dictionary<int, double>();
            var stairsTwoRightColors2P = new Dictionary<int, double>();
            var stairsTwoRightObs2P = new Dictionary<int, double>();

            // 1P-紫-StairsTwoRight
            unitStr = new List<string>();
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors1P, key);

            // 1P-赤-StairsTwoRight
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors1P, key);
            this.AddCount(stairsTwoRightColors1P, key);
            unitStr = new List<string>();
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors1P, key);
            this.AddCount(stairsTwoRightColors1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("10000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("10000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors1P, key);

            // 1P-おじゃま-StairsTwoRight
            unitStr = new List<string>();
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightObs1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightObs1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("10000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightObs1P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("10000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightObs1P, key);

            // 2P-青-StairsTwoRight
            unitStr = new List<string>();
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors2P, key);

            // 2P-紫-StairsTwoRight
            unitStr = new List<string>();
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors2P, key);
            unitStr = new List<string>();
            unitStr.Add("00000000");
            unitStr.Add("01000000");
            unitStr.Add("00000000");
            unitStr.Add("00000000");
            key = stairsTwoRight.GetIndex(FieldContextHelper.ConvertDigitStrsToUnit(unitStr));
            this.AddCount(stairsTwoRightColors2P, key);

            var context = TestHelper.Receiver.Receive("../../resources/generators/patterngenerator/test-field-in-001-001.txt");
            var actual = this.Generator.GetNext(context);
            Assert.AreEqual(RaisedIndex * 8, actual.Length);

            var raisedIndex = 0;
            // 1P-StairsOneLeft-color
            foreach(var kv in stairsOneLeftColors1P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsOneLeft.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 1P-StairsOneLeft-obs
            foreach (var kv in stairsOneLeftObs1P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsOneLeft.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 1P-StairsTwoRight-color
            foreach (var kv in stairsTwoRightColors1P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsTwoRight.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 1P-StairsTwoRight-obs
            foreach (var kv in stairsTwoRightObs1P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsTwoRight.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 2P-StairsOneLeft-color
            foreach (var kv in stairsOneLeftColors2P)
            {
                FileHelper.WriteLine($"key:{kv.Key} value:{kv.Value}");
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsOneLeft.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 2P-StairsOneLeft-obs
            foreach (var kv in stairsOneLeftObs2P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsOneLeft.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 2P-StairsTwoRight-color
            foreach (var kv in stairsTwoRightColors2P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsTwoRight.GetIndexKey(kv.Key)));
            }
            raisedIndex += RaisedIndex;

            // 2P-StairsTwoRight-obs
            foreach (var kv in stairsTwoRightObs2P)
            {
                Assert.AreEqual(kv.Value, actual[kv.Key + raisedIndex], 0.0d, DebugHelper.ConvertUintToFieldUnit(stairsTwoRight.GetIndexKey(kv.Key)));
            }
        }

        /// <summary>
        /// カウントアップします。
        /// </summary>
        /// <param name="dic">カウントアップ対象の辞書</param>
        /// <param name="key">キー</param>
        private void AddCount(Dictionary<int, double> dic, int key)
        {
            if (dic.ContainsKey(key))
            {
                dic[key]++;
            } else
            {
                dic.Add(key, 1.0d);
            }
        }
    }
}
