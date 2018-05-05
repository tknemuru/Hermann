using Hermann.Models;
using Hermann.Contexts;
using Hermann.Di;
using Hermann.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hermann.Updaters
{
    /// <summary>
    /// NEXTスライムの更新機能を提供します。
    /// </summary>
    public sealed class NextSlimeUpdater : IPlayerFieldUpdatable, IInjectable<NextSlimeGenerator>
    {
        /// <summary>
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

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
        public void Inject(NextSlimeGenerator nextSlimeGen)
        {
            this.NextSlimeGen = nextSlimeGen;
            this.HasInjected = true;
        }

        /// <summary>   
        /// フィールド状態を更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context, Player.Index player)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");

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
