using Hermann.Collections;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Collections
{
    /// <summary>
    /// MovableSlimeUnitクラスのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class MovableSlimeUnitTest
    {
        /// <summary>
        /// 001:GetFormのテスト
        /// </summary>
        [TestMethod]
        public void GetFormのテスト()
        {
            var movables = new MovableSlime[MovableSlimeUnit.Length];

            // 1つ目のフィールド単位で横並び
            movables[(int)MovableSlimeUnit.Index.First] = new MovableSlime(Slime.Blue, 0, 6);
            movables[(int)MovableSlimeUnit.Index.Second] = new MovableSlime(Slime.Blue, 0, 7);
            Assert.AreEqual(MovableSlimeUnit.Form.Horizontal, MovableSlimeUnit.GetForm(movables));

            // 1つ目のフィールド単位で縦並び
            movables[(int)MovableSlimeUnit.Index.First] = new MovableSlime(Slime.Blue, 0, 7);
            movables[(int)MovableSlimeUnit.Index.Second] = new MovableSlime(Slime.Blue, 0, 15);
            Assert.AreEqual(MovableSlimeUnit.Form.Vertical, MovableSlimeUnit.GetForm(movables));

            // 1つ目と2つ目のフィールド単位をまたいで縦並び
            movables[(int)MovableSlimeUnit.Index.First] = new MovableSlime(Slime.Blue, 0, 26);
            movables[(int)MovableSlimeUnit.Index.Second] = new MovableSlime(Slime.Blue, 1, 2);
            Assert.AreEqual(MovableSlimeUnit.Form.Vertical, MovableSlimeUnit.GetForm(movables));

            // 2つ目と3つ目のフィールド単位をまたいで縦並び
            movables[(int)MovableSlimeUnit.Index.First] = new MovableSlime(Slime.Blue, 1, 31);
            movables[(int)MovableSlimeUnit.Index.Second] = new MovableSlime(Slime.Blue, 2, 7);
            Assert.AreEqual(MovableSlimeUnit.Form.Vertical, MovableSlimeUnit.GetForm(movables));
        }
    }
}
