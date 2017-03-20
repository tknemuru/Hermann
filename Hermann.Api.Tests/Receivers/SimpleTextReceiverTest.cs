using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using Hermann.Tests.Di;
using Hermann.Api.Receivers;
using Hermann.Contexts;
using Hermann.Collections;
using Hermann.Tests.TestHelpers;

namespace Hermann.Api.Tests.Receivers
{
    /// <summary>
    /// SimpleTextReceiverテストクラス
    /// </summary>
    [TestClass]
    public class SimpleTextReceiverTest
    {
        /// <summary>
        /// 001:基本の動作をテストします。
        /// </summary>
        [TestMethod]
        public void Receive基本パターン()
        {
            // [001]1P:右
            var context = TestHelper.Receiver.Receive("../../resources/receivers/simple-text-file-receiver/test-field-in-001-001.txt");

            // 検証
            // 1P:右
            Assert.AreEqual(Convert.ToUInt32("1000", 2), context.Command);

            // 2行目の左から2番目に赤がひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(2, 2), context.SlimeFields[Slime.Red][0]);
            
            // 2行目の左から3番目に青がひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(2, 3), context.SlimeFields[Slime.Blue][0]);

            // 2行目の左から3番目に移動可能な青がひとつ存在する
            Assert.AreEqual(TestHelper.GetFieldUnitIndex(2), context.MovableInfos[(int)MovableUnit.First].Index);
            Assert.AreEqual(TestHelper.GetShift(2, 3), context.MovableInfos[(int)MovableUnit.First].Position);
            Assert.AreEqual(Slime.Blue, context.MovableInfos[(int)MovableUnit.First].Slime);
            // 2行目の左から2番目に移動可能な赤がひとつ存在する
            Assert.AreEqual(TestHelper.GetFieldUnitIndex(2), context.MovableInfos[(int)MovableUnit.Second].Index);
            Assert.AreEqual(TestHelper.GetShift(2, 2), context.MovableInfos[(int)MovableUnit.Second].Position);
            Assert.AreEqual(Slime.Red, context.MovableInfos[(int)MovableUnit.Second].Slime);
        }

        //private bool AssertAreAllEqualZero(ulong[] context, bool useDefault, params int[] exclusionIndexList)
        //{
        //    if (useDefault)
        //    {
        //        var defaultExclusionList = new int[] { 
        //            FieldContext.IndexCommand,
        //            FieldContext.IndexOccupiedFieldUpper,
        //            FieldContext.IndexOccupiedFieldLower,
        //            FieldContext.IndexMovableFieldUpper,
        //            FieldContext.IndexMovableFieldLower
        //        };

        //        for (var i = 0; i < defaultExclusionList.Length; i++)
        //        {
        //            //if(context[defaultExclusionList[i]] )
        //            Assert.AreEqual(0ul, context[FieldContext.IndexRedFieldUpper]);
        //        }
        //    }
        //}
    }
}
