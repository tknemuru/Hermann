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
    public sealed class NextSlimeUpdater : IFieldUpdatable
    {
        /// <summary>
        /// NEXTスライム生成機能
        /// </summary>
        private NextSlimeGenerator NextSlimeGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NextSlimeUpdater()
        {
            this.NextSlimeGen = DiProvider.GetContainer().GetInstance<NextSlimeGenerator>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nextSlimeGen">NEXTスライム生成機能</param>
        public NextSlimeUpdater(NextSlimeGenerator nextSlimeGen)
        {
            this.NextSlimeGen = nextSlimeGen;
        }

        /// <summary>
        /// フィールド状態を更新します。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            // 新しいNEXTスライムを生成する
            var movables = this.NextSlimeGen.GetNext();

            // NEXTスライムをスライドする
            context.NextSlimes[(int)context.OperationPlayer][(int)NextSlime.Index.First] = context.NextSlimes[(int)context.OperationPlayer][(int)NextSlime.Index.Second];
            context.NextSlimes[(int)context.OperationPlayer][(int)MovableSlime.UnitIndex.Second] = movables;
        }
    }
}
