using Hermann.Models;
using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// NEXTスライムの更新機能を提供します。
    /// </summary>
    public sealed class NextSlimeUpdater : IPlayerFieldUpdatable
    {
        /// <summary>
        /// NEXTスライム生成機能
        /// </summary>
        private NextSlimeGenerator NextSlimeGen { get; set; }

        /// <summary>
        /// NEXTスライムのキュー
        /// </summary>
        private Queue<Slime[]>[] NextSlimeQueue { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NextSlimeUpdater()
        {
            this.NextSlimeQueue = new Queue<Slime[]>[Player.Length];
            Player.ForEach(player =>
            {
                this.NextSlimeQueue[(int)player] = new Queue<Slime[]>();
            });
        }

        /// <summary>
        /// 依存機能を注入します。
        /// </summary>
        /// <param name="nextSlimeGen">NEXTスライム生成機能</param>
        public void Injection(NextSlimeGenerator nextSlimeGen)
        {
            this.NextSlimeGen = nextSlimeGen;
        }

        /// <summary>   
        /// フィールド状態を更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context, Player.Index player)
        {
            Slime[] movables = null;
            if (this.NextSlimeQueue[(int)player].Count <= 0)
            {
                // キューにストックがない場合は、新しいNEXTスライムを生成して両プレイヤのキューにストックする
                var newMovables = this.NextSlimeGen.GetNext();
                Player.ForEach(p =>
                {
                    this.NextSlimeQueue[(int)p].Enqueue(newMovables);
                });
            }

            // キューからNEXTスライムを取得
            movables = this.NextSlimeQueue[(int)player].Dequeue();

            // NEXTスライムをスライドする
            context.NextSlimes[(int)player][(int)NextSlime.Index.First] = context.NextSlimes[(int)player][(int)NextSlime.Index.Second];
            context.NextSlimes[(int)player][(int)MovableSlime.UnitIndex.Second] = movables;
        }
    }
}
