using Hermann.Analyzers;
using Hermann.Contexts;
using Hermann.Helper;
using Hermann.Learning.Di;
using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning.Analyzers
{
    /// <summary>
    /// 削除できる可能性のあるスライムの分析機能を提供します。
    /// </summary>
    public class ErasedPotentialSlimeAnalyzer : IPlayerFieldParameterizedAnalyzable<ErasedPotentialSlimeAnalyzer.Param, int>
    {
        /// <summary>
        /// 重力
        /// </summary>
        private Gravity Gravity { get; set; }

        /// <summary>
        /// 重力パラメータ
        /// </summary>
        private static readonly Gravity.Param GravityParam = new Gravity.Param
        {
            Strength = 99,
        };

        /// <summary>
        /// 消済マーキングパラメータ
        /// </summary>
        private static readonly SlimeErasingMarker.Param ErasingMarkerParam = new SlimeErasingMarker.Param();

        /// <summary>
        /// 消済マーキング機能
        /// </summary>
        private SlimeErasingMarker ErasingMarker { get; set; }

        /// <summary>
        /// パラメータ
        /// </summary>
        public class Param
        {
            /// <summary>
            /// デフォルトの削除対象スライム
            /// </summary>
            private static readonly Slime[] DefaultErasedSlimes = ExtensionSlime.Slimes.ToArray();

            /// <summary>
            /// 分析対象のスライム
            /// </summary>
            public Slime TargetSlime { get; set; }

            /// <summary>
            /// 削除対象のスライム
            /// </summary>
            public Slime[] ErasedSlimes { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Param()
            {
                this.TargetSlime = Slime.None;
                this.ErasedSlimes = DefaultErasedSlimes;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErasedPotentialSlimeAnalyzer()
        {
            this.Gravity = LearningClientDiProvider.GetContainer().GetInstance<Gravity>();
            this.ErasingMarker = LearningClientDiProvider.GetContainer().GetInstance<SlimeErasingMarker>();
        }

        /// <summary>
        /// 削除できる可能性のあるスライムの分析する
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        /// <returns>削除できる可能性のあるスライム数</returns>
        public int Analyze(FieldContext context, Player.Index player, Param param)
        {
            Debug.Assert(param.TargetSlime != Slime.None, "対象スライムがNoneはありえません");

            var _context = context.DeepCopy();

            // フィールドから削除対象のスライムを削除する
            foreach(var slime in param.ErasedSlimes)
            {
                if(slime == param.TargetSlime)
                {
                    continue;
                }
                _context.SlimeFields[(int)player][slime] = Enumerable.Range(0, FieldContextConfig.FieldUnitCount).Select(i => 0u).ToArray();
            }

            // 移動可能スライムも落下させたいので、情報のみ場外においやってしまう
            MovableSlime.ForEach(index =>
            {
                var movable = _context.MovableSlimes[(int)player][(int)index];
                movable.Index = 0;
                movable.Position = (int)index;
            });

            // 重力ですべて落下させる
            this.Gravity.Update(_context, player, GravityParam);

            // スライムを消済にマーキングする
            this.ErasingMarker.Update(_context, player, ErasingMarkerParam);

            // 消済スライム数をカウントする
            return SlimeCountHelper.GetSlimeCount(_context, player, Slime.Erased);
        }
    }
}
