using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// フィールド上の位置に関する変換機能を提供します。
    /// </summary>
    public static class FieldPositionConverter
    {
        private const float PaddingRateX = 1.0f;
        private const float BaseY = 300;
        private const float BaseZ = -1;
        private const float UnitX = 30;
        private const float UnitY = 30;

        /// <summary>
        /// 指定したユニット・ユニット内のインデックスに対応したフィールド座標を取得します。
        /// </summary>
        /// <param name="fieldWidth">フィールドの横幅</param>
        /// <param name="slimeWidth">スライムの横幅</param>
        /// <param name="unit">フィールドユニット</param>
        /// <param name="index">フィールドインデックス</param>
        /// <returns>フィールド座標</returns>
        public static Vector3 GetSlimeFieldPosition(float fieldWidth, float slimeWidth, int unit, int index)
        {
            var x = (fieldWidth / 2f) - (slimeWidth / 2f);

            var line = FieldContextHelper.GetLineIndex(unit, index);
            var column = FieldContextHelper.GetColumnIndex(index);

            return new Vector3((x - (slimeWidth * column)), BaseY + (UnitY * line), BaseZ);
        }
    }
}
