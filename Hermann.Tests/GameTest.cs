using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.TestHelpers;
using Hermann.Tests.Di;

namespace Hermann.Tests
{
    /// <summary>
    /// Gameのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class GameTest
    {
        /// <summary>
        /// ゲーム機能
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameTest()
        {
            TestDiProvider.Register();
            this.Game = new Game();
        }

        /// <summary>
        /// 001:壁際で回転ができない
        /// </summary>
        [TestMethod]
        public void 壁際で回転ができない()
        {
            // 001:1P-右2回
            var context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-001-001.txt");
            this.Game.Update(context);
            this.Game.Update(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-001-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }
    }
}
