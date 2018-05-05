using Accord.Math;
using Hermann.Ai.Generators;
using Hermann.Ai.Models;
using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Generators;
using Hermann.Helpers;
using Hermann.Ai.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hermann.Ai.Generators
{
    /// <summary>
    /// パターン生成機能を提供します。
    /// </summary>
    public class PatternGenerator : IGeneratable<FieldContext, SparseVector<double>>, IInjectable<PatternGenerator.Config>
    {
        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// 設定情報
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 生成対象のパターンリスト
            /// </summary>
            public IEnumerable<PatternDefinition> Patterns { get; set; }

            /// <summary>
            /// 疎な部分の値
            /// </summary>
            /// <value>The sparse value.</value>
            public double SparseValue { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Config()
            {
                this.SparseValue = 0.0d;
            }
        }

        /// <summary>
        /// 設定情報
        /// </summary>
        private Config MyConfig { get; set; }

        /// <summary>
        /// 依存する情報を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Inject(Config config)
        {
            this.MyConfig = config;
            this.HasInjected = true;
        }

        /// <summary>
        /// パターンを生成します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>現在のフィールド状態のパターンリスト</returns>
        public SparseVector<double> GetNext(FieldContext context)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

            int maxIndex = 0;
            var dic = new Dictionary<int, double>();
            Player.ForEach(player =>
            {
                var colorSlimeFields = context.SlimeFields[player.ToInt()].
                    Where(f => context.UsingSlimes.Contains(f.Key)).
                    Select(f => f.Value.Skip(FieldContextConfig.MinDisplayUnitIndex).ToArray()).ToArray();
                var obsSlimeFields = context.SlimeFields[player.ToInt()].
                    Where(f => f.Key == Slime.Obstruction).
                    Select(f => f.Value.Skip(FieldContextConfig.MinDisplayUnitIndex).ToArray()).ToArray();

                foreach (var pattern in this.MyConfig.Patterns)
                {
                    foreach(var fields in colorSlimeFields)
                    {
                        this.AddPatternCount(fields, pattern, maxIndex, dic);
                    }
                    maxIndex += (int)pattern.MaxIndex + 1;
                    foreach (var fields in obsSlimeFields)
                    {
                        this.AddPatternCount(fields, pattern, maxIndex, dic);
                    }
                    maxIndex += (int)pattern.MaxIndex + 1;
                }
            });
            var vector = new SparseVector<double>(maxIndex, dic, this.MyConfig.SparseValue);
            return vector;
        }

        /// <summary>
        /// パターンの出現回数を辞書に追加します。
        /// </summary>
        /// <param name="fields">フィールドのスライム配置状態</param>
        /// <param name="pattern">パターン</param>
        /// <param name="maxIndex">インデックスの最大値</param>
        /// <param name="dic">パターン出現回数記録辞書</param>
        private void AddPatternCount(uint[] fields, PatternDefinition pattern, int maxIndex, Dictionary<int, double> dic)
        {
            // 1行ずつずらしたユニットを作成
            var mergedField = AiDiProvider.GetContainer().GetInstance<MergedFieldsGenerator>().GetNext(fields);

            foreach (var field in mergedField)
            {
                for (var w = 0; w <= FieldContextConfig.VerticalLineLength - pattern.Width; w++)
                {
                    for (var h = 0; h <= FieldContextConfig.FieldUnitLineCount - pattern.Height; h++)
                    {
                        var shiftedPattern = ((pattern.PatternDigit >> (1 * w)) << (FieldContextConfig.OneLineBitCount * h));
                        var key = ((field & shiftedPattern) >> (FieldContextConfig.OneLineBitCount * h)) << (1 * w);
                        var index = pattern.GetIndex(key);

                        index += maxIndex;
                        if (dic.ContainsKey(index))
                        {
                            dic[index]++;
                        }
                        else
                        {
                            dic[index] = 1d;
                        }
                    }
                }
            }
        }
    }
}
