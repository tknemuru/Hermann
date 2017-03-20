using Hermann.Collections;
using Hermann.Contexts;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Collections
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
            var x = new MovableSlime(Slime.Red, 1, 4);
            var y = new MovableSlime(Slime.Red, 1, 4);
            Assert.IsTrue(x.Equals(y));

            // スライムが等しくない
            y = new MovableSlime(Slime.Blue, 1, 4);
            Assert.IsFalse(x.Equals(y));

            // インデックスが等しくない
            y = new MovableSlime(Slime.Red, 2, 4);
            Assert.IsFalse(x.Equals(y));

            // ポジションが等しくない
            y = new MovableSlime(Slime.Red, 1, 5);
            Assert.IsFalse(x.Equals(y));
        }
    }
}
