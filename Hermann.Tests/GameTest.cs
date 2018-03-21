using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hermann.Tests.TestHelpers;
using Hermann.Tests.Di;
using SimpleInjector;
using Hermann.Updaters.Times;
using Hermann.Generators;
using Hermann.Models;

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
        /// 001:壁とスライムに挟まれて回転ができない
        /// </summary>
        [TestMethod]
        public void 壁とスライムに挟まれて回転ができない()
        {
            // 001:1P-右2回
            var context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-001-001.txt");
            this.Game.Update(context);
            this.Game.Update(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-001-001.txt");

            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        ///002:移動・１連鎖・落下
        /// </summary>
        [TestMethod]
        public void 移動１連鎖落下()
        {
            TestDiProvider.Register();
            var container = TestDiProvider.GetContainer();
            container.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(300, 300));
            container.Register<NextSlimeGenerator>(() => new NextSlimeStableGenerator(new[] { Slime.Red, Slime.Blue }));
            container.Register<UsingSlimeGenerator>(() => new UsingSlimeStableGenerator(new[] { Slime.Red, Slime.Blue, Slime.Green, Slime.Purple }));
            container.Verify();
            this.Game = new Game();

            var context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-001.txt");
            this.Game.Update(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-001.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-002.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-002.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-003.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-003.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-004.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-004.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-005.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-005.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-006.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-006.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-007.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-007.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-008.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-008.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-009.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-009.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 010:相手が設置済でなければおじゃまスライムは配置するだけ
            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-010.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-010.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            // 011:相手が設置済であればおじゃまスライムを落とす
            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-011.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-011.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-012.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-012.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-013.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-013.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-002-014.txt");
            this.Game.Update(context);
            expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-002-014.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);

            TestDiProvider.Register();
            TestDiProvider.GetContainer().Verify();
            this.Game = new Game();
        }

        /// <summary>
        /// 003:設置残タイム0で移動して接地を解除する
        /// </summary>
        [TestMethod]
        public void 設置残タイム0で移動して接地を解除する()
        {
            TestDiProvider.Register();
            var container = TestDiProvider.GetContainer();
            container.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(1, 12));
            container.Verify();
            this.Game = new Game();

            var context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-003-001.txt");
            this.Game.Update(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-003-001.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }

        /// <summary>
        /// 004:設置残タイムが0になった時点で連鎖開始される
        /// </summary>
        [TestMethod]
        public void 設置残タイムが0になった時点で連鎖開始される()
        {
            TestDiProvider.Register();
            var container = TestDiProvider.GetContainer();
            container.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(1, 12));
            container.Verify();
            this.Game = new Game();

            var context = TestHelper.Receiver.Receive("../../resources/game/test-field-in-004-001.txt");
            this.Game.Update(context);
            var expectedContext = TestHelper.Receiver.Receive("../../resources/game/test-field-out-004-001.txt");
            TestHelper.AssertEqualsFieldContext(expectedContext, context);
        }
    }
}
