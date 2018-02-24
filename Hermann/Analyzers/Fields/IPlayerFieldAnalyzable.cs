using Hermann.Contexts;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers.Fields
{
    /// <summary>
    /// 指定したプレイヤのフィールド状態の分析機能を提供します。
    /// </summary>
    /// <typeparam name="TOut">分析結果</typeparam>
    public interface IPlayerFieldAnalyzable<TOut>
    {
        /// <summary>
        /// フィールド状態を分析した結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>分析結果</returns>
        TOut Analyze(FieldContext context, Player.Index player);
    }
}
