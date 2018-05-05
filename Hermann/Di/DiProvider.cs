using Hermann.Contexts;
using Hermann.Models;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using System.Diagnostics;
using Hermann.Environments;

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
        /// コンストラクタ
        /// </summary>
        static DiProvider()
        {
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
                throw new InvalidOperationException("コンテナのインスタンスが生成されていません。");
            }
            return MyContainer;
        }
    }
}