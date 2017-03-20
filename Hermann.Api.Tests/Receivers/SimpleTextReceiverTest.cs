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
            var slimeFields = context.SlimeFields[Player.First];
            var movableSlimes = context.MovableSlimes[Player.First];

            // 検証
            // 1P:右
            Assert.AreEqual(Player.First, context.OperationPlayer);
            Assert.AreEqual(Direction.Right, context.OperationDirection);

            // 2行目の左から2番目に赤がひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(2, 2), slimeFields[Slime.Red][0]);
            
            // 2行目の左から3番目に青がひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(2, 3), slimeFields[Slime.Blue][0]);

            // 2行目の左から3番目に移動可能な青がひとつ存在する
            Assert.AreEqual(TestHelper.GetFieldUnitIndex(2), movableSlimes[(int)MovableSlimeUnit.Index.First].Index);
            Assert.AreEqual(TestHelper.GetShift(2, 3), movableSlimes[(int)MovableSlimeUnit.Index.First].Position);
            Assert.AreEqual(Slime.Blue, movableSlimes[(int)MovableSlimeUnit.Index.First].Slime);
            // 2行目の左から2番目に移動可能な赤がひとつ存在する
            Assert.AreEqual(TestHelper.GetFieldUnitIndex(2), movableSlimes[(int)MovableSlimeUnit.Index.Second].Index);
            Assert.AreEqual(TestHelper.GetShift(2, 2), movableSlimes[(int)MovableSlimeUnit.Index.Second].Position);
            Assert.AreEqual(Slime.Red, movableSlimes[(int)MovableSlimeUnit.Index.Second].Slime);
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
