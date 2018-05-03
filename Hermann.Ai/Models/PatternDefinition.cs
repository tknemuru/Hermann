using Hermann.Environments;
using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Models
{
    /// <summary>
    /// パターン定義
    /// </summary>
    public class PatternDefinition
    {
        /// <summary>
        /// インデックスファイルのパス
        /// </summary>
        private static readonly string IndexFilePath = $"{EnvConfig.GetRootDir()}/Hermann.Ai/resources/patterndefinition/";

        /// <summary>
        /// インデックスファイルの拡張子
        /// </summary>
        private const string IndexFileExtension = @".pidx";

        /// <summary>
        /// パターン
        /// </summary>
        public Pattern Pattern { get; set; }

        /// <summary>
        /// パターンの数値
        /// </summary>
        public uint PatternDigit { get; set; }

        /// <summary>
        /// 横幅
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 縦幅
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// インデックスの最大値
        /// </summary>
        public int MaxIndex { get; set; }

        /// <summary>
        /// インデックス辞書
        /// </summary>
        private Dictionary<uint, int> IndexDic { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pattern">パターン</param>
        /// <param name="patternDigit">2進数で表現されたパターン文字列のリスト</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        public PatternDefinition(Pattern pattern, IEnumerable<string> patternDigit, int width, int height)
        {
            this.Pattern = pattern;
            this.PatternDigit = FieldContextHelper.ConvertDigitStrsToUnit(patternDigit);
            this.Width = width;
            this.Height = height;
            this.IndexDic = this.ReadIndex();
            this.MaxIndex = this.IndexDic.Values.Max();
        }

        /// <summary>
        /// インデックスを取得します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>インデックス</returns>
        public int GetIndex(uint key)
        {
            return this.IndexDic[key];
        }

        /// <summary>
        /// インデックスに対応したキーを取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns>インデックスに対応したキー</returns>
        public uint GetIndexKey(int index)
        {
            return this.IndexDic.Where(kv => kv.Value == index).First().Key;
        }

        /// <summary>
        /// インデックスを書き込みます
        /// </summary>
        /// <param name="dic">インデックス辞書</param>
        public void WriteIndex(Dictionary<uint, int> dic)
        {
            var path = $"{IndexFilePath}{this.Pattern.GetName().ToLower()}{IndexFileExtension}";
            FileHelper.Write($"{dic.First().Key},{dic.First().Value}", path);
            var keyValueStrs = dic.Skip(1).Select(d => $",{d.Key},{d.Value}");
            foreach(var kvs in keyValueStrs)
            {
                FileHelper.Write(kvs, path);
            }
        }

        /// <summary>
        /// インデックスを読み込みます。
        /// </summary>
        /// <returns>インデックス辞書</returns>
        private Dictionary<uint, int> ReadIndex()
        {
            var path = $"{IndexFilePath}{this.Pattern.GetName().ToLower()}{IndexFileExtension}";
            if (!File.Exists(path))
            {
                return new Dictionary<uint, int>() { { 0u, 0 } };
            }

            var csv = FileHelper.ReadTextLines(path);
            Debug.Assert(csv.Count() == 1, "行数が不正です。");
            var list = csv.First().Split(',');
            Debug.Assert(list.Count() % 2 == 0, "要素数が奇数です。");

            var dic = new Dictionary<uint, int>();
            for (var i = 0; i < list.Count() - 1; i = i + 2)
            {
                dic.Add(uint.Parse(list[i]), int.Parse(list[i + 1]));
            }

            return dic;
        }
    }
}
