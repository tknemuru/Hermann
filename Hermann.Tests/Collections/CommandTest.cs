using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Collections;

namespace Hermann.Tests
{
    /// <summary>
    /// Commandクラスのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class CommandTest
    {
        /// <summary>
        /// 001:マスク量のテストを実行します。
        /// </summary>
        [TestMethod]
        public void Mask量のテスト()
        {
            ulong command = 0xffffffff;

            // プレイヤ
            var player = command & Command.PlayerMask;
            Assert.AreEqual(0x1ul, player);

            // 方向
            var direction = command & Command.DirectionMask;
            Assert.AreEqual(0xeul, direction);
        }

        /// <summary>
        /// 002:方向のテストを実行します。
        /// </summary>
        [TestMethod]
        public void Directionのテスト()
        {
            Assert.AreEqual(Convert.ToUInt64("0000", 2), Command.DirectionNone);
            Assert.AreEqual(Convert.ToUInt64("0010", 2), Command.DirectionUp);
            Assert.AreEqual(Convert.ToUInt64("0100", 2), Command.DirectionDown);
            Assert.AreEqual(Convert.ToUInt64("0110", 2), Command.DirectionLeft);
            Assert.AreEqual(Convert.ToUInt64("1000", 2), Command.DirectionRight);
        }
    }
}
