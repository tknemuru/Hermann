using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Contexts
{
    /// <summary>
    /// フィールドの状態
    /// </summary>
    public class FieldContext
    {
        /// <summary>
        /// コマンド
        /// </summary>
        public uint Command { get; set; }

        /// <summary>
        /// 移動可能なスライムの配置状態
        /// </summary>
        public MovableInfo[] MovableInfos { get; set; }

        /// <summary>
        /// スライムごとの配置状態
        /// </summary>
        public Dictionary<Slime, uint[]> SlimeFields { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FieldContext()
        {
            this.MovableInfos = new MovableInfo[ExtensionMovableUnit.Length];
            this.SlimeFields = new Dictionary<Slime, uint[]>();
        }

        /// <summary>
        /// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var context = (FieldContext)obj;
            var equals = new List<bool>();
            
            // コマンド
            equals.Add(context.Command == this.Command);

            // スライムごとの配置状態
            equals.Add(context.SlimeFields.Count == this.SlimeFields.Count);
            foreach (var slime in context.SlimeFields.Keys)
            {
                for (var i = 0; i < context.SlimeFields[slime].Length; i++)
                {
                    equals.Add(context.SlimeFields[slime][i] == this.SlimeFields[slime][i]);
                }
            }

            // 移動可能なスライムの配置状態
            equals.Add(context.MovableInfos.Length == this.MovableInfos.Length);
            equals.Add(context.MovableInfos[(int)MovableUnit.First].Equals(this.MovableInfos[(int)MovableUnit.First]));
            equals.Add(context.MovableInfos[(int)MovableUnit.Second].Equals(this.MovableInfos[(int)MovableUnit.Second]));

            return equals.All(e => e);
        }

        /// <summary>
        /// 既定のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在のオブジェクトのハッシュ コード。</returns>
        public override int GetHashCode()
        {
            return (int)this.Command ^ this.MovableInfos.GetHashCode() ^ this.SlimeFields.GetHashCode();
        }
    }
}
