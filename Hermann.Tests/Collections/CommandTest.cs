using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Contexts;
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
            uint command = 0xffff;

            // プレイヤ
            var player = Command.GetPlyer(command);
            Assert.AreEqual(0x1u, player);

            // 方向
            var direction = Command.GetDirection(command);
            Assert.AreEqual(0xeu, (uint)direction);
        }
    }
}
