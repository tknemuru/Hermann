using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Models
{
    /// <summary>
    /// 方向
    /// </summary>
    public enum Direction : uint
    {
        /// <summary>
        /// 無し
        /// </summary>
        None = 0x0u,

        /// <summary>
        /// 上
        /// </summary>
        Up = 0x1u,

        /// <summary>
        /// 下
        /// </summary>
        Down = 0x2u,

        /// <summary>
        /// 左
        /// </summary>
        Left = 0x3u,

        /// <summary>
        /// 右
        /// </summary>
        Right = 0x4u,
    }

    /// <summary>
    /// 方向拡張
    /// </summary>
    public static class ExtensionDirection
    {
        /// <summary>
        /// 方向リスト
        /// </summary>
        public static readonly IEnumerable<Direction> Directions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ExtensionDirection()
        {
            Directions = ((IEnumerable<Direction>)Enum.GetValues(typeof(Direction)));
        }

        /// <summary>
        /// 方向名を取得します。
        /// </summary>
        /// <param name="direction">方向</param>
        /// <returns>方向名</returns>
        public static string GetName(this Direction direction)
        {
            var name = string.Empty;

            switch (direction)
            {
                case Direction.Down:
                    name = "Down";
                    break;
                case Direction.Left:
                    name = "Left";
                    break;
                case Direction.Right:
                    name = "Right";
                    break;
                case Direction.Up:
                    name = "Up";
                    break;
                default:
                    throw new ArgumentException("方向が不正です。");
            }

            return name;
        }
    }
}
