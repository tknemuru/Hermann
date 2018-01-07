using Hermann.Collections;
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
                var fields = context.SlimeFields[(int)player].Value[slime];

                // 対象スライムの削除情報を初期化
                var erasedSlimes = CreateInitialErasedSlimes();
                Debug.Assert(erasedSlimes.Length == fields.Length, string.Format("フィールドの要素数が不正です。要素数：{0}", fields.Length));
              
                // 最後のユニットの先頭行まで繰り返す
                for (var horizontalIndex = 0; horizontalIndex < (FieldContextConfig.FieldLineCount - (FieldContextConfig.FieldUnitLineCount - 1)); horizontalIndex++)
                {
                    // ユニットをまたいだ削除パターンを検出するために、開始行を1行ずつずらしながらユニットをマージして一つのユニットを作成する
                    var mergedField = MergeFields(fields, horizontalIndex);
                    var mergedErasedSlime = 0u;
                    
                    // マージしたユニットに対する削除情報を作成する
                    // １．縦4
                    uint vertical4 = 0x04040404u;
                    for (var i = 0; i < FieldContextConfig.VerticalLineLength; i++)
                    {
                        if ((mergedField & vertical4) == vertical4)
                        {
                            mergedErasedSlime |= vertical4;
                        }
                        vertical4 <<= 1;
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
            context.SlimeFields[(int)player].Value[Slime.Erased] = erasedAllColorSlimes;

            // 連鎖回数をインクリメントする

            // 消す対象が存在しなかった場合は連鎖回数を0に戻す
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
    }
}
