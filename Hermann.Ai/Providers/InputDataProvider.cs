using Accord.Math;
using Hermann.Ai.Generators;
using Hermann.Ai.Models;
using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Ai.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Di;

namespace Hermann.Ai.Providers
{
    /// <summary>
    /// 入力情報の提供機能を提供します。
    /// </summary>
    public sealed class InputDataProvider
    {
        /// <summary>
        /// ベクトル生成機能群
        /// </summary>
        private Dictionary<AiPlayer.Version, IEnumerable<IGeneratable<FieldContext, SparseVector<double>>>> VectorGens { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputDataProvider()
        {
            this.VectorGens = BuildVectorGens();
        }

        /// <summary>
        /// 指定したバージョンのベクトルを取得します。
        /// </summary>
        /// <param name="version">ベクトルバージョン</param>
        /// <param name="context">フィールド状態</param>
        /// <returns>ベクトル</returns>
        public SparseVector<double> GetVector(AiPlayer.Version version, FieldContext context)
        {
            if (!this.VectorGens.ContainsKey(version))
            {
                throw new ArgumentException("ベクトル種別が不正です");
            }

            var vector = this.GetVector(this.VectorGens[version], context);
            return vector;
        }

        /// <summary>
        /// ベクトル生成機能群を組み立てます。
        /// </summary>
        /// <returns>ベクトル生成機能群</returns>
        private Dictionary<AiPlayer.Version, IEnumerable<IGeneratable<FieldContext, SparseVector<double>>>> BuildVectorGens()
        {
            var gens = new Dictionary<AiPlayer.Version, IEnumerable<IGeneratable<FieldContext, SparseVector<double>>>>();

            // V1.0
            var patternGen = DiProvider.GetContainer().GetInstance<PatternGenerator>();
            var patternConfig = DiProvider.GetContainer().GetInstance<PatternGenerator.Config>();
            patternConfig.Patterns = new[] {
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatFarLeft),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatLeft),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeLowerLeft),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsOneLeft),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsTwoLeft),
            };
            patternGen.Inject(patternConfig);
            var featureGen = DiProvider.GetContainer().GetInstance<FieldFeatureGenerator>();
            var featureConfig = DiProvider.GetContainer().GetInstance<FieldFeatureGenerator.Config>();
            featureConfig.TargetFeatue[FieldFeatureGenerator.Feature.NoticeObstruction] = true;
            featureGen.Injection(featureConfig);
            gens.Add(AiPlayer.Version.V1_0, new IGeneratable<FieldContext, SparseVector<double>>[] {
                    patternGen,
                    featureGen
                });

            // V2.0
            patternGen = DiProvider.GetContainer().GetInstance<PatternGenerator>();
            patternConfig = DiProvider.GetContainer().GetInstance<PatternGenerator.Config>();
            patternConfig.Patterns = new[] {
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatFarLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatFarRight),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatRight),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeLowerLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeLowerRight),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeUpperLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeUpperRight),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsOneLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsOneRight),
                DiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsTwoLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsTwoRight),
            };
            //patternConfig.SparseValue = -1.0d;
            patternConfig.BothPlayer = false;
            //patternConfig.ContainsObstructionSlime = false;
            patternGen.Inject(patternConfig);
            featureGen = DiProvider.GetContainer().GetInstance<FieldFeatureGenerator>();
            featureConfig = DiProvider.GetContainer().GetInstance<FieldFeatureGenerator.Config>();
            featureConfig.TargetFeatue[FieldFeatureGenerator.Feature.NoticeObstruction] = true;
            featureConfig.TargetFeatue[FieldFeatureGenerator.Feature.Chain] = true;
            //featureConfig.TargetFeatue[FieldFeatureGenerator.Feature.ErasedPotentialCount] = true;
            featureConfig.BothPlayer = false;
            featureGen.Injection(featureConfig);
            gens.Add(AiPlayer.Version.V2_0, new IGeneratable<FieldContext, SparseVector<double>>[] {
                    patternGen,
                    featureGen
                });
            return gens;
        }

        /// <summary>
        /// ベクトルを取得します。
        /// </summary>
        /// <param name="gens">ベクトル生成機能群</param>
        /// <param name="context">フィールド状態</param>
        /// <returns>ベクトル</returns>
        private SparseVector<double> GetVector(IEnumerable<IGeneratable<FieldContext, SparseVector<double>>> gens, FieldContext context)
        {
            var vectors = new List<SparseVector<double>>();
            foreach(var gen in gens)
            {
                vectors.Add(gen.GetNext(context));
            }
            return this.MergeVector(vectors);
        }

        /// <summary>
        /// ベクトルをマージします。
        /// </summary>
        /// <param name="vectors">ベクトルリスト</param>
        /// <returns>マージされたベクトル</returns>
        private SparseVector<double> MergeVector(IEnumerable<SparseVector<double>> vectors)
        {
            var mergedVector = vectors.First();
            foreach(var vector in vectors.Skip(1))
            {
                mergedVector.Add(vector);
            }
            return mergedVector;
        }
    }
}
