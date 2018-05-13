using Hermann.Contexts;
using Hermann.Models;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Hermann.Environments;
using Hermann.Updaters.Times;
using Hermann.Updaters;
using Hermann.Initializers;
using Hermann.Analyzers;

namespace Hermann.Di
{
    /// <summary>
    /// DIの生成機能を提供します。
    /// </summary>
    public static class DiProvider
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        private static SimpleContainer MyContainer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static DiProvider()
        {
            MyContainer = new SimpleContainer();
            Register();
#if DEBUG
            if (EnvConfig.GetPlatform() == PlatformID.Unix)
            {
                Debug.Listeners.Add(new DebugListener());
            }
#endif
        }

        /// <summary>
        /// DIコンテナをセットします。
        /// </summary>
        /// <param name="container">DIコンテナ</param>
        public static void SetContainer(SimpleContainer container)
        {
            MyContainer = container;
        }

        /// <summary>
        /// DIコンテナを取得します。
        /// </summary>
        /// <returns></returns>
        public static SimpleContainer GetContainer()
        {
            if (MyContainer == null)
            {
                throw new InvalidOperationException("コンテナのインスタンスが生成されていません。");
            }
            return MyContainer;
        }

        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        private static void Register()
        {
            MyContainer.Register<UsingSlimeGenerator>(() => new UsingSlimeRandomGenerator());
            MyContainer.Register<NextSlimeGenerator>(() => new NextSlimeRandomGenerator());
            MyContainer.Register<ITimeUpdatable>(() => new TimeElapsedTicksUpdater());
            MyContainer.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(1, FieldContextConfig.MaxBuiltRemainingFrameCount));
            MyContainer.Register<ObstructionSlimeSetter>(() => new ObstructionSlimeRandomSetter());
            MyContainer.Register<Game>(() => new Game());
            MyContainer.Register<SlimeMover>(() => new SlimeMover());
            MyContainer.Register<GroundUpdater>(() => new GroundUpdater());
            MyContainer.Register<MovableSlimesUpdater>(() => new MovableSlimesUpdater());
            MyContainer.Register<SlimeErasingMarker>(() => new SlimeErasingMarker());
            MyContainer.Register<WinCountUpdater>(() => new WinCountUpdater());
            MyContainer.Register<SlimeEraser>(() => new SlimeEraser());
            MyContainer.Register<Gravity>(() => new Gravity());
            MyContainer.Register<RotationDirectionUpdater>(() => new RotationDirectionUpdater());
            MyContainer.Register<ObstructionSlimeCalculator>(() => new ObstructionSlimeCalculator());
            MyContainer.Register<ScoreCalculator>(() => new ScoreCalculator());
            MyContainer.Register<ObstructionSlimeErasingMarker>(() => new ObstructionSlimeErasingMarker());
            MyContainer.Register<NextSlimeUpdater>(() => new NextSlimeUpdater());
            MyContainer.Register<FieldContextInitializer>(() => new FieldContextInitializer());
            MyContainer.Register<FieldAnalyzer>(() => new FieldAnalyzer());
            MyContainer.Register<MovableDirectionAnalyzer>(() => new MovableDirectionAnalyzer());
            MyContainer.Register<SlimeJoinStateAnalyzer>(() => new SlimeJoinStateAnalyzer());
         }
    }
}