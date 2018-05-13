using Hermann.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hermann.Analyzers;
using Hermann.Ai.Evaluators;
using Hermann.Models;
using Hermann.Ai.Generators;
using Hermann.Ai.Models;
using Hermann.Ai.Providers;
using Hermann.Generators;
using Hermann.Updaters.Times;
using Hermann.Updaters;
using Hermann.Contexts;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Api.Commands;
using Hermann.Api.Containers;

namespace Hermann.Ai.Di
{
    /// <summary>
    /// DIの登録機能を提供します。
    /// </summary>
    public static class ApiDiRegister
    {
        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        public static void Register()
        {
            DiProvider.GetContainer().Register<NativeCommandReceiver>(() => new NativeCommandReceiver(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<SimpleTextReceiver>(() => new SimpleTextReceiver(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<SimpleTextSender>(() => new SimpleTextSender(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<NativeCommand>(() => new NativeCommand());
            DiProvider.GetContainer().Register<UiDecorationContainer>(() => new UiDecorationContainer());
            DiProvider.GetContainer().Register<UiDecorationContainerReceiver>(() => new UiDecorationContainerReceiver());

        }
    }
}