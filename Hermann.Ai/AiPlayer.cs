using Hermann.Ai.Serchers;
using Hermann.Contexts;
using Hermann.Learning.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai
{
    /// <summary>
    /// AIプレイヤ
    /// </summary>
    public class AiPlayer
    {
        /// <summary>
        /// 探索ロジック
        /// </summary>
        private NegaMax SearchLogic { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AiPlayer()
        {
            this.SearchLogic = AiDiProvider.GetContainer().GetInstance<NegaMax>();
        }

        /// <summary>
        /// 依存関係がある機能を注入します。
        /// </summary>
        /// <param name="game">ゲーム</param>
        public void Injection(Game game)
        {
            this.SearchLogic.Injection(game);
        }

        /// <summary>
        /// 移動する方向を考え、結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>移動方向</returns>
        public Direction Think(FieldContext context)
        {
            return this.SearchLogic.Search(context);
        }
    }
}
