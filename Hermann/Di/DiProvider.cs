using Hermann.Contexts;
using Hermann.Models;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using Hermann.Updaters;
using Hermann.Updaters.Times;

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
            MyContainer.Register<ITimeUpdatable>(() => new TimeElapsedTicksUpdater(), Lifestyle.Singleton);
            MyContainer.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeElapsedTicksUpdater(), Lifestyle.Singleton);

            MyContainer.Options.AllowOverridingRegistrations = true;
            DiProvider.SetContainer(MyContainer);
        }
    }
}
