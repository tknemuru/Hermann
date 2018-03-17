using Hermann.Contexts;
using Hermann.Helper;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Analyzers.Fields
{
    /// <summary>
    /// スライムの結合状態の分析機能を提供します。
    /// </summary>
    public class SlimeJoinStateAnalyzer : IPlayerFieldAnalyzable<SlimeJoinState[]>
    {
        /// <summary>
        /// 移動メソッドとその結果のスライム結合状態を格納するコンテナ
        /// </summary>
        private class MoveFuncContainer
        {
            /// <summary>
            /// 移動メソッド
            /// </summary>
            public Func<int, int, MoveHelper.Move, bool> MoveFunc { get; set; }

            /// <summary>
            /// 移動メソッドの結果のスライム結合状態
            /// </summary>
            public SlimeJoinState SlimeJoinState { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="func">移動メソッド</param>
            /// <param name="state">移動メソッドの結果のスライム結合状態</param>
            public MoveFuncContainer(Func<int, int, MoveHelper.Move, bool> func, SlimeJoinState state)
            {
                this.MoveFunc = func;
                this.SlimeJoinState = state;
            }
        }

        /// <summary>
        /// フィールド状態を分析した結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <returns>分析結果</returns>
        public SlimeJoinState[] Analyze(FieldContext context, Player.Index player)
        {
            var status = Enumerable.Range(0, (FieldContextConfig.FieldUnitCount - (FieldContextConfig.MaxHiddenUnitIndex + 1)) * FieldContextConfig.FieldUnitBitCount).Select(i => SlimeJoinState.Default).ToArray();
            var slimes = ExtensionSlime.Slimes.Where(s => s != Slime.Obstruction && s != Slime.Erased);
            var move = new MoveHelper.Move();
            var moveFuncContainers = new List<MoveFuncContainer>()
                {
                    new MoveFuncContainer(MoveHelper.TryMoveUp, SlimeJoinState.Up),
                    new MoveFuncContainer(MoveHelper.TryMoveDown, SlimeJoinState.Down),
                    new MoveFuncContainer(MoveHelper.TryMoveRight, SlimeJoinState.Right),
                    new MoveFuncContainer(MoveHelper.TryMoveLeft, SlimeJoinState.Left)
                };

            foreach(var slime in slimes)
            {
                var slimeField = context.SlimeFields[(int)player][slime];
                var stateIndex = 0;
                for (var unit = 0; unit < FieldContextConfig.FieldUnitCount; unit++)
                {
                    // 隠し行は対象外
                    if(unit <= FieldContextConfig.MaxHiddenUnitIndex)
                    {
                        continue;
                    }

                    for (var i = 0; i < FieldContextConfig.FieldUnitBitCount; i++)
                    {
                        // 移動可能スライムは対象外
                        if (FieldContextHelper.ExistsMovableSlime(context, player, unit, i))
                        {
                            stateIndex++;
                            continue;
                        }

                        if ((slimeField[unit] & 1u << i) > 0)
                        {
                            Debug.Assert(status[stateIndex] == SlimeJoinState.Default, "既に結合状態が初期状態から書き換えられています。");

                            foreach (var container in moveFuncContainers)
                            {
                                if (container.MoveFunc(unit, i, move))
                                {
                                    if ((slimeField[move.Unit] & 1u << move.Index) > 0)
                                    {
                                        // 移動可能スライムは対象外
                                        if (!FieldContextHelper.ExistsMovableSlime(context, player, move.Unit, move.Index))
                                        {
                                            status[stateIndex] |= container.SlimeJoinState;
                                        }
                                    }
                                }
                            }
                        }
                        stateIndex++;
                    }
                }
            }

            return status;
        }
    }
}
