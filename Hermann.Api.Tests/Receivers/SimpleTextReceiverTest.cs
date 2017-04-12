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

            // 経過時間
            Assert.AreEqual(200, context.Time);

            // 接地
            CollectionAssert.AreEqual(new[] { false, true }, context.Ground);

            // 設置残タイム
            CollectionAssert.AreEqual(new[] { 260, 90 }, context.BuiltRemainingTime);

            // 得点
            CollectionAssert.AreEqual(new[] { 3000L, 2000L }, context.Score);

            // 連鎖
            CollectionAssert.AreEqual(new[] { 3, 2 }, context.Chain);

            // 相殺
            CollectionAssert.AreEqual(new[] { true, false }, context.Offset);

            // 全消
            CollectionAssert.AreEqual(new[] { true, false }, context.AllErase);

            // 勝数
            CollectionAssert.AreEqual(new[] { 10, 9 }, context.WinCount);

            // 使用スライム
            CollectionAssert.AreEqual(new[] { Slime.Red, Slime.Blue, Slime.Green, Slime.Purple }, context.UsingSlimes);

            // おじゃまスライム
            Assert.AreEqual(2, context.ObstructionSlimes[Player.First][ObstructionSlime.Small]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.First][ObstructionSlime.Big]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.First][ObstructionSlime.Rock]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.First][ObstructionSlime.Star]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.First][ObstructionSlime.Moon]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.First][ObstructionSlime.Crown]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.First][ObstructionSlime.Comet]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Small]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Big]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Rock]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Star]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Moon]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Crown]);
            Assert.AreEqual(1, context.ObstructionSlimes[Player.Second][ObstructionSlime.Comet]);

            // フィールド
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

            // NEXTスライム
            // 1P:1つめ
            Assert.AreEqual(Slime.Green, context.NextSlimes[Player.First][(int)NextSlime.Index.First][(int)MovableSlime.UnitIndex.First]);
            Assert.AreEqual(Slime.Blue, context.NextSlimes[Player.First][(int)NextSlime.Index.First][(int)MovableSlime.UnitIndex.Second]);

            // 1P:2つめ
            Assert.AreEqual(Slime.Yellow, context.NextSlimes[Player.First][(int)NextSlime.Index.Second][(int)MovableSlime.UnitIndex.First]);
            Assert.AreEqual(Slime.Red, context.NextSlimes[Player.First][(int)NextSlime.Index.Second][(int)MovableSlime.UnitIndex.Second]);

            // 2P:1つめ
            Assert.AreEqual(Slime.Purple, context.NextSlimes[Player.Second][(int)NextSlime.Index.First][(int)MovableSlime.UnitIndex.First]);
            Assert.AreEqual(Slime.Red, context.NextSlimes[Player.Second][(int)NextSlime.Index.First][(int)MovableSlime.UnitIndex.Second]);

            // 2P:2つめ
            Assert.AreEqual(Slime.Blue, context.NextSlimes[Player.Second][(int)NextSlime.Index.Second][(int)MovableSlime.UnitIndex.First]);
            Assert.AreEqual(Slime.Green, context.NextSlimes[Player.Second][(int)NextSlime.Index.Second][(int)MovableSlime.UnitIndex.Second]);
        }
    }
}
