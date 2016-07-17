using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Api
{
    /// <summary>
    /// 標準テキストに関する情報/機能を提供します。
    /// </summary>
    public static class SimpleText
    {
        /// <summary>
        /// 行数に関する情報を提供します。
        /// </summary>
        public enum Lines
        {
            /// <summary>
            /// プレイヤ
            /// </summary>
            Player = 0,

            /// <summary>
            /// 方向
            /// </summary>
            Direction = 1,

            /// <summary>
            /// 上部開始行
            /// </summary>
            UpperStart = 2,

            /// <summary>
            /// 上部終了行
            /// </summary>
            UpperEnd = 9,

            /// <summary>
            /// 下部開始行
            /// </summary>
            LowerStart = 10,

            /// <summary>
            /// 下部終了行
            /// </summary>
            LowerEnd = 13,
        }

        /// <summary>
        /// 合計行数
        /// </summary>
        public const int LineCount = 14;

        /// <summary>
        /// 方向：無
        /// </summary>
        public const string DirectionNone = "無";

        /// <summary>
        /// 方向：上
        /// </summary>
        public const string DirectionUp = "上";

        /// <summary>
        /// 方向：下
        /// </summary>
        public const string DirectionDown = "下";

        /// <summary>
        /// 方向：左
        /// </summary>
        public const string DirectionLeft = "左";

        /// <summary>
        /// 方向：右
        /// </summary>
        public const string DirectionRight = "右";

        /// <summary>
        /// スライム：無
        /// </summary>
        public const char SlimeNone = 'ロ';

        /// <summary>
        /// スライム：赤
        /// </summary>
        public const char SlimeRed = '赤';

        /// <summary>
        /// スライム：青
        /// </summary>
        public const char SlimeBlue = '青';

        /// <summary>
        /// スライム：黄
        /// </summary>
        public const char SlimeYellow = '黄';

        /// <summary>
        /// スライム：紫
        /// </summary>
        public const char SlimePurple = '紫';

        /// <summary>
        /// スライム：緑
        /// </summary>
        public const char SlimeGreen = '緑';

        /// <summary>
        /// スライム：おじゃま
        /// </summary>
        public const char SlimeObstruction = 'お';

        /// <summary>
        /// 操作対象スライム：赤
        /// </summary>
        public const char MovableSlimeRed = 'Ｒ';

        /// <summary>
        /// 操作対象スライム：赤
        /// </summary>
        public const char MovableSlimeBlue = 'Ｂ';

        /// <summary>
        /// 操作対象スライム：赤
        /// </summary>
        public const char MovableSlimeYellow = 'Ｙ';

        /// <summary>
        /// 操作対象スライム：赤
        /// </summary>
        public const char MovableSlimePurple = 'Ｐ';

        /// <summary>
        /// 操作対象スライム：赤
        /// </summary>
        public const char MovableSlimeGreen = 'Ｇ';
    }
}
