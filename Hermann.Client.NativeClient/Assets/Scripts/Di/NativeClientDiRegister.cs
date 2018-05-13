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
using Assets.Scripts.Initializers;
using Assets.Scripts.Updater;
using Assets.Scripts.Analyzers;
using Assets.Scripts.Containers;
using Hermann.Ai.Di;

namespace Assets.Scripts.Di
{
    /// <summary>
    /// DIの登録機能を提供します。
    /// </summary>
    public static class NativeClientDiRegister
    {
        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        public static void Register()
        {
            ApiDiRegister.Register();
            AiDiRegister.Register();
            DiProvider.GetContainer().Register<InputManager>(() => new InputManager(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<SoundEffectOutputter>(() => new SoundEffectOutputter());
            DiProvider.GetContainer().Register<SoundEffectAnalyzer>(() => new SoundEffectAnalyzer());
            DiProvider.GetContainer().Register<SoundEffectDecorationContainer>(() => new SoundEffectDecorationContainer());
            DiProvider.GetContainer().Register<FieldContextSimpleTextInitializer>(() => new FieldContextSimpleTextInitializer());
        }
    }
}