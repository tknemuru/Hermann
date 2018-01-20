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
            return DiProvider.GetContainer();
        }

        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        private static void Register()
        {
            var container = DiProvider.GetContainer();
            container.Register<ITimeUpdatable>(() => new TimeStableUpdater(0), Lifestyle.Singleton);
            container.Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(0), Lifestyle.Singleton);
            container.Verify();
        }
    }
}
