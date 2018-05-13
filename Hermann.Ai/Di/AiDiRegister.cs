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
using Hermann.Ai.Updaters;
using Hermann.Ai.Serchers;
using Hermann.Ai.Analyzers;

namespace Hermann.Ai.Di
{
    /// <summary>
    /// DIの登録機能を提供します。
    /// </summary>
    public static class AiDiRegister
    {
        /// <summary>
        /// 依存性の登録を行います。
        /// </summary>
        public static void Register()
        {
            DiProvider.GetContainer().Register<LearnerManager>(() => new LearnerManager(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<MergedFieldsGenerator>(() => new MergedFieldsGenerator(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<PatternProvider>(() => new PatternProvider(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<InputDataProvider>(() => new InputDataProvider(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<EvalProvider>(() => new EvalProvider(), Lifestyle.Singleton);
            DiProvider.GetContainer().Register<AiPlayer>(() => new AiPlayer());
            DiProvider.GetContainer().Register<AutoMoveAndDropUpdater>(() => new AutoMoveAndDropUpdater());
            DiProvider.GetContainer().Register<ThinCompleteReading>(() => new ThinCompleteReading());
            DiProvider.GetContainer().Register<ErasedPotentialSlimeAnalyzer>(() => new ErasedPotentialSlimeAnalyzer());
            DiProvider.GetContainer().Register<DifferenceHeightAnalyzer>(() => new DifferenceHeightAnalyzer());
            DiProvider.GetContainer().Register<ChainAnalyzer>(() => new ChainAnalyzer());
            DiProvider.GetContainer().Register<MergedFieldsGenerator>(() => new MergedFieldsGenerator());
            DiProvider.GetContainer().Register<LinearRegressionEvaluator>(() => new LinearRegressionEvaluator());
            DiProvider.GetContainer().Register<PatternGenerator>(() => new PatternGenerator());
            DiProvider.GetContainer().Register<PatternGenerator.Config>(() => new PatternGenerator.Config());
            DiProvider.GetContainer().Register<FieldFeatureGenerator>(() => new FieldFeatureGenerator());
            DiProvider.GetContainer().Register<FieldFeatureGenerator.Config>(() => new FieldFeatureGenerator.Config());
            DiProvider.GetContainer().Register<RandomEvaluator>(() => new RandomEvaluator());
            DiProvider.GetContainer().Register<AliveEvaluator>(() => new AliveEvaluator());
        }
    }
}