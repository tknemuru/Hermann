using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Contexts
{
    /// <summary>
    /// フィールドの設定情報
    /// </summary>
    public static class FieldContextConfig
    {
        /// <summary>
        /// フィールドを分割したユニットの要素数
        /// </summary>
        public const int FieldUnitCount = 5;

        /// <summary>
        /// フィールドユニット内の行数
        /// </summary>
        public const int FieldUnitLineCount = 4;

        /// <summary>
        /// フィールドの行数
        /// </summary>
        public const int FieldLineCount = FieldUnitCount * FieldUnitLineCount;

        /// <summary>
        /// フィールドを分割したユニット1つあたりのビット数
        /// </summary>
        public const int FieldUnitBitCount = 32;

        /// <summary>
        /// 1行あたりのビット数
        /// </summary>
        public const int OneLineBitCount = 8;

        /// <summary>
        /// 使用するスライムの数
        /// </summary>
        public const int UsingSlimeCount = 4;

        /// <summary>
        /// 隠しフィールドの最小インデックス
        /// </summary>
        public const int MinHiddenUnitIndex = 0;

        /// <summary>
        /// 隠しフィールドの最大インデックス
        /// </summary>
        public const int MaxHiddenUnitIndex = 1;

        /// <summary>
        /// 移動可能なスライムの初期シフト量（おじゃまスライム落下前）
        /// </summary>
        public const int MovableSlimeInitialShiftBeforeDroped = 5;

        /// <summary>
        /// 移動可能なスライムの調整シフト量（おじゃまスライム落下後）
        /// </summary>
        public const int MovableSlimeInitialShiftAfterDroped = 21;

        /// <summary>
        /// おじゃまスライムを配置する最小シフト量
        /// </summary>
        public const int MinObstructionSlimeSetShift = 16;

        /// <summary>
        /// 最大設置残タイム（ミリ秒）
        /// </summary>
        public const long MaxBuiltRemainingTime = 260;

        /// <summary>
        /// 最大設置フレーム回数
        /// </summary>
        public const long MaxBuiltRemainingFrameCount = 12;

        /// <summary>
        /// 縦の列数
        /// </summary>
        public const int VerticalLineLength = 6;

        /// <summary>
        /// 横の行数
        /// </summary>
        public const int HorizontalLineLength = FieldUnitLineCount * FieldUnitCount;

        /// <summary>
        /// 回転方向の初期値
        /// </summary>
        public const Direction InitialDirection = Direction.Right;
    }
}
