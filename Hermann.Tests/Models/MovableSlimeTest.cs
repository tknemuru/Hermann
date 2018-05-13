using Hermann.Models;
using Hermann.Contexts;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Models
{
    /// <summary>
    /// MovableSlimeクラスのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class MovableSlimeTest
    {
        /// <summary>
        /// 001:Equalsテスト
        /// </summary>
        [TestMethod]
        public void Equalsテスト()
        {
            // 等しい
            var x = new MovableSlime()
            {
                Slime = Slime.Red,
                Index = 1,
                Position = 4,
            };
            var y = new MovableSlime()
            {
                Slime = Slime.Red,
                Index = 1,
                Position = 4,
            };
            Assert.IsTrue(x.Equals(y));

            // スライムが等しくない
            y = new MovableSlime()
            {
                Slime = Slime.Blue,
                Index = 1,
                Position = 4,
            };
            Assert.IsFalse(x.Equals(y));

            // インデックスが等しくない
            y = new MovableSlime()
            {
                Slime = Slime.Red,
                Index = 2,
                Position = 4,
            };
            Assert.IsFalse(x.Equals(y));

            // ポジションが等しくない
            y = new MovableSlime()
            {
                Slime = Slime.Red,
                Index = 1,
                Position = 5,
            };
            Assert.IsFalse(x.Equals(y));
        }
    }
}
