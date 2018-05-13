using Hermann.Contexts;
using Hermann.Di;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers
{
    /// <summary>
    /// 移動可能な方向の分析機能を提供します。
    /// </summary>
    public class MovableDirectionAnalyzer : IPlayerFieldAnalyzable<IEnumerable<Direction>>
    {
        /// <summary>
        /// スライム移動機能
        /// </summary>
        private SlimeMover Mover { get; set; }

        /// <summary>
        /// 戻し回転方向の配列の長さ
        /// </summary>
        private const int UndoUpRotationDirectionLength = 3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MovableDirectionAnalyzer()
        {
            this.Mover = DiProvider.GetContainer().GetInstance<SlimeMover>();
        }

        /// <summary>
        /// 移動可能な方向を分析した結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>分析結果</returns>
        public IEnumerable<Direction> Analyze(FieldContext context, Player.Index player)
        {
            var ret = new List<Direction>();

            // イベント中の場合は移動不可
            if(context.FieldEvent[(int)player] != FieldEvent.None)
            {
                return ret;
            }

            // 元のフィールド状態を記録しておく
            var _context = context.DeepCopy();
            var param = new SlimeMover.Param();

            // 下
            _context.OperationDirection = Direction.Down;
            this.Mover.Update(_context, player, param);
            if (param.ResultState == SlimeMover.ResultState.Success)
            {
                ret.Add(Direction.Down);
                _context = context.DeepCopy();
            }

            // 上
            _context.OperationDirection = Direction.Up;
            this.Mover.Update(_context, player, param);
            if (param.ResultState == SlimeMover.ResultState.Success)
            {
                ret.Add(Direction.Up);
                _context = context.DeepCopy();
            }

            // 右
            _context.OperationDirection = Direction.Right;
            this.Mover.Update(_context, player, param);
            if(param.ResultState == SlimeMover.ResultState.Success)
            {
                ret.Add(Direction.Right);
                _context = context.DeepCopy();
            }

            // 左
            _context.OperationDirection = Direction.Left;
            this.Mover.Update(_context, player, param);
            if (param.ResultState == SlimeMover.ResultState.Success)
            {
                ret.Add(Direction.Left);
                _context = context.DeepCopy();
            }

            return ret;
        }
    }
}
