using Hermann.Ai.Analyzers;
using Hermann.Ai.Evaluators;
using Hermann.Contexts;
using Hermann.Learning.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Serchers
{
    public class SingleCompleteReading : ISerchable<FieldContext, IEnumerable<Direction>>
    {
        /// <summary>
        /// 読み手分析機能
        /// </summary>
        private CompleteReadingMovableDirectionAnalyzer Analyzer { get; set; }

        /// <summary>
        /// フィールド状態の評価機能
        /// </summary>
        private MultipleLinearRegressionFieldContextEvaluator Evaluator { get; set; }

        /// <summary>
        /// ゲームロジック
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SingleCompleteReading()
        {
            this.Analyzer = AiDiProvider.GetContainer().GetInstance<CompleteReadingMovableDirectionAnalyzer>();
            this.Evaluator = AiDiProvider.GetContainer().GetInstance<MultipleLinearRegressionFieldContextEvaluator>();
        }

        /// <summary>
        /// 依存関係がある機能を注入します。
        /// </summary>
        /// <param name="game">ゲーム</param>
        public void Injection(Game game)
        {
            this.Game = game;
        }

        /// <summary>
        /// 一手を完全に読みきる探索を実行します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>最善手</returns>
        public IEnumerable<Direction> Search(FieldContext context)
        {
            var valueDic = new Dictionary<int, double>();
            var index = 0;

            // 全移動パターンを取得
            var allPatterns = this.Analyzer.Analyze(context, context.OperationPlayer).ToArray();

            foreach(var patterns in allPatterns)
            {
                var player = context.OperationPlayer;

                // スライムを動かす
                Debug.Assert(context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.None, "イベント発生中はありえません");
                var _context = context.DeepCopy();

                // 回転と横移動
                foreach (var p in patterns)
                {
                    _context.OperationDirection = p;
                    _context = this.Game.Update(_context);
                }

                // 最後まで落下させる
                while (_context.Ground[(int)player])
                {
                    _context.OperationDirection = Direction.Down;
                    _context = this.Game.Update(_context);
                }

                // 接地したら即設置完了にする
                _context.BuiltRemainingTime[(int)player] = -1;
                _context.OperationDirection = Direction.None;
                _context = this.Game.Update(_context);

                // イベント終了まで更新する
                while (_context.FieldEvent[(int)player] != FieldEvent.None)
                {
                    _context = this.Game.Update(_context);
                }
                Debug.Assert(_context.FieldEvent[(int)player] == FieldEvent.None, "イベント発生中はありえません");

                // 評価実施
                valueDic.Add(index, this.Evaluator.Evaluate(_context));
                index++;
            }

            return context.OperationPlayer == Player.Index.First ?
                allPatterns[valueDic.OrderByDescending(kv => kv.Value).First().Key]
                : allPatterns[valueDic.OrderBy(kv => kv.Value).First().Key];
        }
    }
}
