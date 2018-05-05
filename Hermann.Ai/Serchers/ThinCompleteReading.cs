using Hermann.Ai.Evaluators;
using Hermann.Ai.Helpers;
using Hermann.Ai.Providers;
using Hermann.Contexts;
using Hermann.Ai.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Serchers
{
    /// <summary>
    /// 1階層のみの完全読み探索機能を提供します。
    /// </summary>
    public class ThinCompleteReading : ISerchable<FieldContext, IEnumerable<Direction>>, IInjectable<ThinCompleteReading.Config>
    {
        /// <summary>
        /// 設定情報
        /// </summary>
        public class Config
        {
            /// <summary>
            /// AIのバージョン
            /// </summary>
            /// <value>The version.</value>
            public AiPlayer.Version Version { get; set; }

            /// <summary>
            /// 使用スライム
            /// </summary>
            public Slime[] UsingSlime { get; set; }
        }

        /// <summary>ß
        /// 注入が完了したかどうか
        /// </summary>
        /// <value><c>true</c> if has injected; otherwise, <c>false</c>.</value>
        public bool HasInjected { get; private set; } = false;

        /// <summary>
        /// 設定情報
        /// </summary>
        private Config MyConfig { get; set; }

        /// <summary>
        /// ゲームロジック
        /// </summary>
        private Game Game { get; set; }

        /// <summary>
        /// 依存関係がある機能を注入します。
        /// </summary>
        /// <param name="config">設定情報</param>
        public void Inject(Config config)
        {
            this.MyConfig = config;
            this.Game = AiDiProvider.GetContainer().GetInstance<Game>();
            this.Game.Inject(config.UsingSlime);
            this.HasInjected = true;
        }

        /// <summary>
        /// 一手を完全に読みきる探索を実行します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>最善手</returns>
        public IEnumerable<Direction> Search(FieldContext context)
        {
            Debug.Assert(this.HasInjected, "依存性の注入が完了していません");
#if DEBUG
            for (var i = 0; i < context.UsingSlimes.Length; i++)
            {
                Debug.Assert(context.UsingSlimes[i] == this.MyConfig.UsingSlime[i], "使用スライムが不正です");
            }
#endif
            var valueDic = new Dictionary<int, double>();
            var index = 0;

            // 全移動パターンを取得
            var allPatterns = CompleteReadingHelper.GetAllMovePatterns().ToArray();

            foreach(var patterns in allPatterns)
            {
                // 移動実施
                var _context = this.Update(context, patterns);

                // 評価実施
                valueDic.Add(index, AiDiProvider.GetContainer().GetInstance<EvalProvider>().
                    GetEval(this.MyConfig.Version, _context));
                index++;
            }

            return context.OperationPlayer == Player.Index.First ?
                allPatterns[valueDic.OrderByDescending(kv => kv.Value).First().Key]
                : allPatterns[valueDic.OrderBy(kv => kv.Value).First().Key];
        }

        /// <summary>
        /// フィールドの更新を行います。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="patterns">移動パターン</param>
        /// <returns>更新されたフィールド</returns>
        private FieldContext Update(FieldContext context, IEnumerable<Direction> patterns)
        {
            Debug.Assert(context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.None || context.FieldEvent[(int)context.OperationPlayer] == FieldEvent.End, "イベント発生中はありえません");
            var player = context.OperationPlayer;
            var _context = context.DeepCopy();

            // 回転と横移動
            foreach (var p in patterns)
            {
                _context.OperationDirection = p;
                _context = this.Game.Update(_context);
            }

            // 最後まで落下させる
            while (!_context.Ground[(int)player] && _context.FieldEvent[(int)player] != FieldEvent.End)
            {
                _context.OperationDirection = Direction.Down;
                _context = this.Game.Update(_context);
            }

            // 接地したら即設置完了にする
            _context.BuiltRemainingTime[(int)player] = -1;
            _context.OperationDirection = Direction.None;
            _context = this.Game.Update(_context);

            // イベント終了 or ゲーム終了まで更新する
            while (_context.FieldEvent[(int)player] != FieldEvent.None && _context.FieldEvent[(int)player] != FieldEvent.End)
            {
                _context = this.Game.Update(_context);
            }
            Debug.Assert(_context.FieldEvent[(int)player] == FieldEvent.None || _context.FieldEvent[(int)player] == FieldEvent.End, "イベント発生中はありえません");

            return _context;
        }
    }
}
