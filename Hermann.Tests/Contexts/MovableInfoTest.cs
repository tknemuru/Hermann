using Hermann.Collections;
using Hermann.Contexts;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Contexts
{
    /// <summary>
    /// MovableInfoクラスのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class MovableInfoTest
    {
        /// <summary>
        /// 001:Equalsテスト
        /// </summary>
        [TestMethod]
        public void Equalsテスト()
        {
            // 等しい
            var x = new MovableInfo(Slime.Red, 1, 4);
            var y = new MovableInfo(Slime.Red, 1, 4);
            Assert.IsTrue(x.Equals(y));

            // スライムが等しくない
            y = new MovableInfo(Slime.Blue, 1, 4);
            Assert.IsFalse(x.Equals(y));

            // インデックスが等しくない
            y = new MovableInfo(Slime.Red, 2, 4);
            Assert.IsFalse(x.Equals(y));

            // ポジションが等しくない
            y = new MovableInfo(Slime.Red, 1, 5);
            Assert.IsFalse(x.Equals(y));
        }
    }
}
