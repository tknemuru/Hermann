using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Helpers;
using System.Collections.Generic;
using System.Linq;
using Hermann.Models;

namespace Hermann.Tests.Helpers
{
    /// <summary>
    /// ObstructionSlimeHelperのテスト実行機能を提供します。
    /// </summary>
    [TestClass]
    public class ObstructionSlimeHelperTest
    {
        /// <summary>
        /// 001:ScoreToCountテスト
        /// </summary>
        [TestMethod]
        public void ScoreToCountテスト()
        {
            // 001:0点
            var actual = ObstructionSlimeHelper.ScoreToCount(0);
            Assert.AreEqual(0, actual);

            // 002:割り切れる
            actual = ObstructionSlimeHelper.ScoreToCount(210);
            Assert.AreEqual(3, actual);

            // 003:割り切れない
            actual = ObstructionSlimeHelper.ScoreToCount(260);
            Assert.AreEqual(3, actual);
        }

        /// <summary>
        /// 002:CountToObstructionsテスト
        /// </summary>
        [TestMethod]
        public void CountToObstructionsテスト()
        {
            // 001:0個
            var expected = CreateInitialObstructionSlimes();
            var actual = ObstructionSlimeHelper.CountToObstructions(0);
            CollectionAssert.AreEqual(expected, actual);

            // 002:小スライム1個
            expected = CreateInitialObstructionSlimes();
            expected[ObstructionSlime.Small] = 1;
            actual = ObstructionSlimeHelper.CountToObstructions(1);
            CollectionAssert.AreEqual(expected, actual);

            // 003:小スライム5個
            expected = CreateInitialObstructionSlimes();
            expected[ObstructionSlime.Small] = 5;
            actual = ObstructionSlimeHelper.CountToObstructions(5);
            CollectionAssert.AreEqual(expected, actual);

            // 004:王冠スライム1個・月スライム1個・星スライム1個・岩スライム5個・大スライム4個・小スライム5個
            expected = CreateInitialObstructionSlimes();
            expected[ObstructionSlime.Crown] = 1;
            expected[ObstructionSlime.Moon] = 1;
            expected[ObstructionSlime.Star] = 1;
            expected[ObstructionSlime.Rock] = 5;
            expected[ObstructionSlime.Big] = 4;
            expected[ObstructionSlime.Small] = 5;
            actual = ObstructionSlimeHelper.CountToObstructions(1439);
            CollectionAssert.AreEqual(expected, actual);

            // 005:彗星スライム1個
            expected = CreateInitialObstructionSlimes();
            expected[ObstructionSlime.Comet] = 1;
            actual = ObstructionSlimeHelper.CountToObstructions(1440);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// 004:ObstructionsToCountテスト
        /// </summary>
        [TestMethod]
        public void ObstructionsToCountテスト()
        {
            // 001:0個
            var obs = CreateInitialObstructionSlimes();
            var actual = ObstructionSlimeHelper.ObstructionsToCount(obs);
            Assert.AreEqual(0, actual);

            // 002:小スライム1個
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Small] = 1;
            actual = ObstructionSlimeHelper.ObstructionsToCount(obs);
            Assert.AreEqual(1, actual);

            // 004:王冠スライム1個・月スライム1個・星スライム1個・岩スライム5個・大スライム4個・小スライム5個
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Crown] = 1;
            obs[ObstructionSlime.Moon] = 1;
            obs[ObstructionSlime.Star] = 1;
            obs[ObstructionSlime.Rock] = 5;
            obs[ObstructionSlime.Big] = 4;
            obs[ObstructionSlime.Small] = 5;
            actual = ObstructionSlimeHelper.ObstructionsToCount(obs);
            Assert.AreEqual(1439, actual);

            // 005:彗星スライム1個
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Comet] = 1;
            actual = ObstructionSlimeHelper.ObstructionsToCount(obs);
            Assert.AreEqual(1440, actual);
        }

        /// <summary>
        /// 005:GetScoreRemainderテスト
        /// </summary>
        [TestMethod]
        public void GetScoreRemainderテスト()
        {
            // 001:0点
            var actual = ObstructionSlimeHelper.GetScoreRemainder(0);
            Assert.AreEqual(0, actual);

            // 002:割り切れる
            actual = ObstructionSlimeHelper.GetScoreRemainder(140);
            Assert.AreEqual(0, actual);

            // 003:割り切れない
            actual = ObstructionSlimeHelper.GetScoreRemainder(170);
            Assert.AreEqual(30, actual);
        }

        /// <summary>
        /// 006:ExistsObstructionSlimeテスト
        /// </summary>
        [TestMethod]
        public void ExistsObstructionSlimeテスト()
        {
            // 001:存在しない
            var obs = CreateInitialObstructionSlimes();
            var actual = ObstructionSlimeHelper.ExistsObstructionSlime(obs);
            Assert.IsFalse(actual);

            // 002:存在する
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Star] = 1;
            actual = ObstructionSlimeHelper.ExistsObstructionSlime(obs);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// 007:ObstructionsToScoreテスト
        /// </summary>
        [TestMethod]
        public void ObstructionsToScoreテスト()
        {
            // 001:0個
            var obs = CreateInitialObstructionSlimes();
            var actual = ObstructionSlimeHelper.ObstructionsToScore(obs);
            Assert.AreEqual(0, actual);

            // 002:小スライム1個
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Small] = 1;
            actual = ObstructionSlimeHelper.ObstructionsToScore(obs);
            Assert.AreEqual(ObstructionSlimeHelper.Rate * 1, actual);

            // 004:王冠スライム1個・月スライム1個・星スライム1個・岩スライム5個・大スライム4個・小スライム5個
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Crown] = 1;
            obs[ObstructionSlime.Moon] = 1;
            obs[ObstructionSlime.Star] = 1;
            obs[ObstructionSlime.Rock] = 5;
            obs[ObstructionSlime.Big] = 4;
            obs[ObstructionSlime.Small] = 5;
            actual = ObstructionSlimeHelper.ObstructionsToScore(obs);
            Assert.AreEqual(ObstructionSlimeHelper.Rate * 1439, actual);

            // 005:彗星スライム1個
            obs = CreateInitialObstructionSlimes();
            obs[ObstructionSlime.Comet] = 1;
            actual = ObstructionSlimeHelper.ObstructionsToScore(obs);
            Assert.AreEqual(ObstructionSlimeHelper.Rate * 1440, actual);
        }

        /// <summary>
        /// 初期状態のおじゃまスライムの集合を作成します。
        /// </summary>
        /// <returns>初期状態のおじゃまスライムの集合</returns>
        private static Dictionary<ObstructionSlime, int> CreateInitialObstructionSlimes()
        {
            var ret = new Dictionary<ObstructionSlime, int>();
            var obs = ((IEnumerable<ObstructionSlime>)Enum.GetValues(typeof(ObstructionSlime))).OrderByDescending(o => o);
            foreach (var o in obs)
            {
                ret.Add(o, 0);
            }
            return ret;
        }
    }
}
