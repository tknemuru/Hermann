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
        /// 001:GetPlayerのテストを実行します。
        /// </summary>
        [TestMethod]
        public void GetPlyerのテスト()
        {
            uint command = 0xffff;
            var player = Command.GetPlayer(command);
            Assert.AreEqual(Convert.ToUInt32("0001", 2), player);
        }

        /// <summary>
        /// 002:GetDirectionのテストを実行します。
        /// </summary>
        [TestMethod]
        public void GetDirectionのテスト()
        {
            uint command = 0xffff;
            var direction = Command.GetDirection(command);
            Assert.AreEqual(Convert.ToUInt32("111", 2), (uint)direction);
        }

        /// <summary>
        /// 003:SetPlayerのテストを実行します。
        /// </summary>
        [TestMethod]
        public void SetPlayerのテスト()
        {
            var command = 0u;
            Assert.AreEqual(0u, Command.SetPlayer(command, Player.First));
            Assert.AreEqual(1u, Command.SetPlayer(command, Player.Second));

            command = Convert.ToUInt32("1110", 2);
            Assert.AreEqual(Convert.ToUInt32("1111", 2), Command.SetPlayer(command, Player.Second));
            Assert.AreEqual(command, Command.SetPlayer(command, Player.First));
        }

        /// <summary>
        /// 004:SetDirectionのテストを実行します。
        /// </summary>
        [TestMethod]
        public void SetDirectionのテスト()
        {
            var command = 1u;
            Assert.AreEqual(Convert.ToUInt32("1001", 2), Command.SetDirection(command, Command.Direction.Right));
        }
    }
}
