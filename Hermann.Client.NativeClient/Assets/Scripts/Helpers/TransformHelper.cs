using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Transformに関する補助機能を提供します。
    /// </summary>
    public static class TransformHelper
    {
        /// <summary>
        /// サイズを取得します。
        /// </summary>
        /// <param name="obj">RectTransform</param>
        /// <returns>サイズ</returns>
        public static Vector2 GetSize(RectTransform obj)
        {
            return obj.sizeDelta;
        }

        /// <summary>
        /// サイズを取得します。
        /// </summary>
        /// <param name="obj">IClippable</param>
        /// <returns>サイズ</returns>
        public static Vector2 GetSize(IClippable obj)
        {
            return GetSize(obj.rectTransform);
        }

        /// <summary>
        /// サイズをセットします。
        /// </summary>
        /// <param name="obj">IClippable</param>
        /// <param name="size">サイズ</param>
        public static void SetSize(IClippable obj, Vector2 size)
        {
            obj.rectTransform.sizeDelta = size;
        }

        /// <summary>
        /// 比率調整されたサイズを取得します。
        /// </summary>
        /// <param name="obj">RectTransform</param>
        /// <returns>比率調整されたサイズ</returns>
        public static Vector2 GetScaledSize(RectTransform obj)
        {
            return new Vector2(GetSize(obj).x * GetScale(obj).x, GetSize(obj).y * GetScale(obj).y);
        }

        /// <summary>
        /// 比率調整されたサイズを取得します。
        /// </summary>
        /// <param name="obj">Image</param>
        /// <returns>比率調整されたサイズ</returns>
        public static Vector2 GetScaledSize(Image obj)
        {
            return new Vector2(GetSize(obj).x * GetScale(obj).x, GetSize(obj).y * GetScale(obj).y);
        }

        /// <summary>
        /// 元のサイズを取得します。
        /// </summary>
        /// <param name="obj">ILayoutElement</param>
        /// <returns>元のサイズ</returns>
        public static Vector2 GetOriginalSize(ILayoutElement obj)
        {
            return new Vector2(obj.preferredWidth, obj.preferredHeight);
        }

        /// <summary>
        /// サイズを取得します。
        /// </summary>
        /// <param name="obj">RectTransform</param>
        /// <returns>サイズ</returns>
        public static Vector3 GetScale(RectTransform obj)
        {
            return obj.localScale;
        }

        /// <summary>
        /// サイズを取得します。
        /// </summary>
        /// <param name="obj">Transform</param>
        /// <returns>サイズ</returns>
        public static Vector3 GetScale(Transform obj)
        {
            return obj.localScale;
        }

        /// <summary>
        /// サイズを取得します。
        /// </summary>
        /// <param name="obj">IClippable</param>
        /// <returns>サイズ</returns>
        public static Vector3 GetScale(IClippable obj)
        {
            return GetScale(obj.rectTransform);
        }

        /// <summary>
        /// 比率をセットします。
        /// </summary>
        /// <param name="obj">IClippable</param>
        /// <param name="scale">比率</param>
        public static void SetScale(IClippable obj, Vector3 scale)
        {
            obj.rectTransform.localScale = scale;
        }

        /// <summary>
        /// 位置をセットします。
        /// </summary>
        /// <param name="obj">RectTransform</param>
        /// <param name="position">位置</param>
        public static void SetPosition(RectTransform obj, Vector3 position)
        {
            obj.localPosition = position;
        }
    }
}
