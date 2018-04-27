using Hermann.Ai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Providers
{
    /// <summary>
    /// パターンを提供します。
    /// </summary>
    public class PatternProvider
    {
        /// <summary>
        /// パターンリスト
        /// </summary>
        public static readonly IEnumerable<Pattern> Patterns = ((IEnumerable<Pattern>)Enum.GetValues(typeof(Pattern)));

        /// <summary>
        /// パターン辞書
        /// </summary>
        private Dictionary<Pattern, PatternDefinition> PatternDefinitionDic { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PatternProvider()
        {
            this.PatternDefinitionDic = this.BuildPatternDefinitionDic();
        }

        /// <summary>
        /// 指定したパターンを取得します。
        /// </summary>
        /// <param name="pattern">パターン</param>
        /// <returns>パターン</returns>
        public PatternDefinition Get(Pattern pattern)
        {
            return this.PatternDefinitionDic[pattern];
        }

        /// <summary>
        /// パターン辞書を組み立てます。
        /// </summary>
        /// <returns>パターン辞書</returns>
        private Dictionary<Pattern, PatternDefinition> BuildPatternDefinitionDic()
        {
            var dic = new Dictionary<Pattern, PatternDefinition>();
            var pattern = new List<string>();
            pattern.Add("10000000");
            pattern.Add("01000000");
            pattern.Add("01000000");
            pattern.Add("01000000");
            dic.Add(Pattern.StairsOneLeft, new PatternDefinition(
                    Pattern.StairsOneLeft,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("01000000");
            pattern.Add("10000000");
            pattern.Add("10000000");
            pattern.Add("10000000");
            dic.Add(Pattern.StairsOneRight, new PatternDefinition(
                    Pattern.StairsOneRight,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("10000000");
            pattern.Add("10000000");
            pattern.Add("01000000");
            pattern.Add("01000000");
            dic.Add(Pattern.StairsTwoLeft, new PatternDefinition(
                    Pattern.StairsTwoLeft,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("01000000");
            pattern.Add("01000000");
            pattern.Add("10000000");
            pattern.Add("10000000");
            dic.Add(Pattern.StairsTwoRight, new PatternDefinition(
                    Pattern.StairsTwoRight,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("01000000");
            pattern.Add("00000000");
            pattern.Add("11000000");
            pattern.Add("01000000");
            dic.Add(Pattern.FloatRight, new PatternDefinition(
                    Pattern.FloatRight,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("00100000");
            pattern.Add("00000000");
            pattern.Add("11000000");
            pattern.Add("01000000");
            dic.Add(Pattern.FloatFarRight, new PatternDefinition(
                    Pattern.FloatFarRight,
                    pattern,
                    3,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("10000000");
            pattern.Add("00000000");
            pattern.Add("11000000");
            pattern.Add("10000000");
            dic.Add(Pattern.FloatLeft, new PatternDefinition(
                    Pattern.FloatLeft,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("10000000");
            pattern.Add("00000000");
            pattern.Add("01100000");
            pattern.Add("01000000");
            dic.Add(Pattern.FloatFarLeft, new PatternDefinition(
                    Pattern.FloatFarLeft,
                    pattern,
                    3,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("10000000");
            pattern.Add("01000000");
            pattern.Add("10000000");
            pattern.Add("10000000");
            dic.Add(Pattern.InterposeUpperLeft, new PatternDefinition(
                    Pattern.InterposeUpperLeft,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("01000000");
            pattern.Add("10000000");
            pattern.Add("01000000");
            pattern.Add("01000000");
            dic.Add(Pattern.InterposeUpperRight, new PatternDefinition(
                    Pattern.InterposeUpperRight,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("10000000");
            pattern.Add("10000000");
            pattern.Add("01000000");
            pattern.Add("10000000");
            dic.Add(Pattern.InterposeLowerLeft, new PatternDefinition(
                    Pattern.InterposeLowerLeft,
                    pattern,
                    2,
                    4
                ));

            pattern = new List<string>();
            pattern.Add("01000000");
            pattern.Add("01000000");
            pattern.Add("10000000");
            pattern.Add("01000000");
            dic.Add(Pattern.InterposeLowerRight, new PatternDefinition(
                    Pattern.InterposeLowerRight,
                    pattern,
                    2,
                    4
                ));
            return dic;
        }
    }
}
