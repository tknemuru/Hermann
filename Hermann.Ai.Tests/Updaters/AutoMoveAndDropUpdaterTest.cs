using System;
using Hermann.Ai.Analyzers;
using Hermann.Ai.Di;
using Hermann.Ai.Updaters;
using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Models;
using Hermann.Tests.Di;
using Hermann.Tests.TestHelpers;
using Hermann.Updaters;
using Hermann.Updaters.Times;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Hermann.Ai.Test.Analyzers
{
    /// <summary>
    /// AutoMoveAndDropUpdaterのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class AutoMoveAndDropUpdaterTest
    {
        /// <summary>
        /// 自動移動機能
        /// </summary>
        private AutoMoveAndDropUpdater Updater { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AutoMoveAndDropUpdaterTest()
        {
            this.Updater = new AutoMoveAndDropUpdater();
        }

        /// <summary>
        /// 001:自動移動が正しく行われる
        /// </summary>
        [TestMethod]
        public void 自動移動が正しく行われる()
        {
            var container = new Container();
            container.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(0, 12));
            container.Register<NextSlimeGenerator>(() => new NextSlimeStableGenerator(new[] { Slime.Red, Slime.Blue }));
            container.Register(() => new MovableSlime());
            container.Register<ITimeUpdatable>(() => new TimeStableUpdater(0));
            container.Register<ObstructionSlimeSetter, ObstructionSlimeRandomSetter>();
            container.Verify();
            DiProvider.SetContainer(container);
            AiDiProvider.SetContainer(container);
            TestDiProvider.SetContainer(container);

            var param = new AutoMoveAndDropUpdater.Param()
            {
                Pattern = new[] { Direction.Left, Direction.Left },
            };

            var actual = TestHelper.Receiver.Receive("../../resources/updaters/automoveanddropupdater/test-field-in-001-001.txt");
            this.Updater.Inject(actual.UsingSlimes);
            this.Updater.Update(actual, actual.OperationPlayer, param);
            var expected = TestHelper.Receiver.Receive("../../resources/updaters/automoveanddropupdater/test-field-out-001-001.txt");
            TestHelper.AssertEqualsFieldContext(expected, actual);
            Assert.AreEqual(5, param.Chain);
        }
    }
}
