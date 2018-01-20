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
using System.Threading.Tasks;
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
            }
            return MyContainer;
        }

        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        private static void Register()
        {
            MyContainer = new Container();
            MyContainer.Register<FieldContext, FieldContext>();
            MyContainer.Register<MovableSlime>(() => new MovableSlime());
            MyContainer.Register<UsingSlimeGenerator, UsingSlimeRandomGenerator>();
            MyContainer.Register<NextSlimeGenerator, NextSlimeRandomGenerator>();
            MyContainer.Register<NextSlimeUpdater>();
            MyContainer.Register<CommandReceiver<NativeCommand, FieldContext>, NativeCommandReceiver>(Lifestyle.Singleton);
            MyContainer.Register<FieldContextReceiver<string>, SimpleTextReceiver>(Lifestyle.Singleton);
            MyContainer.Register<FieldContextSender<string>, SimpleTextSender>(Lifestyle.Singleton);
            MyContainer.Register<ITimeUpdatable>(() => new TimeStableUpdater(0), Lifestyle.Singleton);
            MyContainer.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(0), Lifestyle.Singleton);

            DiProvider.SetContainer(MyContainer);
            MyContainer.Verify();
        }
    }
}
