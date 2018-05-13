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
using Hermann.Initializers;

namespace Hermann.Tests.Di
{
    /// <summary>
    /// テスト用DIの登録機能を提供します。
    /// </summary>
    public static class TestDiRegister
    {
        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        public static void Register()
        {
            DiProvider.GetContainer().Register<ITimeUpdatable>(() => new TimeStableUpdater(0));
            DiProvider.GetContainer().Register<IBuiltRemainingTimeUpdatable>(() => new BuiltRemainingTimeStableUpdater(0, FieldContextConfig.MaxBuiltRemainingTime));
        }
    }
}