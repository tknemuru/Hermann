using Hermann.Models;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Updaters
{
    /// <summary>
    /// 消去対象のスライムを消済スライムとしてマーキングする機能を提供します。
    /// </summary>
    public class SlimeErasingMarker : IFieldUpdatable
    {
        /// <summary>
        /// 消去パターンリスト
        /// </summary>
        private static List<ErasePattern> ErasePatternList = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlimeErasingMarker()
        {
            if (ErasePatternList == null)
            {
                ErasePatternList = BuildErasePatternList();
            }
        }

        /// <summary>
        /// 消去対象のスライムを消済スライムとしてマーキングします。
        /// </summary>
        /// <param name="context">フィールドの状態</param>
        public void Update(FieldContext context)
        {
            var player = context.OperationPlayer;
            var slimes = ExtensionSlime.Slimes.Where(s => s != Slime.Erased);
            var erasedAllColorSlimes = CreateInitialErasedSlimes();

            // 各色のスライムを消済スライムに変換していく
            foreach (var slime in slimes)
            {
                // 対象スライムのフィールド状態を取得
                var fields = context.SlimeFields[(int)player][slime];

                // 対象スライムの削除情報を初期化
                var erasedSlimes = CreateInitialErasedSlimes();
                Debug.Assert(erasedSlimes.Length == fields.Length, string.Format("フィールドの要素数が不正です。要素数：{0}", fields.Length));

                // 最後のユニットの先頭行まで繰り返す
                var maxIndex = (FieldContextConfig.FieldLineCount - (FieldContextConfig.FieldUnitLineCount - 1));
                for (var horizontalIndex = 0; horizontalIndex < maxIndex; horizontalIndex++)
                {
                    // ユニットをまたいだ削除パターンを検出するために、開始行を1行ずつずらしながらユニットをマージして一つのユニットを作成する
                    var mergedField = MergeFields(fields, horizontalIndex);
                    var mergedErasedSlime = 0u;

                    // マージしたユニットに対する削除情報を作成する
                    foreach (var erasePattern in ErasePatternList)
                    {
                        var pattern = erasePattern.Pattern;
                        for (var v = 0; v < erasePattern.MaxVerticalShift; v++)
                        {
                            pattern = erasePattern.Pattern << v;
                            for (var h = 0; h < erasePattern.MaxHorizontalShift; h++)
                            {
                                if ((mergedField & pattern) == pattern)
                                {
                                    mergedErasedSlime |= pattern;
                                }
                                pattern <<= FieldContextConfig.OneLineBitCount;
                            }
                        }
                    }

                    // 削除情報を本来のユニット単位に分解する
                    var updateInfos = SeparateField(mergedErasedSlime, horizontalIndex);

                    // 分解した削除情報で対象スライムの削除情報を更新する
                    UpdateErasedSlimes(erasedSlimes, updateInfos, horizontalIndex);
                }

                // 対象スライムをフィールド上から消す
                UpdateContextSlimeFields(fields, erasedSlimes);

                // 全色消済スライム情報を更新する
                UpdateErasedAllColorSlimes(erasedAllColorSlimes, erasedSlimes);
            }

            // フィールド状態の消済スライム情報を更新する
            context.SlimeFields[(int)player][Slime.Erased] = erasedAllColorSlimes;
        }

        /// <summary>
        /// 初期状態の消済スライム配列を作成します。
        /// </summary>
        /// <returns>初期状態の消済スライム配列</returns>
        private static uint[] CreateInitialErasedSlimes()
        {
            var erasedSlimes = new uint[FieldContextConfig.FieldUnitCount];
            for (var i = 0; i < erasedSlimes.Length; i++)
            {
                erasedSlimes[i] = 0u;
            }
            return erasedSlimes;
        }

        /// <summary>
        /// 指定した開始行をもとにマージしたフィールド状態を返却します。
        /// </summary>
        /// <param name="fields">フィールドの状態</param>
        /// <param name="horizontalIndex">マージ開始行</param>
        /// <returns></returns>
        private static uint MergeFields(uint[] fields, int horizontalIndex)
        {
            var startUnitIndex = horizontalIndex / FieldContextConfig.FieldUnitLineCount;
            Debug.Assert(startUnitIndex >= 0 && startUnitIndex < FieldContextConfig.FieldUnitCount, string.Format("開始ユニットのインデックスが不正です。{0}", startUnitIndex));
            var shiftLine = horizontalIndex % FieldContextConfig.FieldUnitLineCount;
            Debug.Assert(shiftLine >= 0 && shiftLine < FieldContextConfig.FieldUnitLineCount, string.Format("シフト行数が不正です。{0}", shiftLine));

            // シフト行数が0なら開始ユニットをそのまま返却して終了
            if (shiftLine == 0)
            {
                return fields[startUnitIndex];
            }

            Debug.Assert(startUnitIndex < (FieldContextConfig.FieldUnitCount - 1), "最終ユニットがこのロジックを通ることは想定外です。");
            var mergedField = (fields[startUnitIndex] >> shiftLine * FieldContextConfig.OneLineBitCount) | (fields[startUnitIndex + 1] << (FieldContextConfig.FieldUnitLineCount - shiftLine) * FieldContextConfig.OneLineBitCount);

            return mergedField;
        }

        /// <summary>
        /// 指定した開始行をもとにマージしたフィールド状態を本来のユニット単位に分解します。
        /// </summary>
        /// <param name="field">マージしたフィールド状態</param>
        /// <param name="horizontalIndex">開始行</param>
        /// <returns>分解したフィールド状態</returns>
        private static uint[] SeparateField(uint field, int horizontalIndex)
        {
            var shiftLine = horizontalIndex % FieldContextConfig.FieldUnitLineCount;
            Debug.Assert(shiftLine >= 0 && shiftLine < FieldContextConfig.FieldUnitLineCount, string.Format("シフト行数が不正です。{0}", shiftLine));

            // シフト行数が0なら引数のフィールド状態を配列にして返却して終了
            if (shiftLine == 0)
            {
                return new[] { field };
            }

            var a = field << shiftLine * FieldContextConfig.OneLineBitCount;
            var b = field >> (FieldContextConfig.FieldUnitLineCount - shiftLine) * FieldContextConfig.OneLineBitCount;

            return new[] { a, b };
        }

        /// <summary>
        /// 消済スライム情報を更新します。
        /// </summary>
        /// <param name="erasedSlimes">更新対象の消済スライム情報</param>
        /// <param name="updateInfos">更新情報</param>
        /// <param name="horizontalIndex">マージ開始行</param>
        private static void UpdateErasedSlimes(uint[] erasedSlimes, uint[] updateInfos, int horizontalIndex)
        {
            Debug.Assert(updateInfos.Length > 0 && updateInfos.Length <= 2, string.Format("更新情報の要素数が不正です。{0}", updateInfos.Length));
            var startUnitIndex = horizontalIndex / FieldContextConfig.FieldUnitLineCount;
            Debug.Assert(startUnitIndex >= 0 && startUnitIndex < FieldContextConfig.FieldUnitCount, string.Format("開始ユニットのインデックスが不正です。{0}", startUnitIndex));

            erasedSlimes[startUnitIndex] |= updateInfos[0];
            if (updateInfos.Length > 1)
            {
                erasedSlimes[startUnitIndex + 1] |= updateInfos[1];
            }
        }

        /// <summary>
        /// フィールド状態のスライム情報を更新します。
        /// </summary>
        /// <param name="slimeFields">フィールド状態のスライム情報</param>
        /// <param name="erasedSlimes">消済スライムの情報</param>
        private static void UpdateContextSlimeFields(uint[] slimeFields, uint[] erasedSlimes)
        {
            UpdateSlimes(slimeFields, erasedSlimes, (a, b, i) => a[i] &= ~b[i]);
        }

        /// <summary>
        /// 全色消済スライム情報を更新します。
        /// </summary>
        /// <param name="erasedAllColorSlimes">全色消済スライム情報</param>
        /// <param name="erasedSlimes">消済スライム情報</param>
        private static void UpdateErasedAllColorSlimes(uint[] erasedAllColorSlimes, uint[] erasedSlimes)
        {
            UpdateSlimes(erasedAllColorSlimes, erasedSlimes, (a, b, i) => a[i] |= b[i]);
        }

        /// <summary>
        /// スライムの状態を更新します。
        /// </summary>
        /// <param name="a">更新対象のスライムの状態</param>
        /// <param name="b">更新に用いるスライムの状態</param>
        /// <param name="update">更新処理</param>
        private static void UpdateSlimes(uint[] a, uint[] b, Action<uint[], uint[], int> update)
        {
            Debug.Assert(a.Length == b.Length && a.Length == FieldContextConfig.FieldUnitCount,
                string.Format("要素数が不正です。{0}：{1}", a.Length, b.Length));

            for (var i = 0; i < a.Length; i++)
            {
                update(a, b, i);
            }
        }

        /// <summary>
        /// 消去パターンのリストを作成します。
        /// </summary>
        /// <returns>消去パターンのリスト</returns>
        private static List<ErasePattern> BuildErasePatternList()
        {
            var list = new List<ErasePattern>();

            // １．縦4
            list.Add(new ErasePattern(0x04040404u,
                FieldContextConfig.FieldUnitLineCount - 4 + 1,
                FieldContextConfig.VerticalLineLength - 1 + 1));

            // ２．横4
            list.Add(new ErasePattern(0x0000003cu,
                FieldContextConfig.FieldUnitLineCount - 1 + 1,
                FieldContextConfig.VerticalLineLength - 4 + 1));

            // ３．L字上1
            list.Add(new ErasePattern(0x00001c10u,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // ４．L字上２
            list.Add(new ErasePattern(0x000c0808u,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // ５．L字下1
            list.Add(new ErasePattern(0x0000101cu,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // ６．L字下2
            list.Add(new ErasePattern(0x0008080cu,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // ７．逆L字上1
            list.Add(new ErasePattern(0x00001c04u,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // ８．逆L字上2
            list.Add(new ErasePattern(0x000c0404u,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // ９．逆L字下1
            list.Add(new ErasePattern(0x0000041cu,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // １０．逆L字下2
            list.Add(new ErasePattern(0x0004040cu,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // １１．四角
            list.Add(new ErasePattern(0x00000c0cu,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // １２．縦ト
            list.Add(new ErasePattern(0x00080c08u,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // １３．横ト
            list.Add(new ErasePattern(0x0000081cu,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // １４．逆縦ト
            list.Add(new ErasePattern(0x00040c04u,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // １５．逆横ト
            list.Add(new ErasePattern(0x00001c08u,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // １６．縦鍵
            list.Add(new ErasePattern(0x00040c08u,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // １７．横鍵
            list.Add(new ErasePattern(0x00000c18u,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            // １８．逆縦鍵
            list.Add(new ErasePattern(0x00080c04u,
                FieldContextConfig.FieldUnitLineCount - 3 + 1,
                FieldContextConfig.VerticalLineLength - 2 + 1));

            // １９．逆横鍵
            list.Add(new ErasePattern(0x0000180cu,
                FieldContextConfig.FieldUnitLineCount - 2 + 1,
                FieldContextConfig.VerticalLineLength - 3 + 1));

            return list;
        }

        /// <summary>
        /// 消去パターン
        /// </summary>
        private class ErasePattern
        {
            /// <summary>
            /// 消去パターン
            /// </summary>
            public uint Pattern { get; private set; }

            /// <summary>
            /// 最大平行シフト量
            /// </summary>
            public int MaxHorizontalShift { get; private set; }

            /// <summary>
            /// 最大垂直シフト量
            /// </summary>
            public int MaxVerticalShift { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pattern">消去パターン</param>
            /// <param name="maxHorizontalShift">最大平行シフト量</param>
            /// <param name="maxVerticalShift">最大垂直シフト量</param>
            public ErasePattern(uint pattern, int maxHorizontalShift, int maxVerticalShift)
            {
                this.Pattern = pattern;
                this.MaxHorizontalShift = maxHorizontalShift;
                this.MaxVerticalShift = maxVerticalShift;
            }
        }
    }
}
