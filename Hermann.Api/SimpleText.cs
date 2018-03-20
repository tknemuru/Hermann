using Hermann.Models;
using Hermann.Contexts;
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
        /// 仕切り文字
        /// </summary>
        public static class Separator
        {
            /// <summary>
            /// キーと値
            /// </summary>
            public const char KeyValue = ':';

            /// <summary>
            /// プレイヤ
            /// </summary>
            public const char Player = '|';
        }

        /// <summary>
        /// フィールドイベントを示す記号
        /// </summary>
        public static class FieldEventSymbol
        {
            /// <summary>
            /// 無
            /// </summary>
            public const string None = "無";

            /// <summary>
            /// 連鎖開始
            /// </summary>
            public const string StartChain = "連鎖開始";

            /// <summary>
            /// 消済マーク
            /// </summary>
            public const string MarkErasing = "消済マーク";

            /// <summary>
            /// 削除
            /// </summary>
            public const string Erase = "削除";

            /// <summary>
            /// おじゃまスライム落下
            /// </summary>
            public const string DropObstructions = "おじゃまスライム落下";
        }

        /// <summary>
        /// 方向を示す記号
        /// </summary>
        public static class DirectionSymbol
        {
            /// <summary>
            /// 無
            /// </summary>
            public const string None = "無";

            /// <summary>
            /// 上
            /// </summary>
            public const string Up = "上";

            /// <summary>
            /// 下
            /// </summary>
            public const string Down = "下";

            /// <summary>
            /// 左
            /// </summary>
            public const string Left = "左";

            /// <summary>
            /// 右
            /// </summary>
            public const string Right = "右";
        }

        /// <summary>
        /// スライムを示す記号
        /// </summary>
        public static class SlimeSymbol
        {
            /// <summary>
            /// 無
            /// </summary>
            public const char None = 'ロ';

            /// <summary>
            /// 無（隠し領域）
            /// </summary>
            public const char NoneHidden = '・';

            /// <summary>
            /// 赤
            /// </summary>
            public const char Red = '赤';

            /// <summary>
            /// 青
            /// </summary>
            public const char Blue = '青';

            /// <summary>
            /// 黄
            /// </summary>
            public const char Yellow = '黄';

            /// <summary>
            /// 紫
            /// </summary>
            public const char Purple = '紫';

            /// <summary>
            /// 緑
            /// </summary>
            public const char Green = '緑';

            /// <summary>
            /// おじゃま
            /// </summary>
            public const char Obstruction = 'お';

            /// <summary>
            /// 消済
            /// </summary>
            public const char Erased = '消';
        }

        /// <summary>
        /// 操作対象スライムを示す記号
        /// </summary>
        public static class MovableSlimeSymbol
        {
            /// <summary>
            /// 赤
            /// </summary>
            public const char Red = 'Ｒ';

            /// <summary>
            /// 青
            /// </summary>
            public const char Blue = 'Ｂ';

            /// <summary>
            /// 黄
            /// </summary>
            public const char Yellow = 'Ｙ';

            /// <summary>
            /// 紫
            /// </summary>
            public const char Purple = 'Ｐ';

            /// <summary>
            /// 緑
            /// </summary>
            public const char Green = 'Ｇ';
        }

        /// <summary>
        /// おじゃまぷよを示す記号
        /// </summary>
        public static class ObstructionSlimeSymbol
        {
            /// <summary>
            /// 小
            /// </summary>
            public const char Small = '小';

            /// <summary>
            /// 大
            /// </summary>
            public const char Big = '大';

            /// <summary>
            /// 岩
            /// </summary>
            public const char Rock = '岩';

            /// <summary>
            /// 星
            /// </summary>
            public const char Star = '星';

            /// <summary>
            /// 月
            /// </summary>
            public const char Moon = '月';

            /// <summary>
            /// 王冠
            /// </summary>
            public const char Crown = '王';

            /// <summary>
            /// 彗星
            /// </summary>
            public const char Comet = '彗';
        }

        /// <summary>
        /// 情報のキー
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// プレイヤ
            /// </summary>
            public const string Player = "プレイヤ";

            /// <summary>
            /// イベント
            /// </summary>
            public const string FieldEvent = "イベント";

            /// <summary>
            /// 操作方向
            /// </summary>
            public const string Direction = "操作方向";

            /// <summary>
            /// 回転方向
            /// </summary>
            public const string RotationDirection = "回転方向";

            /// <summary>
            /// 経過時間
            /// </summary>
            public const string Time = "経過時間";

            /// <summary>
            /// 接地
            /// </summary>
            public const string Ground = "接地";

            /// <summary>
            /// 接地残タイム
            /// </summary>
            public const string BuiltRemainingTime = "設置残タイム";

            /// <summary>
            /// 得点
            /// </summary>
            public const string Score = "得点";

            /// <summary>
            /// 使用済得点
            /// </summary>
            public const string UsedScore = "使用済得点";

            /// <summary>
            /// 連鎖
            /// </summary>
            public const string Chain = "連鎖";

            /// <summary>
            /// 相殺
            /// </summary>
            public const string Offset = "相殺";

            /// <summary>
            /// 全消
            /// </summary>
            public const string AllErase = "全消";

            /// <summary>
            /// 勝数
            /// </summary>
            public const string WinCount = "勝数";

            /// <summary>
            /// 使用スライム
            /// </summary>
            public const string UsingSlimes = "使用スライム";

            /// <summary>
            /// おじゃまスライム
            /// </summary>
            public const string ObstructionSlime = "おじゃまスライム";

            /// <summary>
            /// フィールド
            /// </summary>
            public const string Field = "フィールド";
        }

        /// <summary>
        /// 行数に関する情報を提供します。
        /// </summary>
        public static class Length
        {
            /// <summary>
            /// デフォルト
            /// </summary>
            public const int Default = 1;

            /// <summary>
            /// おじゃまぷよ
            /// </summary>
            public const int Obstruction = 2;

            /// <summary>
            /// フィールド
            /// </summary>
            public const int Field = FieldContextConfig.FieldLineCount + 1;

            /// <summary>
            /// 合計行数
            /// </summary>
            public const int Sum = 37;
        }

        /// <summary>
        /// インデックスに関する情報を提供します。
        /// </summary>
        public static class Index
        {
            /// <summary>
            /// NEXTスライム：1つめの集合体の1つめのスライム
            /// </summary>
            public const int NextSlimeFirstUnitFirstSlime = 8;

            /// <summary>
            /// NEXTスライム：1つめの集合体の2つめのスライム
            /// </summary>
            public const int NextSlimeFirstUnitSecondSlime = 9;

            /// <summary>
            /// NEXTスライム：2つめの集合体の1つめのスライム
            /// </summary>
            public const int NextSlimeSecondUnitFirstSlime = 11;

            /// <summary>
            /// NEXTスライム：2つめの集合体の2つめのスライム
            /// </summary>
            public const int NextSlimeSecondUnitSecondSlime = 12;
        }

        /// <summary>
        /// イベントの変換辞書
        /// </summary>
        private static readonly Dictionary<FieldEvent, string> FieldEvents = BuildFieldEvents();

        /// <summary>
        /// イベントの逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<string, FieldEvent> ReverseFieldEvents = BuildReverseFieldEvents();

        /// <summary>
        /// 方向の変換辞書
        /// </summary>
        private static readonly Dictionary<Direction, string> Directions = BuildDirections();

        /// <summary>
        /// 方向の逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<string, Direction> ReverseDirections = BuildReverseDirections();

        /// <summary>
        /// スライムの変換辞書
        /// </summary>
        private static readonly Dictionary<Slime, char> Slimes = BuildSlimes();

        /// <summary>
        /// スライムの逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<char, Slime> ReverseSlimes = BuildReverseSlimes();

        /// <summary>
        /// 移動可能なスライムの変換辞書
        /// </summary>
        private static readonly Dictionary<Slime, char> MovableSlimes = BuildMovableSlimes();

        /// <summary>
        /// 移動可能なスライムの逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<char, Slime> ReverseMovableSlimes = BuildReverseMovableSlimes();

        /// <summary>
        /// 複数行情報の行数辞書
        /// </summary>
        private static readonly Dictionary<string, int> MultiLineLengths = BuildMultiLineLengths();

        /// <summary>
        /// おじゃまぷよの変換辞書
        /// </summary>
        private static readonly Dictionary<ObstructionSlime, char> ObstructionSlimes = BuildObstructionSlimes();

        /// <summary>
        /// おじゃまぷよの逆引き変換辞書
        /// </summary>
        private static readonly Dictionary<char, ObstructionSlime> ReverseObstructionSlimes = BuildReverseObstructionSlimes();

        /// <summary>
        /// NEXTスライムのインデックスの変換辞書
        /// </summary>
        private static readonly Dictionary<int, NextSlime.Index> NextSlimeIndexs = BuildNextSlimeIndexs();

        /// <summary>
        /// 移動可能なスライムのインデックスの変換辞書
        /// </summary>
        private static readonly Dictionary<int, MovableSlime.UnitIndex> MovableSlimeIndexs = BuildMovableSlimeIndexs();

        /// <summary>
        /// フィールドイベントの変換を行います。
        /// </summary>
        /// <param name="fieldEvent">変換前のフィールドイベント</param>
        /// <returns>変換後のフィールドイベント</returns>
        public static string ConvertFieldEvent(FieldEvent fieldEvent)
        {
            return FieldEvents[fieldEvent];
        }

        /// <summary>
        /// フィールドイベントの変換を行います。
        /// </summary>
        /// <param name="fieldEvent">変換前のフィールドイベント</param>
        /// <returns>変換後のフィールドイベント</returns>
        public static FieldEvent ConvertFieldEvent(string fieldEvent)
        {
            return ReverseFieldEvents[fieldEvent];
        }

        /// <summary>
        /// 方向の変換を行います。
        /// </summary>
        /// <param name="direction">変換前の方向</param>
        /// <returns>変換後の方向</returns>
        public static string ConvertDirection(Direction direction)
        {
            return Directions[direction];
        }

        /// <summary>
        /// 方向の変換を行います。
        /// </summary>
        /// <param name="direction">変換前の方向</param>
        /// <returns>変換後の方向</returns>
        public static Direction ConvertDirection(string direction)
        {
            return ReverseDirections[direction];
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
        /// おじゃまスライムの変換を行います。
        /// </summary>
        /// <param name="slime">変換前のおじゃまスライム</param>
        /// <returns>変換後のおじゃまスライム</returns>
        public static char ConvertObstructionSlime(ObstructionSlime slime)
        {
            return ObstructionSlimes[slime];
        }

        /// <summary>
        /// おじゃまスライムの変換を行います。
        /// </summary>
        /// <param name="slime">変換前のおじゃまスライム</param>
        /// <returns>変換後のおじゃまスライム</returns>
        public static ObstructionSlime ConvertObstructionSlime(char slime)
        {
            return ReverseObstructionSlimes[slime];
        }

        /// <summary>
        /// NEXTスライムのインデックスの変換を行います。
        /// </summary>
        /// <param name="lineIndex">フィールドの行インデックス</param>
        /// <returns>NEXTスライムのインデックス</returns>
        public static NextSlime.Index ConvertNextSlimeIndex(int lineIndex)
        {
            return NextSlimeIndexs[lineIndex];
        }

        /// <summary>
        /// 移動可能なスライムのインデックスの変換を行います。
        /// </summary>
        /// <param name="lineIndex">フィールドの行インデックス</param>
        /// <returns>移動可能なスライムのインデックス</returns>
        public static MovableSlime.UnitIndex ConvertMovableSlimeUnitIndex(int lineIndex)
        {
            return MovableSlimeIndexs[lineIndex];
        }

        /// <summary>
        /// NEXTスライムの情報が記載されている行かどうかを判定します。
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <returns>NEXTスライムの情報が存在する行かどうか</returns>
        public static bool ContainsNextSlimeInfo(int lineIndex)
        {
            return NextSlimeIndexs.ContainsKey(lineIndex);
        }

        /// <summary>
        /// 情報の行数を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns>情報の行数</returns>
        public static int GetInfoLength(string key)
        {
            return MultiLineLengths.ContainsKey(key) ? MultiLineLengths[key] : Length.Default;
        }

        /// <summary>
        /// フィールドイベントの変換辞書を作成します。
        /// </summary>
        /// <returns>フィールドイベントの変換辞書</returns>
        private static Dictionary<FieldEvent, string> BuildFieldEvents()
        {
            var fieldEvents = new Dictionary<FieldEvent, string>();
            fieldEvents.Add(FieldEvent.None, FieldEventSymbol.None);
            fieldEvents.Add(FieldEvent.DropObstructions, FieldEventSymbol.DropObstructions);
            fieldEvents.Add(FieldEvent.Erase, FieldEventSymbol.Erase);
            fieldEvents.Add(FieldEvent.MarkErasing, FieldEventSymbol.MarkErasing);
            fieldEvents.Add(FieldEvent.StartChain, FieldEventSymbol.StartChain);
            return fieldEvents;
        }

        /// <summary>
        /// 方向の逆引き変換辞書を作成します。
        /// </summary>
        /// <returns>方向の逆引き変換辞書</returns>
        private static Dictionary<string, FieldEvent> BuildReverseFieldEvents()
        {
            var fieldEvents = new Dictionary<string, FieldEvent>();
            fieldEvents.Add(FieldEventSymbol.None, FieldEvent.None);
            fieldEvents.Add(FieldEventSymbol.DropObstructions, FieldEvent.DropObstructions);
            fieldEvents.Add(FieldEventSymbol.Erase, FieldEvent.Erase);
            fieldEvents.Add(FieldEventSymbol.MarkErasing, FieldEvent.MarkErasing);
            fieldEvents.Add(FieldEventSymbol.StartChain, FieldEvent.StartChain);
            return fieldEvents;
        }

        /// <summary>
        /// 方向の変換辞書を作成します。
        /// </summary>
        /// <returns>方向の変換辞書</returns>
        private static Dictionary<Direction, string> BuildDirections()
        {
            var directions = new Dictionary<Direction, string>();
            directions.Add(Direction.None, DirectionSymbol.None);
            directions.Add(Direction.Down, DirectionSymbol.Down);
            directions.Add(Direction.Left, DirectionSymbol.Left);
            directions.Add(Direction.Right, DirectionSymbol.Right);
            directions.Add(Direction.Up, DirectionSymbol.Up);
            return directions;
        }

        /// <summary>
        /// 方向の逆引き変換辞書を作成します。
        /// </summary>
        /// <returns>方向の逆引き変換辞書</returns>
        private static Dictionary<string, Direction> BuildReverseDirections()
        {
            var directions = new Dictionary<string, Direction>();
            directions.Add(DirectionSymbol.None, Direction.None);
            directions.Add(DirectionSymbol.Down, Direction.Down);
            directions.Add(DirectionSymbol.Left, Direction.Left);
            directions.Add(DirectionSymbol.Right, Direction.Right);
            directions.Add(DirectionSymbol.Up, Direction.Up);
            return directions;
        }

        /// <summary>
        /// スライムの変換辞書を作成します。
        /// </summary>
        /// <returns>スライムの変換辞書</returns>
        private static Dictionary<Slime, char> BuildSlimes()
        {
            var slimes = new Dictionary<Slime, char>();
            slimes.Add(Slime.None, SlimeSymbol.None);
            slimes.Add(Slime.Blue, SlimeSymbol.Blue);
            slimes.Add(Slime.Green, SlimeSymbol.Green);
            slimes.Add(Slime.Purple, SlimeSymbol.Purple);
            slimes.Add(Slime.Red, SlimeSymbol.Red);
            slimes.Add(Slime.Yellow, SlimeSymbol.Yellow);
            slimes.Add(Slime.Obstruction, SlimeSymbol.Obstruction);
            slimes.Add(Slime.Erased, SlimeSymbol.Erased);
            return slimes;
        }

        /// <summary>
        /// スライムの変換辞書を作成します。
        /// </summary>
        /// <returns>スライムの変換辞書</returns>
        private static Dictionary<char, Slime> BuildReverseSlimes()
        {
            var slimes = new Dictionary<char, Slime>();
            slimes.Add(SlimeSymbol.None, Slime.None);
            slimes.Add(SlimeSymbol.Blue, Slime.Blue);
            slimes.Add(SlimeSymbol.Green, Slime.Green);
            slimes.Add(SlimeSymbol.Purple, Slime.Purple);
            slimes.Add(SlimeSymbol.Red, Slime.Red);
            slimes.Add(SlimeSymbol.Yellow, Slime.Yellow);
            slimes.Add(SlimeSymbol.Obstruction, Slime.Obstruction);
            slimes.Add(SlimeSymbol.Erased, Slime.Erased);
            return slimes;
        }

        /// <summary>
        /// 移動可能なスライムの変換辞書を作成します。
        /// </summary>
        /// <returns>移動可能なスライムの変換辞書</returns>
        private static Dictionary<Slime, char> BuildMovableSlimes()
        {
            var slimes = new Dictionary<Slime, char>();
            slimes.Add(Slime.Blue, MovableSlimeSymbol.Blue);
            slimes.Add(Slime.Green, MovableSlimeSymbol.Green);
            slimes.Add(Slime.Purple, MovableSlimeSymbol.Purple);
            slimes.Add(Slime.Red, MovableSlimeSymbol.Red);
            slimes.Add(Slime.Yellow, MovableSlimeSymbol.Yellow);
            slimes.Add(Slime.Obstruction, SlimeSymbol.Obstruction);
            slimes.Add(Slime.Erased, SlimeSymbol.Erased);
            return slimes;
        }

        /// <summary>
        /// 移動可能なスライムの逆引き変換辞書を作成します。
        /// </summary>
        /// <returns>移動可能なスライムの変換辞書</returns>
        private static Dictionary<char, Slime> BuildReverseMovableSlimes()
        {
            var slimes = new Dictionary<char, Slime>();
            slimes.Add(MovableSlimeSymbol.Blue, Slime.Blue);
            slimes.Add(MovableSlimeSymbol.Green, Slime.Green);
            slimes.Add(MovableSlimeSymbol.Purple, Slime.Purple);
            slimes.Add(MovableSlimeSymbol.Red, Slime.Red);
            slimes.Add(MovableSlimeSymbol.Yellow, Slime.Yellow);
            slimes.Add(SlimeSymbol.Obstruction, Slime.Obstruction);
            slimes.Add(SlimeSymbol.Erased, Slime.Erased);
            return slimes;
        }

        /// <summary>
        /// 複数行情報の行数辞書を作成します。
        /// </summary>
        /// <returns>複数行情報の行数辞書</returns>
        private static Dictionary<string, int> BuildMultiLineLengths()
        {
            var lengths = new Dictionary<string, int>();
            lengths.Add(Keys.ObstructionSlime, Length.Obstruction);
            lengths.Add(Keys.Field, Length.Field);
            return lengths;
        }

        /// <summary>
        /// おじゃまスライムの変換辞書を作成します。
        /// </summary>
        /// <returns>おじゃまスライムの変換辞書</returns>
        private static Dictionary<ObstructionSlime, char> BuildObstructionSlimes()
        {
            var dic = new Dictionary<ObstructionSlime, char>();
            dic.Add(ObstructionSlime.Small, ObstructionSlimeSymbol.Small);
            dic.Add(ObstructionSlime.Big, ObstructionSlimeSymbol.Big);
            dic.Add(ObstructionSlime.Rock, ObstructionSlimeSymbol.Rock);
            dic.Add(ObstructionSlime.Star, ObstructionSlimeSymbol.Star);
            dic.Add(ObstructionSlime.Moon, ObstructionSlimeSymbol.Moon);
            dic.Add(ObstructionSlime.Crown, ObstructionSlimeSymbol.Crown);
            dic.Add(ObstructionSlime.Comet, ObstructionSlimeSymbol.Comet);

            return dic;
        }

        /// <summary>
        /// おじゃまスライムの逆引き変換辞書を作成します。
        /// </summary>
        /// <returns>おじゃまスライムの逆引き変換辞書</returns>
        private static Dictionary<char, ObstructionSlime> BuildReverseObstructionSlimes()
        {
            var dic = new Dictionary<char, ObstructionSlime>();
            dic.Add(ObstructionSlimeSymbol.Small, ObstructionSlime.Small);
            dic.Add(ObstructionSlimeSymbol.Big, ObstructionSlime.Big);
            dic.Add(ObstructionSlimeSymbol.Rock, ObstructionSlime.Rock);
            dic.Add(ObstructionSlimeSymbol.Star, ObstructionSlime.Star);
            dic.Add(ObstructionSlimeSymbol.Moon, ObstructionSlime.Moon);
            dic.Add(ObstructionSlimeSymbol.Crown, ObstructionSlime.Crown);
            dic.Add(ObstructionSlimeSymbol.Comet, ObstructionSlime.Comet);

            return dic;
        }

        /// <summary>
        /// NEXTスライムのインデックスの変換辞書を作成します。
        /// </summary>
        /// <returns>NEXTスライムのインデックスの変換辞書</returns>
        private static Dictionary<int, NextSlime.Index> BuildNextSlimeIndexs()
        {
            var dic = new Dictionary<int, NextSlime.Index>();
            dic.Add(SimpleText.Index.NextSlimeFirstUnitFirstSlime, NextSlime.Index.First);
            dic.Add(SimpleText.Index.NextSlimeFirstUnitSecondSlime, NextSlime.Index.First);
            dic.Add(SimpleText.Index.NextSlimeSecondUnitFirstSlime, NextSlime.Index.Second);
            dic.Add(SimpleText.Index.NextSlimeSecondUnitSecondSlime, NextSlime.Index.Second);

            return dic;
        }

        /// <summary>
        /// 移動可能なスライムのインデックスの変換辞書を作成します。
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, MovableSlime.UnitIndex> BuildMovableSlimeIndexs()
        {
            var dic = new Dictionary<int, MovableSlime.UnitIndex>();
            dic.Add(SimpleText.Index.NextSlimeFirstUnitFirstSlime, MovableSlime.UnitIndex.First);
            dic.Add(SimpleText.Index.NextSlimeFirstUnitSecondSlime, MovableSlime.UnitIndex.Second);
            dic.Add(SimpleText.Index.NextSlimeSecondUnitFirstSlime, MovableSlime.UnitIndex.First);
            dic.Add(SimpleText.Index.NextSlimeSecondUnitSecondSlime, MovableSlime.UnitIndex.Second);

            return dic;
        }
    }
}
