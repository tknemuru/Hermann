using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Api.Commands;
using Hermann.Contexts;
using Hermann.Models;
using Hermann.Di;
using Hermann.Generators;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hermann.Updaters.Times;
using Hermann.Initializers;

namespace Hermann.Client.ConsoleClient.Di
{
    /// <summary>
    /// DIの生成機能を提供します。
    /// </summary>
    public static class ConsoleClientDiProvider
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        private static SimpleContainer MyContainer { get; set; }

        /// <summary>
        /// DIコンテナをセットします。
        /// </summary>
        /// <param name="container">DIコンテナ</param>
        public static void SetContainer(SimpleContainer container)
        {
            MyContainer = container;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ConsoleClientDiProvider()
        {
            Register();
        }

        /// <summary>
        /// DIコンテナを取得します。
        /// </summary>
        /// <returns></returns>
        public static SimpleContainer GetContainer()
        {
            if (MyContainer == null)
            {
                Register();
            }
            return MyContainer;
        }

        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        private static void Register()
        {
            MyContainer = new SimpleContainer();
            MyContainer.Register<FieldContext>(() => new FieldContext());
            MyContainer.Register(() => new MovableSlime());
            MyContainer.Register<UsingSlimeGenerator>(() => new UsingSlimeRandomGenerator());
            MyContainer.Register<NextSlimeGenerator>(() => new NextSlimeRandomGenerator());
            MyContainer.Register(() => new NextSlimeUpdater());
            //MyContainer.Register<CommandReceiver<NativeCommand, FieldContext>, NativeCommandReceiver>(Lifestyle.Singleton);
            //MyContainer.Register<FieldContextReceiver<string>, SimpleTextReceiver>(Lifestyle.Singleton);
            //MyContainer.Register<FieldContextSender<string>, SimpleTextSender>(Lifestyle.Singleton);
            MyContainer.Register<ITimeUpdatable>(() => new TimeElapsedTicksUpdater());
            MyContainer.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(1, 3));
            //MyContainer.Register<IFieldContextInitializable>(() => new FieldContextInitializer(), Lifestyle.Singleton);
            MyContainer.Register<ObstructionSlimeSetter>(() => new ObstructionSlimeRandomSetter());

            DiProvider.SetContainer(MyContainer);
            //MyContainer.Verify();
        }
    }
}