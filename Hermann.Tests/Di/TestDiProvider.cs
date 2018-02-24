using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Api.Commands;
using Hermann.Contexts;
using Hermann.Models;
using Hermann.Di;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleInjector;
using Hermann.Updaters.Times;
using Hermann.Updaters;

namespace Hermann.Tests.Di
{
    /// <summary>
    /// テスト用DIの生成機能を提供します。
    /// </summary>
    public static class TestDiProvider
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        private static Container MyContainer { get; set; }

        /// <summary>
        /// DIコンテナをセットします。
        /// </summary>
        /// <param name="container">DIコンテナ</param>
        public static void SetContainer(Container container)
        {
            MyContainer = container;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static TestDiProvider()
        {
            Register();
            MyContainer.Verify();
        }

        /// <summary>
        /// DIコンテナを取得します。
        /// </summary>
        /// <returns></returns>
        public static Container GetContainer()
        {
            if (MyContainer == null)
            {
                Register();
                MyContainer.Verify();
            }
            return MyContainer;
        }

        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        public static void Register()
        {
            MyContainer = new Container();
            MyContainer.Options.AllowOverridingRegistrations = true;
            MyContainer.Register<FieldContext, FieldContext>();
            MyContainer.Register<MovableSlime>(() => new MovableSlime());
            MyContainer.Register<UsingSlimeGenerator, UsingSlimeRandomGenerator>();
            MyContainer.Register<NextSlimeGenerator, NextSlimeRandomGenerator>();
            MyContainer.Register<NextSlimeUpdater>(() => new NextSlimeUpdater());
            MyContainer.Register<CommandReceiver<NativeCommand, FieldContext>, NativeCommandReceiver>();
            MyContainer.Register<FieldContextReceiver<string>, SimpleTextReceiver>();
            MyContainer.Register<FieldContextSender<string>, SimpleTextSender>();
            MyContainer.Register<ITimeUpdatable>(() => new TimeStableUpdater(0));
            MyContainer.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(0));

            DiProvider.SetContainer(MyContainer);
        }
    }
}