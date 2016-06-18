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
    }
}
