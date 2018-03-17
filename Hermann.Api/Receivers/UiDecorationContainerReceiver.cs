using Hermann.Analyzers.Fields;
using Hermann.Api.Containers;
using Hermann.Contexts;
using Hermann.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api.Receivers
{
    /// <summary>
    /// フィールド状態を受信し、UIの飾りに必要な情報を返却する機能を提供します。
    /// </summary>
    public class UiDecorationContainerReceiver : IReceivable<FieldContext, UiDecorationContainer>
    {
        /// <summary>
        /// フィールド状態を受信し、リッチUIに必要な情報を返却します。
        /// </summary>
        /// <param name="source">ソース</param>
        /// <returns>受信結果</returns>
        public UiDecorationContainer Receive(FieldContext context)
        {
            var container = DiProvider.GetContainer().GetInstance<UiDecorationContainer>();
            container.FieldContext = context;
            var slimeJoinAnalyzer = DiProvider.GetContainer().GetInstance<SlimeJoinStateAnalyzer>();
            Player.ForEach(player =>
            {
                container.SlimeJoinStatus[(int)player] = slimeJoinAnalyzer.Analyze(context, player);
            });
            return container;
        }
    }
}
