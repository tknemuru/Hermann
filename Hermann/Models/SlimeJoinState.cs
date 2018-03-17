using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Models
{
    /// <summary>
    /// スライムの結合状態
    /// </summary>
    public enum SlimeJoinState : uint
    {
        /// <summary>
        /// 初期状態
        /// </summary>
        Default = 0b0000,

        /// <summary>
        /// 下結合
        /// </summary>
        Down = 0b0001,

        /// <summary>
        /// 上結合
        /// </summary>
        Up = 0b0010,

        /// <summary>
        /// 上下結合
        /// </summary>
        UpDown = 0b0001 | 0b0010,

        /// <summary>
        /// 右結合
        /// </summary>
        Right = 0b0100,

        /// <summary>
        /// 下右結合
        /// </summary>
        DownRight = 0b0001 | 0b0100,

        /// <summary>
        /// 上右結合
        /// </summary>
        UpRight = 0b0010 | 0b0100,

        /// <summary>
        /// 上下右結合
        /// </summary>
        UpDownRight = 0b0010 | 0b0001 | 0b0100,

        /// <summary>
        /// 左結合
        /// </summary>
        Left = 0b1000,

        /// <summary>
        /// 下左結合
        /// </summary>
        DownLeft = 0b0001 | 0b1000,

        /// <summary>
        /// 上左結合
        /// </summary>
        UpLeft = 0b0010 | 0b1000,

        /// <summary>
        /// 上下左
        /// </summary>
        UpDownLeft = 0b0010 | 0b0001 | 0b1000,

        /// <summary>
        /// 右左結合
        /// </summary>
        RightLeft = 0b0100 | 0b1000,

        /// <summary>
        /// 下右左結合
        /// </summary>
        DownRightLeft = 0b0001 | 0b0100 | 0b1000,

        /// <summary>
        /// 上右左結合
        /// </summary>
        UpRightLeft = 0b0010 | 0b0100 | 0b1000,

        /// <summary>
        /// 上下右左結合
        /// </summary>
        UpDownRightLeft = 0b0010 | 0b0001 | 0b0100 | 0b1000,
    }

    /// <summary>
    /// スライム結合状態拡張
    /// </summary>
    public static class ExtensionSlimeJoinState
    {
        /// <summary>
        /// スライム結合状態名を取得します。
        /// </summary>
        /// <param name="state">スライム結合状態</param>
        /// <returns>スライム結合状態名</returns>
        public static string GetName(this SlimeJoinState state)
        {
            var name = string.Empty;
            switch (state)
            {
                case SlimeJoinState.Default:
                    name = "Default";
                    break;
                case SlimeJoinState.Down:
                    name = "Down";
                    break;
                case SlimeJoinState.DownLeft:
                    name = "Down_Left";
                    break;
                case SlimeJoinState.DownRight:
                    name = "Down_Right";
                    break;
                case SlimeJoinState.DownRightLeft:
                    name = "Down_Right_Left";
                    break;
                case SlimeJoinState.Left:
                    name = "Left";
                    break;
                case SlimeJoinState.Right:
                    name = "Right";
                    break;
                case SlimeJoinState.RightLeft:
                    name = "Right_Left";
                    break;
                case SlimeJoinState.Up:
                    name = "Up";
                    break;
                case SlimeJoinState.UpDown:
                    name = "Up_Down";
                    break;
                case SlimeJoinState.UpDownLeft:
                    name = "Up_Down_Left";
                    break;
                case SlimeJoinState.UpDownRight:
                    name = "Up_Down_Right";
                    break;
                case SlimeJoinState.UpDownRightLeft:
                    name = "Up_Down_Right_Left";
                    break;
                case SlimeJoinState.UpLeft:
                    name = "Up_Left";
                    break;
                case SlimeJoinState.UpRight:
                    name = "Up_Right";
                    break;
                case SlimeJoinState.UpRightLeft:
                    name = "Up_Right_Left";
                    break;
                default:
                    throw new ArgumentException("スライム結合状態が不正です");
            }
            return name;
        }
    }
}
