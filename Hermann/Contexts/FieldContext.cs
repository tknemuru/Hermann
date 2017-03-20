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
        /// プレイヤ
        /// </summary>
        public int OperationPlayer { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public Direction OperationDirection { get; set; }

        /// <summary>
        /// 移動可能なスライム
        /// </summary>
        public MovableSlime[][] MovableSlimes { get; set; }

        /// <summary>
        /// スライムごとの配置状態
        /// </summary>
        public Dictionary<Slime, uint[]>[] SlimeFields { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FieldContext()
        {
            this.MovableSlimes = new MovableSlime[Player.Length][];
            this.MovableSlimes[Player.First] = new MovableSlime[MovableSlimeUnit.Length];
            this.MovableSlimes[Player.Second] = new MovableSlime[MovableSlimeUnit.Length];

            this.SlimeFields = new Dictionary<Slime, uint[]>[Player.Length];
            this.SlimeFields[Player.First] = new Dictionary<Slime, uint[]>();
            this.SlimeFields[Player.Second] = new Dictionary<Slime, uint[]>();
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
            
            // プレイヤ
            equals.Add(context.OperationPlayer == this.OperationPlayer);

            // 方向
            equals.Add(context.OperationDirection == this.OperationDirection);

            for (var player = Player.First; player < Player.Length; player++)
            {
                // スライムごとの配置状態
                var slimeFields = context.SlimeFields[player];
                var mySlimeFields = this.SlimeFields[player];
                equals.Add(slimeFields.Count == mySlimeFields.Count);
                foreach (var slime in slimeFields.Keys)
                {
                    for (var i = 0; i < slimeFields[slime].Length; i++)
                    {
                        equals.Add(slimeFields[slime][i] == mySlimeFields[slime][i]);
                    }
                }

                // 移動可能なスライムの配置状態
                var movable = context.MovableSlimes[player];
                var myMovable = this.MovableSlimes[player];
                equals.Add(movable.Length == myMovable.Length);
                equals.Add(movable[(int)MovableSlimeUnit.Index.First].Equals(myMovable[(int)MovableSlimeUnit.Index.First]));
                equals.Add(movable[(int)MovableSlimeUnit.Index.Second].Equals(myMovable[(int)MovableSlimeUnit.Index.Second]));
            }

            return equals.All(e => e);
        }

        /// <summary>
        /// 既定のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在のオブジェクトのハッシュ コード。</returns>
        public override int GetHashCode()
        {
            return this.OperationPlayer ^ (int)this.OperationDirection ^ this.MovableSlimes.GetHashCode() ^ this.SlimeFields.GetHashCode();
        }
    }
}
