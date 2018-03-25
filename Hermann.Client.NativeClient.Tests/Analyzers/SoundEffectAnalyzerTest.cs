using System;
using Assets.Scripts.Analyzers;
using Assets.Scripts.Containers;
using Assets.Scripts.Models;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Initializers;
using Hermann.Models;
using Hermann.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Client.NativeClient.Tests.Analyzers
{
    /// <summary>
    /// SoundEffectAnalyzerのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class SoundEffectAnalyzerTest
    {
        /// <summary>
        /// フィールド状態のファイルパス
        /// </summary>
        private const string FieldResourcePath = "../../resources/analyzers/soundeffectanalyzer/test-field-in.txt";

        /// <summary>
        /// フィールド状態
        /// </summary>
        private FieldContext Context { get; set; }

        /// <summary>
        /// 音の演出に関する情報を格納庫
        /// </summary>
        private SoundEffectDecorationContainer Container { get; set; }

        /// <summary>
        /// 分析機能
        /// </summary>
        private SoundEffectAnalyzer Analyzer { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SoundEffectAnalyzerTest()
        {
            this.Analyzer = new SoundEffectAnalyzer();
            this.Initialize();
        }

        /// <summary>
        /// 001:攻撃の効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 攻撃の効果音が適切なタイミングで要求される()
        {            
            var player = Player.Index.First;
            var pIndex = (int)Player.Index.First;

            // 001:前回)連鎖0 今回)連鎖0 → 無
            this.Initialize();
            this.Container.LastFieldContext.Chain[pIndex] = 0;
            this.Context.Chain[pIndex] = 0;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack1]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack2]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack3]);

            // 002:前回)連鎖1 今回)連鎖0 → 無
            this.Initialize();
            this.Container.LastFieldContext.Chain[pIndex] = 1;
            this.Context.Chain[pIndex] = 0;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack1]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack2]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack3]);

            // 003:前回)連鎖1 今回)連鎖1 → 無
            this.Initialize();
            this.Container.LastFieldContext.Chain[pIndex] = 1;
            this.Context.Chain[pIndex] = 1;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack1]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack2]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack3]);

            // 004:前回)連鎖0 今回)連鎖1 → 有(レベル1)
            this.Initialize();
            this.Container.LastFieldContext.Chain[pIndex] = 0;
            this.Context.Chain[pIndex] = 1;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Attack1]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack2]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack3]);

            // 005:前回)連鎖2 今回)連鎖3 → 有(レベル2)
            this.Initialize();
            this.Container.LastFieldContext.Chain[pIndex] = 2;
            this.Context.Chain[pIndex] = 3;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack1]);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Attack2]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack3]);

            // 006:前回)連鎖4 今回)連鎖5 → 有(レベル3)
            this.Initialize();
            this.Container.LastFieldContext.Chain[pIndex] = 4;
            this.Context.Chain[pIndex] = 5;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack1]);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Attack2]);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Attack3]);
        }

        /// <summary>
        /// 002:削除の効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 削除の効果音が適切なタイミングで要求される()
        {
            var player = Player.Index.Second;
            var pIndex = (int)Player.Index.Second;

            // 001:前回)削除 今回)スライム落下 → 有
            this.Initialize();
            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.Erase;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Erase]);

            // 002:前回)削除 今回)削除 → 無
            this.Initialize();
            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.Erase;
            this.Context.FieldEvent[pIndex] = FieldEvent.Erase;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Erase]);

            // 003:前回)スライム落下 今回)スライム落下 → 無
            this.Initialize();
            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Erase]);
        }

        /// <summary>
        /// 003:接地の効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 接地の効果音が適切なタイミングで要求される()
        {
            var player = Player.Index.First;
            var pIndex = (int)Player.Index.First;

            // 001:前回)接地false 今回)接地true → 有
            this.Initialize();
            this.Container.LastFieldContext.Ground[pIndex] = false;
            this.Context.Ground[pIndex] = true;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Ground]);

            // 002:前回)接地true 今回)接地true → 無
            this.Initialize();
            this.Container.LastFieldContext.Ground[pIndex] = true;
            this.Context.Ground[pIndex] = true;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Ground]);

            // 003:前回)接地false 今回)接地false → 無
            this.Initialize();
            this.Container.LastFieldContext.Ground[pIndex] = false;
            this.Context.Ground[pIndex] = false;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Ground]);
        }

        /// <summary>
        /// 004:移動の効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 移動の効果音が適切なタイミングで要求される()
        {
            var player = Player.Index.Second;
            var pIndex = (int)Player.Index.Second;

            // 001:今回)移動方向:上、かつ、移動可能スライムが前回から移動している → 有
            this.Initialize();
            this.Context.OperationPlayer = player;
            this.Context.OperationDirection = Direction.Up;

            this.Container.LastFieldContext.MovableSlimes[pIndex][(int)MovableSlime.UnitIndex.First].Index = 12;
            this.Context.MovableSlimes[pIndex][(int)MovableSlime.UnitIndex.First].Index = 20;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Move]);

            // 002:今回)移動方向:上、かつ、移動可能スライムが前回から移動していない → 無
            this.Initialize();
            this.Context.OperationPlayer = player;
            this.Context.OperationDirection = Direction.Up;

            this.Container.LastFieldContext.MovableSlimes[pIndex][(int)MovableSlime.UnitIndex.First].Index = 12;
            this.Context.MovableSlimes[pIndex][(int)MovableSlime.UnitIndex.First].Index = 12;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Move]);

            // 003:今回)移動方向:上以外、かつ、移動可能スライムが前回から移動している → 無
            this.Initialize();
            this.Context.OperationPlayer = player;
            this.Context.OperationDirection = Direction.Right;

            this.Container.LastFieldContext.MovableSlimes[pIndex][(int)MovableSlime.UnitIndex.First].Index = 12;
            this.Context.MovableSlimes[pIndex][(int)MovableSlime.UnitIndex.First].Index = 20;
            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Move]);
        }

        /// <summary>
        /// 005:複数おじゃまスライムの効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 複数おじゃまスライムの効果音が適切なタイミングで要求される()
        {
            var player = Player.Index.First;
            var pIndex = (int)Player.Index.First;

            // 001:前回)おじゃまスライム落下 今回)準備 かつ おじゃまスライム数：18以上 → 有
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropObstructions;
            this.Context.FieldEvent[pIndex] = FieldEvent.NextPreparation;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Obstructions]);

            // 002:前回)おじゃまスライム落下 今回)準備 かつ おじゃまスライム数：18未満 → 無
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(600);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropObstructions;
            this.Context.FieldEvent[pIndex] = FieldEvent.NextPreparation;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Obstructions]);

            // 003:前回)おじゃまスライム落下 今回)おじゃまスライム落下 かつ おじゃまスライム数：18以上 → 無
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropObstructions;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropObstructions;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Obstructions]);

            // 004:前回)準備 今回)準備 かつ おじゃまスライム数：18以上 → 無
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.NextPreparation;
            this.Context.FieldEvent[pIndex] = FieldEvent.NextPreparation;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Obstructions]);
        }

        /// <summary>
        /// 006:相殺の効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 相殺の効果音が適切なタイミングで要求される()
        {
            var player = Player.Index.First;
            var pIndex = (int)Player.Index.First;

            // 001:前回)削除 今回)スライム落下 かつ 前回)自分のおじゃまスライム数 > 今回)自分のおじゃまスライム数 → 有
            this.Initialize();
            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.Erase;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Container.LastFieldContext.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(600);

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.Offset]);

            // 002:前回)削除 今回)スライム落下 かつ 前回)自分のおじゃまスライム数 = 今回)自分のおじゃまスライム数 → 無
            this.Initialize();
            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.Erase;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Container.LastFieldContext.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Offset]);

            // 003:前回)スライム落下 今回)スライム落下かつ 前回)自分のおじゃまスライム数 > 今回)自分のおじゃまスライム数 → 無
            this.Initialize();
            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropSlimes;
            this.Container.LastFieldContext.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(600);

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.Offset]);
        }

        /// <summary>
        /// 007:単発おじゃまスライムの効果音が適切なタイミングで要求される
        /// </summary>
        [TestMethod]
        public void 単発おじゃまスライムの効果音が適切なタイミングで要求される()
        {
            var player = Player.Index.First;
            var pIndex = (int)Player.Index.First;

            // 001:前回)おじゃまスライム落下 今回)準備 かつ おじゃまスライム数：18以上 → 無
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(3000);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropObstructions;
            this.Context.FieldEvent[pIndex] = FieldEvent.NextPreparation;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.SingleObstruction]);

            // 002:前回)おじゃまスライム落下 今回)準備 かつ おじゃまスライム数：18未満 → 有
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(600);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropObstructions;
            this.Context.FieldEvent[pIndex] = FieldEvent.NextPreparation;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(true, this.Container.Required[SoundEffect.SingleObstruction]);

            // 003:前回)おじゃまスライム落下 今回)おじゃまスライム落下 かつ おじゃまスライム数：18未満 → 無
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(600);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.DropObstructions;
            this.Context.FieldEvent[pIndex] = FieldEvent.DropObstructions;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.SingleObstruction]);

            // 004:前回)準備 今回)準備 かつ おじゃまスライム数：18未満 → 無
            this.Initialize();
            this.Context.FieldEvent[pIndex] = FieldEvent.MarkErasing;
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(600);
            this.Analyzer.Analyze(this.Context, player, this.Container);
            this.Context.ObstructionSlimes[pIndex] = ObstructionSlimeHelper.ScoreToObstructions(0);

            this.Container.LastFieldContext.FieldEvent[pIndex] = FieldEvent.NextPreparation;
            this.Context.FieldEvent[pIndex] = FieldEvent.NextPreparation;

            this.Analyzer.Analyze(this.Context, player, this.Container);
            Assert.AreEqual(false, this.Container.Required[SoundEffect.SingleObstruction]);
        }

        /// <summary>
        /// 初期化処理を行います。
        /// </summary>
        private void Initialize()
        {
            this.Container = new SoundEffectDecorationContainer();
            this.Container.LastFieldContext = TestHelper.Receiver.Receive(FieldResourcePath);
            this.Context = TestHelper.Receiver.Receive(FieldResourcePath);
        }
    }
}
