using Hermann.Collections;
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
            /// フィールド状態開始行
            /// </summary>
            FieldStart = 2,
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

        /// <summary>
        /// 方向の変換辞書
        /// </summary>
        private static readonly Dictionary<Command.Direction, string> Directions = buildDirections();

        /// <summary>
        /// 方向の逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<string, Command.Direction> ReverseDirections = buildReverseDirections();

        /// <summary>
        /// スライムの変換辞書
        /// </summary>
        private static readonly Dictionary<Slime, char> Slimes = buildSlimes();

        /// <summary>
        /// スライムの逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<char, Slime> ReverseSlimes = buildReverseSlimes();

        /// <summary>
        /// 移動可能なスライムの変換辞書
        /// </summary>
        private static readonly Dictionary<Slime, char> MovableSlimes = buildMovableSlimes();

        /// <summary>
        /// 移動可能なスライムの逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<char, Slime> ReverseMovableSlimes = buildReverseMovableSlimes();

        /// <summary>
        /// 方向の変換を行います。
        /// </summary>
        /// <param name="directions">変換前の方向</param>
        /// <returns>変換後の方向</returns>
        public static string ConvertDirection(Command.Direction directions)
        {
            return Directions[directions];
        }

        /// <summary>
        /// 方向の変換を行います。
        /// </summary>
        /// <param name="directions">変換前の方向</param>
        /// <returns>変換後の方向</returns>
        public static Command.Direction ConvertDirection(string directions)
        {
            return ReverseDirections[directions];
        }

        /// <summary>
        /// スライムの変換を行います。
        /// </summary>
        /// <param name="slime">変換前のスライム</param>
        /// <returns>変換後のスライム</returns>
        public static char ConvertSlime(Slime slime)
        {
            return Slimes[slime];
        }

        /// <summary>
        /// スライムの変換を行います。
        /// </summary>
        /// <param name="slime">変換前のスライム</param>
        /// <returns>変換後のスライム</returns>
        public static Slime ConvertSlime(char slime)
        {
            return ReverseSlimes[slime];
        }

        /// <summary>
        /// 移動可能なスライムの変換を行います。
        /// </summary>
        /// <param name="slime">変換前の移動可能なスライム</param>
        /// <returns>変換後の移動可能なスライム</returns>
        public static char ConvertMovableSlime(Slime slime)
        {
            return MovableSlimes[slime];
        }

        /// <summary>
        /// 移動可能なスライムの変換を行います。
        /// </summary>
        /// <param name="slime">変換前の移動可能なスライム</param>
        /// <returns>変換後の移動可能なスライム</returns>
        public static Slime ConvertMovableSlime(char slime)
        {
            return ReverseMovableSlimes[slime];
        }

        /// <summary>
        /// 方向の変換辞書を作成します。
        /// </summary>
        /// <returns>方向の変換辞書</returns>
        private static Dictionary<Command.Direction, string> buildDirections()
        {
            var directions = new Dictionary<Command.Direction, string>();
            directions.Add(Command.Direction.None, DirectionNone);
            directions.Add(Command.Direction.Down, DirectionDown);
            directions.Add(Command.Direction.Left, DirectionLeft);
            directions.Add(Command.Direction.Right, DirectionRight);
            directions.Add(Command.Direction.Up, DirectionUp);
            return directions;
        }

        /// <summary>
        /// 方向の逆引き変換辞書を作成します。
        /// </summary>
        /// <returns>方向の逆引き変換辞書</returns>
        private static Dictionary<string, Command.Direction> buildReverseDirections()
        {
            var directions = new Dictionary<string, Command.Direction>();
            directions.Add(DirectionNone, Command.Direction.None);
            directions.Add(DirectionDown, Command.Direction.Down);
            directions.Add(DirectionLeft, Command.Direction.Left);
            directions.Add(DirectionRight, Command.Direction.Right);
            directions.Add(DirectionUp, Command.Direction.Up);
            return directions;
        }

        /// <summary>
        /// スライムの変換辞書を作成します。
        /// </summary>
        /// <returns>スライムの変換辞書</returns>
        private static Dictionary<Slime, char> buildSlimes()
        {
            var slimes = new Dictionary<Slime, char>();
            slimes.Add(Slime.None, SlimeNone);
            slimes.Add(Slime.Blue, SlimeBlue);
            slimes.Add(Slime.Green, SlimeGreen);
            slimes.Add(Slime.Purple, SlimePurple);
            slimes.Add(Slime.Red, SlimeRed);
            slimes.Add(Slime.Yellow, SlimeYellow);
            slimes.Add(Slime.Obstruction, SlimeObstruction);
            return slimes;
        }

        /// <summary>
        /// スライムの変換辞書を作成します。
        /// </summary>
        /// <returns>スライムの変換辞書</returns>
        private static Dictionary<char, Slime> buildReverseSlimes()
        {
            var slimes = new Dictionary<char, Slime>();
            slimes.Add(SlimeNone, Slime.None);
            slimes.Add(SlimeBlue, Slime.Blue);
            slimes.Add(SlimeGreen, Slime.Green);
            slimes.Add(SlimePurple, Slime.Purple);
            slimes.Add(SlimeRed, Slime.Red);
            slimes.Add(SlimeYellow, Slime.Yellow);
            slimes.Add(SlimeObstruction, Slime.Obstruction);
            return slimes;
        }

        /// <summary>
        /// 移動可能なスライムの変換辞書を作成します。
        /// </summary>
        /// <returns>移動可能なスライムの変換辞書</returns>
        private static Dictionary<Slime, char> buildMovableSlimes()
        {
            var slimes = new Dictionary<Slime, char>();
            slimes.Add(Slime.Blue, MovableSlimeBlue);
            slimes.Add(Slime.Green, MovableSlimeGreen);
            slimes.Add(Slime.Purple, MovableSlimePurple);
            slimes.Add(Slime.Red, MovableSlimeRed);
            slimes.Add(Slime.Yellow, MovableSlimeYellow);
            slimes.Add(Slime.Obstruction, SlimeObstruction);
            return slimes;
        }

        /// <summary>
        /// 移動可能なスライムの逆引き変換辞書を作成します。
        /// </summary>
        /// <returns>移動可能なスライムの変換辞書</returns>
        private static Dictionary<char, Slime> buildReverseMovableSlimes()
        {
            var slimes = new Dictionary<char, Slime>();
            slimes.Add(MovableSlimeBlue, Slime.Blue);
            slimes.Add(MovableSlimeGreen, Slime.Green);
            slimes.Add(MovableSlimePurple, Slime.Purple);
            slimes.Add(MovableSlimeRed, Slime.Red);
            slimes.Add(MovableSlimeYellow, Slime.Yellow);
            slimes.Add(SlimeObstruction, Slime.Obstruction);
            return slimes;
        }
    }
}
