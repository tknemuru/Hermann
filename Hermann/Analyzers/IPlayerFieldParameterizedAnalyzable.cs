using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers
{
    /// <summary>
    /// フィールド状態をパラメータ付きで分析する機能を提供します。
    /// </summary>
    public interface IPlayerFieldParameterizedAnalyzable<TIn, TOut>
    {
        /// <summary>
        /// 指定したプレイヤのフィールド状態を分析します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        /// <returns>分析結果</returns>
        TOut Analyze(FieldContext context, Player.Index player, TIn param);
    }
}
