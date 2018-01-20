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
            // 新しいNEXTスライムを生成する
            var movables = this.NextSlimeGen.GetNext();

            // NEXTスライムをスライドする
            context.NextSlimes[(int)player][(int)NextSlime.Index.First] = context.NextSlimes[(int)player][(int)NextSlime.Index.Second];
            context.NextSlimes[(int)player][(int)MovableSlime.UnitIndex.Second] = movables;
        }
    }
}
