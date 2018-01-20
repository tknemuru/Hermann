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
using System.Threading.Tasks;
using SimpleInjector;
using Hermann.Updaters.Times;

namespace Hermann.Client.ConsoleClient.Di
{
    /// <summary>
    /// DIの生成機能を提供します。
    /// </summary>
    public static class ConsoleClientDiProvider
    {
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
            container.Register<CommandReceiver<NativeCommand, FieldContext>, NativeCommandReceiver>(Lifestyle.Singleton);
            container.Register<FieldContextReceiver<string>, SimpleTextReceiver>(Lifestyle.Singleton);
            container.Register<FieldContextSender<string>, SimpleTextSender>(Lifestyle.Singleton);
            container.Verify();
        }
    }
}
