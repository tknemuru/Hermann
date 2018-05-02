using Hermann.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using Hermann.Analyzers;
using Hermann.Ai.Evaluators;
using Hermann.Models;
using Hermann.Ai.Generators;
using Hermann.Ai.Models;
using Hermann.Ai.Providers;

namespace Hermann.Learning.Di
{
    /// <summary>
    /// DIの生成機能を提供します。
    /// </summary>
    public static class AiDiProvider
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
        static AiDiProvider()
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
        private static void Register()
        {
            MyContainer = new Container();
            MyContainer.Register(() => new MovableSlime());
            MyContainer.Register<LearnerManager>(Lifestyle.Singleton);
            MyContainer.Register<MovableDirectionAnalyzer>(Lifestyle.Singleton);
            MyContainer.Register<MergedFieldsGenerator>(Lifestyle.Singleton);
            MyContainer.Register<PatternProvider>(Lifestyle.Singleton);
            MyContainer.Register<PatternGenerator>(Lifestyle.Singleton);
            MyContainer.Register<FieldFeatureGenerator>(Lifestyle.Singleton);
            MyContainer.Register<InputDataProvider>(Lifestyle.Singleton);
            MyContainer.Register<EvalProvider>(Lifestyle.Singleton);
            MyContainer.Register<AliveEvaluator>(Lifestyle.Singleton);

            DiProvider.SetContainer(MyContainer);
        }
    }
}