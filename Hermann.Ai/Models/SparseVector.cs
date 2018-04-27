using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Models
{
    /// <summary>
    /// 疎ベクトルクラス
    /// </summary>
    public class SparseVector<T> : IEnumerable<T>
    {
        /// <summary>
        /// 疎ベクターの大半の成分を構成する要素
        /// </summary>
        private T SparseItem;

        /// <summary>
        /// 要素を構成するディクショナリ
        /// </summary>
        private Dictionary<int, T> ItemDictionary;

        /// <summary>
        /// 疎ベクトルの長さ
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"> 疎ベクターの大半の成分を構成する要素</param>
        public SparseVector(T sparseItem)
        {
            this.SparseItem = sparseItem;
            this.ItemDictionary = new Dictionary<int, T>();
            this.Length = 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"> 疎ベクターの大半の成分を構成する要素</param>
        public SparseVector(int length, T sparseItem)
        {
            this.SparseItem = sparseItem;
            this.ItemDictionary = new Dictionary<int, T>();
            this.Length = length;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"> 疎ベクターの大半の成分を構成する要素</param>
        public SparseVector(int length, Dictionary<int, T> itemDictionary, T sparseItem)
        {
            this.SparseItem = sparseItem;
            this.ItemDictionary = itemDictionary;
            this.Length = length;
        }

        /// <summary>
        /// 末尾にオブジェクトを追加します。
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (!item.Equals(this.SparseItem))
            {
                this.ItemDictionary.Add(this.Length, item);
            }
            this.Length++;
        }

        /// <summary>
        /// 末尾に疎ベクトルを追加します。
        /// </summary>
        /// <param name="vector"></param>
        public void Add(SparseVector<T> vector)
        {            
            foreach (var item in vector.NoSparseKeyValues)
            {
                this.ItemDictionary.Add(this.Length + item.Key, item.Value);
            }
            this.Length += vector.Length;
        }

        /// <summary>
        /// インデックスと要素のペアをCSV化して返します
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            string csv = string.Empty;
            foreach (KeyValuePair<int, T> keyValue in this.ItemDictionary)
            {
                if (csv != string.Empty) { csv += ","; }
                csv += keyValue.Key + "," + keyValue.Value;
            }
            return csv;
        }

        /// <summary>
        /// インデクサ
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public T this[int i]
        {
            set
            {
                if (this.Length <= i)
                {
                    throw new ApplicationException("存在しないインデックスです。");
                }
                else if (value.Equals(this.SparseItem) && this.ItemDictionary.ContainsKey(i))
                {
                    this.ItemDictionary.Remove(i);
                }
                this.ItemDictionary[i] = value;
            }
            get
            {
                if (this.Length <= i)
                {
                    throw new ApplicationException("存在しないインデックスです。");
                }
                return (this.ItemDictionary.ContainsKey(i) ? this.ItemDictionary[i] : this.SparseItem);
            }
        }

        /// <summary>
        /// 列挙子を取得します。
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Length; i++)
            {
                yield return (this.ItemDictionary.ContainsKey(i) ? this.ItemDictionary[i] : this.SparseItem);
            }
        }

        /// <summary>
        /// 列挙子を取得します。
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>
        /// 疎な部分を除いた列挙子。
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, T> NoSparseKeyValues
        {
            get
            {
                return this.ItemDictionary;
            }
        }
    }
}
