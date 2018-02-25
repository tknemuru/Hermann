using Hermann.Models;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Models
{
    /// <summary>
    /// MovableSlimeクラスのテスト機能を提供します。
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
            var movables = new MovableSlime[MovableSlime.Length];

            // 1つ目のフィールド単位で横並び
            movables[(int)MovableSlime.UnitIndex.First] = new MovableSlime(Slime.Blue, 0, 6);
            movables[(int)MovableSlime.UnitIndex.Second] = new MovableSlime(Slime.Blue, 0, 7);
            Assert.AreEqual(MovableSlime.UnitForm.Horizontal, MovableSlime.GetUnitForm(movables));

            // 1つ目のフィールド単位で縦並び
            movables[(int)MovableSlime.UnitIndex.First] = new MovableSlime(Slime.Blue, 0, 7);
            movables[(int)MovableSlime.UnitIndex.Second] = new MovableSlime(Slime.Blue, 0, 15);
            Assert.AreEqual(MovableSlime.UnitForm.Vertical, MovableSlime.GetUnitForm(movables));

            // 1つ目と2つ目のフィールド単位をまたいで縦並び
            movables[(int)MovableSlime.UnitIndex.First] = new MovableSlime(Slime.Blue, 0, 26);
            movables[(int)MovableSlime.UnitIndex.Second] = new MovableSlime(Slime.Blue, 1, 2);
            Assert.AreEqual(MovableSlime.UnitForm.Vertical, MovableSlime.GetUnitForm(movables));

            // 2つ目と3つ目のフィールド単位をまたいで縦並び
            movables[(int)MovableSlime.UnitIndex.First] = new MovableSlime(Slime.Blue, 1, 31);
            movables[(int)MovableSlime.UnitIndex.Second] = new MovableSlime(Slime.Blue, 2, 7);
            Assert.AreEqual(MovableSlime.UnitForm.Vertical, MovableSlime.GetUnitForm(movables));
        }
    }
}
