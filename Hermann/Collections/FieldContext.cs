using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Collections
{
    /// <summary>
    /// フィールドの状態を表すコンテキスト
    /// </summary>
    public enum FieldContext
    {
        /// <summary>
        /// 操作
        /// </summary>
        Command,

        /// <summary>
        /// 占有状態（上部）
        /// </summary>
        OccupiedUpper,

        /// <summary>
        /// 占有状態（下部）
        /// </summary>
        OccupiedLower,
        
        /// <summary>
        /// 操作可能状態（上部）
        /// </summary>
        MovableUpper,

        /// <summary>
        /// 操作可能状態（下部）
        /// </summary>
        MovableLower,

        /// <summary>
        /// 赤（上部）
        /// </summary>
        RedUpper,

        /// <summary>
        /// 赤（下部）
        /// </summary>
        RedLower,

        /// <summary>
        /// 青（上部）
        /// </summary>
        BlueUpper,

        /// <summary>
        /// 青（下部）
        /// </summary>
        BlueLower,

        /// <summary>
        /// 緑（上部）
        /// </summary>
        GreenUpper,

        /// <summary>
        /// 緑（下部）
        /// </summary>
        GreenLower,

        /// <summary>
        /// 黄（上部）
        /// </summary>
        YellowUpper,

        /// <summary>
        /// 黄（下部）
        /// </summary>
        YellowLower,

        /// <summary>
        /// 紫（上部）
        /// </summary>
        PurpleUpper,

        /// <summary>
        /// 紫（下部）
        /// </summary>
        PurpleLower,
    }

    /// <summary>
    /// フィールドコンテキストのコレクション
    /// </summary>
    public class FieldContextCollection
    {
        /// <summary>
        /// 占有状態
        /// </summary>
        public FieldContext OccupiedField { get; set; }

        /// <summary>
        /// 操作可能状態
        /// </summary>
        public FieldContext MovableField { get; set; }

        /// <summary>
        /// 色リスト
        /// </summary>
        public Dictionary<SlimeColor, FieldContext> ColorFields { get; set; }
    }

    /// <summary>
    /// 拡張フィールドコンテキスト
    /// </summary>
    public static class FieldContextExtension
    {
        /// <summary>
        /// フィールドにおける位置を示します。
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// 上部
            /// </summary>
            Upper,

            /// <summary>
            /// 下部
            /// </summary>
            Lower
        }

        /// <summary>
        /// 上部のコレクション
        /// </summary>
        private static FieldContextCollection upperCollection = CreateUpperCollection();

        /// <summary>
        /// 下部のコレクション
        /// </summary>
        private static FieldContextCollection lowerCollection = CreateLowerCollection();

        /// <summary>
        /// コレクションを取得します。
        /// </summary>
        /// <param name="position">ポジション</param>
        /// <returns>コレクション</returns>
        public static FieldContextCollection GetCollection(Position position)
        {
            return (position == Position.Upper) ? upperCollection : lowerCollection;
        }

        /// <summary>
        /// 要素数を取得します。
        /// </summary>
        /// <param name="context">フィールドコンテキスト</param>
        /// <returns>要素数</returns>
        public static int Count()
        {
            return Enum.GetNames(typeof(FieldContext)).Length;
        }

        /// <summary>
        /// 上部コレクションを作成します。
        /// </summary>
        /// <returns>上部コレクション</returns>
        private static FieldContextCollection CreateUpperCollection()
        {
            var collection = new FieldContextCollection();
            collection.OccupiedField = FieldContext.OccupiedUpper;
            collection.MovableField = FieldContext.MovableUpper;
            collection.ColorFields = GetUpperColorCollection();
            return collection;
        }

        /// <summary>
        /// 上部コレクションを作成します。
        /// </summary>
        /// <returns>下部コレクション</returns>
        private static FieldContextCollection CreateLowerCollection()
        {
            var collection = new FieldContextCollection();
            collection.OccupiedField = FieldContext.OccupiedLower;
            collection.MovableField = FieldContext.MovableLower;
            collection.ColorFields = GetLowerColorCollection();
            return collection;
        }

        /// <summary>
        /// 上部の色コレクションを取得します。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <returns>上部の色コレクション</returns>
        private static Dictionary<SlimeColor, FieldContext> GetUpperColorCollection()
        {
            var collection = new Dictionary<SlimeColor, FieldContext>();
            collection.Add(SlimeColor.Red, FieldContext.RedUpper);
            collection.Add(SlimeColor.Blue, FieldContext.BlueUpper);
            collection.Add(SlimeColor.Green, FieldContext.GreenUpper);
            collection.Add(SlimeColor.Yellow, FieldContext.YellowUpper);
            collection.Add(SlimeColor.Purple, FieldContext.PurpleUpper);
            return collection;
        }

        /// <summary>
        /// 下部の色コレクションを取得します。
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <returns>下部の色コレクション</returns>
        private static Dictionary<SlimeColor, FieldContext> GetLowerColorCollection()
        {
            var collection = new Dictionary<SlimeColor, FieldContext>();
            collection.Add(SlimeColor.Red, FieldContext.RedLower);
            collection.Add(SlimeColor.Blue, FieldContext.BlueLower);
            collection.Add(SlimeColor.Green, FieldContext.GreenLower);
            collection.Add(SlimeColor.Yellow, FieldContext.YellowLower);
            collection.Add(SlimeColor.Purple, FieldContext.PurpleLower);
            return collection;
        }
    }
}
