using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int OperationPlayer { get; set; }

        /// <summary>
        /// 操作方向
        /// </summary>
        public Direction OperationDirection { get; set; }

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
        public int[] BuiltRemainingTime { get; set; }

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
            this.Ground = new bool[Player.Length];
            this.BuiltRemainingTime = new int[Player.Length];
            this.Score = new long[Player.Length];
            this.Chain = new int[Player.Length];
            this.Offset = new bool[Player.Length];
            this.AllErase = new bool[Player.Length];
            this.WinCount = new int[Player.Length];

            this.UsingSlimes = new Slime[FieldContextConfig.UsingSlimeCount];

            this.ObstructionSlimes = new Dictionary<ObstructionSlime, int>[Player.Length];
            this.ObstructionSlimes[Player.First] = new Dictionary<ObstructionSlime, int>();
            this.ObstructionSlimes[Player.Second] = new Dictionary<ObstructionSlime, int>();

            this.MovableSlimes = new MovableSlime[Player.Length][];
            this.MovableSlimes[Player.First] = new MovableSlime[MovableSlime.Length];
            this.MovableSlimes[Player.Second] = new MovableSlime[MovableSlime.Length];

            this.SlimeFields = new Dictionary<Slime, uint[]>[Player.Length];
            this.SlimeFields[Player.First] = new Dictionary<Slime, uint[]>();
            this.SlimeFields[Player.Second] = new Dictionary<Slime, uint[]>();

            this.NextSlimes = new Slime[Player.Length][][];
            this.NextSlimes[Player.First] = new Slime[NextSlime.Count][];
            this.NextSlimes[Player.First][(int)NextSlime.Index.First] = new Slime[MovableSlime.Length];
            this.NextSlimes[Player.First][(int)NextSlime.Index.Second] = new Slime[MovableSlime.Length];
            this.NextSlimes[Player.Second] = new Slime[NextSlime.Count][];
            this.NextSlimes[Player.Second][(int)NextSlime.Index.First] = new Slime[MovableSlime.Length];
            this.NextSlimes[Player.Second][(int)NextSlime.Index.Second] = new Slime[MovableSlime.Length];
        }

        /// <summary>
        /// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns></returns>
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

            for (var player = Player.First; player < Player.Length; player++)
            {
                // 接地
                equals.Add(context.Ground[player] == this.Ground[player]);

                // 設置残タイム
                equals.Add(context.BuiltRemainingTime[player] == this.BuiltRemainingTime[player]);

                // 得点
                equals.Add(context.Score[player] == this.Score[player]);

                // 連鎖
                equals.Add(context.Chain[player] == this.Chain[player]);

                // 相殺
                equals.Add(context.Offset[player] == this.Offset[player]);

                // 全消
                equals.Add(context.AllErase[player] == this.AllErase[player]);

                // 勝数
                equals.Add(context.WinCount[player] == this.WinCount[player]);

                // 使用スライム
                for(var i = 0; i < FieldContextConfig.UsingSlimeCount; i++)
                {
                    equals.Add(context.UsingSlimes[i] == this.UsingSlimes[i]);
                }

                // おじゃまスライム
                var obstructionSlimes = context.ObstructionSlimes[player];
                var myObstructionSlimes = this.ObstructionSlimes[player];
                equals.Add(obstructionSlimes.Count == myObstructionSlimes.Count);
                foreach (var obstruction in obstructionSlimes.Keys)
                {
                    equals.Add(obstructionSlimes[obstruction] == myObstructionSlimes[obstruction]);
                }

                // スライムごとの配置状態
                var slimeFields = context.SlimeFields[player];
                var mySlimeFields = this.SlimeFields[player];
                equals.Add(slimeFields.Count == mySlimeFields.Count);
                foreach (var slime in slimeFields.Keys)
                {
                    for (var i = 0; i < slimeFields[slime].Length; i++)
                    {
                        equals.Add(slimeFields[slime][i] == mySlimeFields[slime][i]);
                    }
                }

                // 移動可能なスライムの配置状態
                var movable = context.MovableSlimes[player];
                var myMovable = this.MovableSlimes[player];
                equals.Add(movable.Length == myMovable.Length);
                equals.Add(movable[(int)MovableSlime.UnitIndex.First].Equals(myMovable[(int)MovableSlime.UnitIndex.First]));
                equals.Add(movable[(int)MovableSlime.UnitIndex.Second].Equals(myMovable[(int)MovableSlime.UnitIndex.Second]));

                // NEXTスライム
                var nextSlimes = context.NextSlimes[player];
                var myNextSlimes = this.NextSlimes[player];
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
            return this.OperationPlayer ^ (int)this.OperationDirection ^ this.Time.GetHashCode() ^ this.Ground.GetHashCode() ^
                this.BuiltRemainingTime.GetHashCode() ^ this.Score.GetHashCode() ^ this.Chain.GetHashCode() ^ this.Offset.GetHashCode() ^
                this.AllErase.GetHashCode() ^ this.WinCount.GetHashCode() ^ this.ObstructionSlimes.GetHashCode() ^
                this.MovableSlimes.GetHashCode() ^ this.SlimeFields.GetHashCode() ^ this.NextSlimes.GetHashCode();
        }
    }
}
