using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

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
        public static Container MyContainer { get; private set; }

        /// <summary>
        /// DIコンテナを取得します。
        /// </summary>
        /// <returns></returns>
        public static Container GetContainer()
        {
            if (MyContainer == null)
            {
                MyContainer = new Container();
            }
            return MyContainer;
        }
    }
}
