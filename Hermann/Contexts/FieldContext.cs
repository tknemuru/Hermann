using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermann.Contexts
{
    /// <summary>
    /// フィールドの状態
    /// </summary>
    public class FieldContext
    {
        /// <summary>
        /// プレイヤ
        /// </summary>
        public Player.Index OperationPlayer { get; set; }

        /// <summary>
        /// イベント
        /// </summary>
        public FieldEvent[] FieldEvent { get; set; }

        /// <summary>
        /// 操作方向
        /// </summary>
        public Direction OperationDirection { get; set; }

        /// <summary>
        /// 回転方向
        /// </summary>
        public Direction[] RotationDirection { get; set; }

        /// <summary>
        /// 経過時間
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// 接地
        /// </summary>
        public bool[] Ground { get; set; }

        /// <summary>
        /// 設置残タイム
        /// </summary>
        public long[] BuiltRemainingTime { get; set; }

        /// <summary>
        /// 使用済得点
        /// </summary>
        public long[] UsedScore { get; set; }

        /// <summary>
        /// 得点
        /// </summary>
        public long[] Score { get; set; }

        /// <summary>
        /// 連鎖
        /// </summary>
        public int[] Chain { get; set; }

        /// <summary>
        /// 相殺
        /// </summary>
        public bool[] Offset { get; set; }

        /// <summary>
        /// 全消
        /// </summary>
        public bool[] AllErase { get; set; }

        /// <summary>
        /// 勝数
        /// </summary>
        public int[] WinCount { get; set; }

        /// <summary>
        /// 使用スライム
        /// </summary>
        public Slime[] UsingSlimes { get; set; }

        /// <summary>
        /// おじゃまスライム
        /// </summary>
        public Dictionary<ObstructionSlime, int>[] ObstructionSlimes { get; set; }

        /// <summary>
        /// 移動可能なスライム
        /// </summary>
        public MovableSlime[][] MovableSlimes { get; set; }

        /// <summary>
        /// スライムごとの配置状態
        /// </summary>
        public Dictionary<Slime, uint[]>[] SlimeFields { get; set; }

        /// <summary>
        /// NEXTスライム
        /// </summary>
        public Slime[][][] NextSlimes { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FieldContext()
        {
            this.FieldEvent = new FieldEvent[Player.Length];
            this.RotationDirection = new Direction[Player.Length];
            this.Ground = new[] { false, false };
            this.BuiltRemainingTime = new long[Player.Length];
            this.UsedScore = new long[Player.Length];
            this.Score = new long[Player.Length];
            this.Chain = new int[Player.Length];
            this.Offset = new bool[Player.Length];
            this.AllErase = new bool[Player.Length];
            this.WinCount = new int[Player.Length];

            this.UsingSlimes = new Slime[FieldContextConfig.UsingSlimeCount];

            this.ObstructionSlimes = new Dictionary<ObstructionSlime, int>[Player.Length];
            this.ObstructionSlimes[(int)Player.Index.First] = new Dictionary<ObstructionSlime, int>();
            this.ObstructionSlimes[(int)Player.Index.Second] = new Dictionary<ObstructionSlime, int>();

            this.MovableSlimes = new MovableSlime[Player.Length][];
            this.MovableSlimes[(int)Player.Index.First] = new MovableSlime[MovableSlime.Length];
            this.MovableSlimes[(int)Player.Index.Second] = new MovableSlime[MovableSlime.Length];

            this.SlimeFields = new Dictionary<Slime, uint[]>[Player.Length];
            this.SlimeFields[(int)Player.Index.First] = new Dictionary<Slime, uint[]>();
            this.SlimeFields[(int)Player.Index.Second] = new Dictionary<Slime, uint[]>();

            this.NextSlimes = new Slime[Player.Length][][];
            this.NextSlimes[(int)Player.Index.First] = new Slime[NextSlime.Length][];
            this.NextSlimes[(int)Player.Index.First][(int)NextSlime.Index.First] = new Slime[MovableSlime.Length];
            this.NextSlimes[(int)Player.Index.First][(int)NextSlime.Index.Second] = new Slime[MovableSlime.Length];
            this.NextSlimes[(int)Player.Index.Second] = new Slime[NextSlime.Length][];
            this.NextSlimes[(int)Player.Index.Second][(int)NextSlime.Index.First] = new Slime[MovableSlime.Length];
            this.NextSlimes[(int)Player.Index.Second][(int)NextSlime.Index.Second] = new Slime[MovableSlime.Length];
        }

        /// <summary>
        /// ディープコピーを行います。
        /// </summary>
        /// <returns>ディープコピーされたフィールドの状態</returns>
        public FieldContext DeepCopy()
        {
            var context = new FieldContext();

            // プレイヤ
            context.OperationPlayer = this.OperationPlayer;

            // 操作方向
            context.OperationDirection = this.OperationDirection;

            // 経過時間
            context.Time = this.Time;

            for (var player = Player.Index.First; (int)player < Player.Length; player++)
            {
                // イベント
                context.FieldEvent[(int)player] = this.FieldEvent[(int)player];

                // 回転方向
                context.RotationDirection[(int)player] = this.RotationDirection[(int)player];

                // 接地
                context.Ground[(int)player] = this.Ground[(int)player];

                // 設置残タイム
                context.BuiltRemainingTime[(int)player] = this.BuiltRemainingTime[(int)player];

                // 使用済得点
                context.UsedScore[(int)player] = this.UsedScore[(int)player];

                // 得点
                context.Score[(int)player] = this.Score[(int)player];

                // 連鎖
                context.Chain[(int)player] = this.Chain[(int)player];

                // 相殺
                context.Offset[(int)player] = this.Offset[(int)player];

                // 全消
                context.AllErase[(int)player] = this.AllErase[(int)player];

                // 勝数
                context.WinCount[(int)player] = this.WinCount[(int)player];

                // 使用スライム
                for (var i = 0; i < FieldContextConfig.UsingSlimeCount; i++)
                {
                    context.UsingSlimes[i] = this.UsingSlimes[i];
                }

                // おじゃまスライム
                var obstructionSlimes = context.ObstructionSlimes[(int)player];
                var myObstructionSlimes = this.ObstructionSlimes[(int)player];
                foreach (var obstruction in myObstructionSlimes.Keys)
                {
                    obstructionSlimes[obstruction] = myObstructionSlimes[obstruction];
                }

                // スライムごとの配置状態
                var slimeFields = context.SlimeFields[(int)player];
                var mySlimeFields = this.SlimeFields[(int)player];
                foreach (var slime in mySlimeFields.Keys)
                {
                    slimeFields.Add(slime, new uint[FieldContextConfig.FieldUnitCount]);
                    for (var i = 0; i < mySlimeFields[slime].Length; i++)
                    {
                        slimeFields[slime][i] = mySlimeFields[slime][i];
                    }
                }

                // 移動可能なスライムの配置状態
                var movable = context.MovableSlimes[(int)player];
                var myMovable = this.MovableSlimes[(int)player];
                movable[(int)MovableSlime.UnitIndex.First] = new MovableSlime
                {
                    Slime = myMovable[(int)MovableSlime.UnitIndex.First].Slime,
                    Index = myMovable[(int)MovableSlime.UnitIndex.First].Index,
                    Position = myMovable[(int)MovableSlime.UnitIndex.First].Position
                };
                movable[(int)MovableSlime.UnitIndex.Second] = new MovableSlime
                {
                    Slime = myMovable[(int)MovableSlime.UnitIndex.Second].Slime,
                    Index = myMovable[(int)MovableSlime.UnitIndex.Second].Index,
                    Position = myMovable[(int)MovableSlime.UnitIndex.Second].Position
                };

                // NEXTスライム
                var nextSlimes = context.NextSlimes[(int)player];
                var myNextSlimes = this.NextSlimes[(int)player];
                for (var unitIndex = 0; unitIndex < myNextSlimes.Count(); unitIndex++)
                {
                    for (var movableIndex = 0; movableIndex < myNextSlimes[unitIndex].Count(); movableIndex++)
                    {
                        nextSlimes[unitIndex][movableIndex] = myNextSlimes[unitIndex][movableIndex];
                    }
                }
            }

            return context;
        }

        /// <summary>
        /// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>指定のオブジェクトが現在のオブジェクトと等しいかどうか</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var context = (FieldContext)obj;
            var equals = new List<bool>();

            // プレイヤ
            equals.Add(context.OperationPlayer == this.OperationPlayer);

            // 操作方向
            equals.Add(context.OperationDirection == this.OperationDirection);

            // 経過時間
            equals.Add(context.Time == this.Time);

            for (var player = Player.Index.First; (int)player < Player.Length; player++)
            {
                // イベント
                equals.Add(context.FieldEvent[(int)player] == this.FieldEvent[(int)player]);

                // 回転方向
                equals.Add(context.RotationDirection[(int)player] == this.RotationDirection[(int)player]);

                // 接地
                equals.Add(context.Ground[(int)player] == this.Ground[(int)player]);

                // 設置残タイム
                equals.Add(context.BuiltRemainingTime[(int)player] == this.BuiltRemainingTime[(int)player]);

                // 使用済得点
                equals.Add(context.UsedScore[(int)player] == this.UsedScore[(int)player]);

                // 得点
                equals.Add(context.Score[(int)player] == this.Score[(int)player]);

                // 連鎖
                equals.Add(context.Chain[(int)player] == this.Chain[(int)player]);

                // 相殺
                equals.Add(context.Offset[(int)player] == this.Offset[(int)player]);

                // 全消
                equals.Add(context.AllErase[(int)player] == this.AllErase[(int)player]);

                // 勝数
                equals.Add(context.WinCount[(int)player] == this.WinCount[(int)player]);

                // 使用スライム
                for(var i = 0; i < FieldContextConfig.UsingSlimeCount; i++)
                {
                    equals.Add(context.UsingSlimes[i] == this.UsingSlimes[i]);
                }

                // おじゃまスライム
                var obstructionSlimes = context.ObstructionSlimes[(int)player];
                var myObstructionSlimes = this.ObstructionSlimes[(int)player];
                equals.Add(obstructionSlimes.Count == myObstructionSlimes.Count);
                foreach (var obstruction in obstructionSlimes.Keys)
                {
                    if (myObstructionSlimes.ContainsKey(obstruction))
                    {
                        equals.Add(obstructionSlimes[obstruction] == myObstructionSlimes[obstruction]);
                    }
                    else
                    {
                        equals.Add(obstructionSlimes[obstruction] == 0);
                    }
                }

                // スライムごとの配置状態
                var slimeFields = context.SlimeFields[(int)player];
                var mySlimeFields = this.SlimeFields[(int)player];
                equals.Add(slimeFields.Count == mySlimeFields.Count);
                foreach (var slime in slimeFields.Keys)
                {
                    for (var i = 0; i < slimeFields[slime].Length; i++)
                    {
                        equals.Add(slimeFields[slime][i] == mySlimeFields[slime][i]);
                    }
                }

                // 移動可能なスライムの配置状態
                var movable = context.MovableSlimes[(int)player];
                var myMovable = this.MovableSlimes[(int)player];
                equals.Add(movable.Length == myMovable.Length);
                equals.Add(movable[(int)MovableSlime.UnitIndex.First].Equals(myMovable[(int)MovableSlime.UnitIndex.First]));
                equals.Add(movable[(int)MovableSlime.UnitIndex.Second].Equals(myMovable[(int)MovableSlime.UnitIndex.Second]));

                // NEXTスライム
                var nextSlimes = context.NextSlimes[(int)player];
                var myNextSlimes = this.NextSlimes[(int)player];
                equals.Add(slimeFields.Count() == mySlimeFields.Count());
                for (var unitIndex = 0; unitIndex < nextSlimes.Count(); unitIndex++)
                {
                    for (var movableIndex = 0; movableIndex < nextSlimes[unitIndex].Count(); movableIndex++)
                    {
                        equals.Add(nextSlimes[unitIndex][movableIndex] == myNextSlimes[unitIndex][movableIndex]);
                    }
                }
            }

            return equals.All(e => e);
        }

        /// <summary>
        /// 既定のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在のオブジェクトのハッシュ コード。</returns>
        public override int GetHashCode()
        {
            return this.OperationPlayer.GetHashCode() ^ this.FieldEvent.GetHashCode() ^ this.OperationDirection.GetHashCode() ^ this.RotationDirection.GetHashCode() ^ this.Time.GetHashCode() ^ this.Ground.GetHashCode() ^
                this.BuiltRemainingTime.GetHashCode() ^ this.Score.GetHashCode() ^ this.Chain.GetHashCode() ^ this.Offset.GetHashCode() ^
                this.AllErase.GetHashCode() ^ this.WinCount.GetHashCode() ^ this.ObstructionSlimes.GetHashCode() ^
                this.MovableSlimes.GetHashCode() ^ this.SlimeFields.GetHashCode() ^ this.NextSlimes.GetHashCode();
        }
    }
}
