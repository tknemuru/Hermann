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
    }
}
