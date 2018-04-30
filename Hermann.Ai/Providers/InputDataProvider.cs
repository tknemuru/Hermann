using Accord.Math;
using Hermann.Ai.Generators;
using Hermann.Ai.Models;
using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Learning.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Dictionary<Vector, IEnumerable<IGeneratable<FieldContext, SparseVector<double>>>> VectorGens { get; set; }

        /// <summary>
        /// ベクトル形式の入力情報種別
        /// </summary>
        public enum Vector
        {
            Main,
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputDataProvider()
        {
            this.VectorGens = BuildVectorGens();
        }

        /// <summary>
        /// 指定した種別のベクトルを取得します。
        /// </summary>
        /// <param name="type">ベクトル種別</param>
        /// <param name="context">フィールド状態</param>
        /// <returns>ベクトル</returns>
        public SparseVector<double> GetVector(Vector type, FieldContext context)
        {
            SparseVector<double> vector = null;
            switch (type)
            {
                case Vector.Main:
                    vector = this.GetVector(this.VectorGens[type], context);
                    break;
                default:
                    throw new ArgumentException("ベクトル種別が不正です");
            }
            return vector;
        }

        /// <summary>
        /// ベクトル生成機能群を組み立てます。
        /// </summary>
        /// <returns>ベクトル生成機能群</returns>
        private Dictionary<Vector, IEnumerable<IGeneratable<FieldContext, SparseVector<double>>>> BuildVectorGens()
        {
            var gens = new Dictionary<Vector, IEnumerable<IGeneratable<FieldContext, SparseVector<double>>>>();
            var patternGen = AiDiProvider.GetContainer().GetInstance<PatternGenerator>();
            patternGen.Injection(new[] {
                AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatFarLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatFarRight),
                AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.FloatRight),
                AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeLowerLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeLowerRight),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeUpperLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.InterposeUpperRight),
                AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsOneLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsOneRight),
                AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsTwoLeft),
                //AiDiProvider.GetContainer().GetInstance<PatternProvider>().Get(Pattern.StairsTwoRight),
                });
            var featureGen = AiDiProvider.GetContainer().GetInstance<FieldFeatureGenerator>();
            var config = AiDiProvider.GetContainer().GetInstance<FieldFeatureGenerator.Config>();
            config.TargetFeatue[FieldFeatureGenerator.Feature.NoticeObstruction] = true;
            config.TargetFeatue[FieldFeatureGenerator.Feature.DangerCount] = true;
            featureGen.Injection(config);
            gens.Add(Vector.Main, new IGeneratable<FieldContext, SparseVector<double>>[] {
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
