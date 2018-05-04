using System;
using System.Linq;
using Hermann.Ai.Generators;
using Hermann.Analyzers;
using Hermann.Client.LearningClient.Di;
using Hermann.Contexts;
using Hermann.Models;

namespace Hermann.Client.LearningClient.Generators
{
    /// <summary>
    /// 次に動かす方向をランダムに生成します。
    /// </summary>
    public class MoveDirectionRandomGenerator : IMoveDirectionGeneratable
    {
        /// <summary>
        /// 移動可能方向の分析機能
        /// </summary>
        private MovableDirectionAnalyzer MovableDirectionAnalyzer { get; set; }

        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MoveDirectionRandomGenerator()
        {
            MovableDirectionAnalyzer = LearningClientDiProvider.GetContainer().GetInstance<MovableDirectionAnalyzer>();
            RandomGen = new Random();
        }

        /// <summary>
        /// 次に動かす方向をランダムに生成します。
        /// </summary>
        /// <returns>次に動かす方向</returns>
        /// <param name="context">フィールド状態</param>
        public Direction GetNext(FieldContext context)
        {
            var directions = MovableDirectionAnalyzer.Analyze(context, context.OperationPlayer).ToArray();
            if (directions.Count() <= 0)
            {
                return Direction.None;
            }

            var index = RandomGen.Next(directions.Count());
            return directions[index];
        }
    }
}
