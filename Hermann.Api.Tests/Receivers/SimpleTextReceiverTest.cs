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

            // 3行目の左から3番目に緑がひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(3, 3), slimeFields[Slime.Green][TestHelper.GetFieldUnitIndex(3)]);
                     
            // 4行目の左から3番目に黄がひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(4, 3), slimeFields[Slime.Yellow][TestHelper.GetFieldUnitIndex(4)]);

            // 16行目の左から2番目におじゃまがひとつ存在する
            TestHelper.AssertAreEqualUint(TestHelper.GetField(16, 2), slimeFields[Slime.Obstruction][TestHelper.GetFieldUnitIndex(16)]);

            // 3行目の左から3番目に移動可能な緑がひとつ存在する
            Assert.AreEqual(TestHelper.GetFieldUnitIndex(3), movableSlimes[(int)MovableSlime.UnitIndex.First].Index);
            Assert.AreEqual(TestHelper.GetShift(3, 3), movableSlimes[(int)MovableSlime.UnitIndex.First].Position);
            Assert.AreEqual(Slime.Green, movableSlimes[(int)MovableSlime.UnitIndex.First].Slime);
            // 4行目の左から3番目に移動可能な赤がひとつ存在する
            Assert.AreEqual(TestHelper.GetFieldUnitIndex(4), movableSlimes[(int)MovableSlime.UnitIndex.Second].Index);
            Assert.AreEqual(TestHelper.GetShift(4, 3), movableSlimes[(int)MovableSlime.UnitIndex.Second].Position);
            Assert.AreEqual(Slime.Yellow, movableSlimes[(int)MovableSlime.UnitIndex.Second].Slime);
        }
    }
}
