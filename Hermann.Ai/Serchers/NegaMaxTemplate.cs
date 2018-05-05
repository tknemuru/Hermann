using Hermann.Contexts;
using Hermann.Helpers;
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
    /// NegaMax法の探索ロジックテンプレートを提供します。
    /// </summary>
    public abstract class NegaMaxTemplate : ISerchable<FieldContext, Direction>
    {
        /// <summary>
        /// <para>初期アルファ値</para>
        /// </summary>
        protected const double DefaultAlpha = -1000000.0;

        /// <summary>
        /// <para>初期ベータ値</para>
        /// </summary>
        protected const double DefaultBeta = 1000000.0;

        /// <summary>
        /// 返却するキー
        /// </summary>
        protected Direction key;

        /// <summary>
        /// 評価値
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public NegaMaxTemplate()
        {
            this.key = this.GetDefaultKey();
            this.Value = DefaultAlpha;
        }

        /// <summary>
        /// 探索し、結果を返却します。
        /// </summary>
        /// <param name="context">フィールド状態</param>
        /// <returns>移動方向</returns>
        public Direction Search(FieldContext context)
        {
            StopWatchLogger.StartEventWatch("NegaMaxTemplate.SearchBestValue");
            this.Value = this.SearchBestValue(context.DeepCopy(), 1, DefaultAlpha, DefaultBeta);
            FileHelper.WriteLine("BestScore:" + this.Value);
            StopWatchLogger.StopEventWatch("NegaMaxTemplate.SearchBestValue");
            StopWatchLogger.WriteAllEventTimes("./log/eventtime.txt");
            return this.key;
        }

        /// <summary>
        /// <para>最善手を探索して取得する</para>
        /// </summary>
        /// <returns></returns>
        protected double SearchBestValue(FieldContext context, int depth, double alpha, double beta)
        {
            // 深さ制限に達した
            if (this.IsLimit(depth)) { return this.GetEvaluate(context); }

            // 可能な手をすべて生成
            var leafList = this.GetAllLeaf(context);

            double maxKeyValue = DefaultAlpha;
            if (leafList.Count() > 0)
            {
                // ソート
                StopWatchLogger.StartEventWatch("MoveOrdering");
                if (this.IsOrdering(depth)) { leafList = this.MoveOrdering(leafList, context); }
                StopWatchLogger.StopEventWatch("MoveOrdering");

                var lastContext = context.DeepCopy();
                foreach (Direction leaf in leafList)
                {
                    // 前処理
                    StopWatchLogger.StartEventWatch("SearchSetUp");
                    this.SearchSetUp(context, leaf);
                    StopWatchLogger.StopEventWatch("SearchSetUp");

                    double value = this.SearchBestValue(context, depth + 1, -beta, -alpha) * -1;

                    // 後処理
                    StopWatchLogger.StartEventWatch("SearchTearDown");
                    //context = this.SearchTearDown(lastContext);
                    context = lastContext.DeepCopy();
                    StopWatchLogger.StopEventWatch("SearchTearDown");

                    // ベータ刈り
                    if (value >= beta)
                    {
                        this.SetKey(leaf, depth);
                        return value;
                    }

                    if (value > maxKeyValue)
                    {
                        // より良い手が見つかった
                        this.SetKey(leaf, depth);
                        maxKeyValue = value;
                        // α値の更新
                        alpha = Math.Max(alpha, maxKeyValue);
                    }
                }
            }
            else
            {
                // ▼パスの場合▼
                // 前処理
                var lastContext = context.DeepCopy();
                this.PassSetUp(context);

                maxKeyValue = this.SearchBestValue(context, depth + 1, -beta, -alpha) * -1;

                // 後処理
                //context = this.PassTearDown(lastContext);
                context = lastContext.DeepCopy();
            }

            Debug.Assert(((maxKeyValue != DefaultAlpha) && (maxKeyValue != DefaultBeta)), "デフォルト値のまま返そうとしています。");
            return maxKeyValue;
        }

        /// <summary>
        /// 返却するキーをセットする
        /// </summary>
        /// <param name="leaf"></param>
        private void SetKey(Direction leaf, int depth)
        {
            if (depth == 1)
            {
                this.key = leaf;
            }
        }

        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected abstract bool IsLimit(int limit);

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract double GetEvaluate(FieldContext context);

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Direction> GetAllLeaf(FieldContext context);

        /// <summary>
        /// ソートをする場合はTrueを返す
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsOrdering(int depth);

        /// <summary>
        /// ソートする
        /// </summary>
        /// <param name="allLeaf"></param>
        /// <returns></returns>
        protected abstract IEnumerable<Direction> MoveOrdering(IEnumerable<Direction> allLeaf, FieldContext context);

        /// <summary>
        /// キーの初期値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract Direction GetDefaultKey();

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected abstract FieldContext SearchSetUp(FieldContext context, Direction leaf);

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected abstract FieldContext SearchTearDown(FieldContext lastContext);

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected abstract FieldContext PassSetUp(FieldContext context);

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected abstract FieldContext PassTearDown(FieldContext context);
    }
}
