using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hermann.Models;
using Hermann.Contexts;
using Assets.Scripts.Updater;
using Hermann.Api.Receivers;
using Hermann.Api.Commands;
using Hermann.Api.Senders;
using System.Threading.Tasks;
using Assets.Scripts.Di;
using Hermann.Client.ConsoleClient;
using Hermann.Helpers;
using System;
using Assets.Scripts.Containers;
using Assets.Scripts.Analyzers;
using Assets.Scripts.Initializers;
using Assets.Scripts;
using Hermann.Ai;
using Hermann.Ai.Helpers;
using Hermann.Di;

/// <summary>
/// ゲーム管理機能を提供します。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// スライムオブジェクト
    /// </summary>
    public GameObject SlimeObject;

    /// <summary>
    /// デバッグモードかどうか
    /// </summary>
    private static bool IsDebug = false;

    /// <summary>
    /// テストモードかどうか
    /// </summary>
    private static bool IsTest = false;

    /// <summary>
    /// 学習中かどうか
    /// </summary>
    private static bool IsLearning = false;

    /// <summary>
    /// トレーニングモードかどうか
    /// </summary>
    public static bool IsTraining = false;

    /// <summary>
    /// AIプレイヤのバージョン
    /// </summary>
    public static AiPlayer.Version?[] AiVersions = new AiPlayer.Version?[] { AiPlayer.Version.V1_0, null };

    /// <summary>
    /// ゲームが終了したかどうか
    /// </summary>
    public static bool End = false;

    /// <summary>
    /// BGMファイル名
    /// </summary>
    private const string BgmFileName = "bgm1_low_vol";

    /// <summary>
    /// ボーナス加算割合
    /// </summary>
    private const double BonusRate = 0.001d;

    /// <summary>
    /// フィールド情報のUIフィールドへの反映機能
    /// </summary>
    private FieldContextReflector FieldContextReflector { get; set; }

    /// <summary>
    /// フィールドに追加したスライムのリスト
    /// </summary>
    private Dictionary<Player.Index, List<GameObject>> Slimes { get; set; }

    /// <summary>
    /// フィールド状態
    /// </summary>
    private static FieldContext Context;

    /// <summary>
    /// コマンドの受信機能
    /// </summary>
    private static NativeCommandReceiver Receiver;

    /// <summary>
    /// UI飾り情報の受信機能
    /// </summary>
    private static UiDecorationContainerReceiver UiDecorationContainerReceiver;

    /// <summary>
    /// 音の管理機能
    /// </summary>
    private static AudioManager AudioManager;

    /// <summary>
    /// 入力の管理機能
    /// </summary>
    private static InputManager InputManager;

    /// <summary>
    /// 効果音に関する分析機能
    /// </summary>
    private static SoundEffectAnalyzer SoundEffectAnalyzer;

    /// <summary>
    /// 音の演出に関する情報格納庫
    /// </summary>
    private static SoundEffectDecorationContainer[] SoundEffectDecorationContainer;

    /// <summary>
    /// 効果音出力機能
    /// </summary>
    private static SoundEffectOutputter SoundEffectOutputter;

    /// <summary>
    /// フィールドの送信機能
    /// </summary>
    private static SimpleTextSender Sender;

    /// <summary>
    /// 方向：無で更新するフレーム回数累計
    /// </summary>
    private static int[] NoneDirectionUpdateFrameElapsed { get; set; }

    /// <summary>
    /// AIが動かすフレーム回数累計
    /// </summary>
    private static int[] AiMoveFrameCountElapsed { get; set; }

    /// <summary>
    /// 入力されたコンソールキー情報
    /// </summary>
    private static List<KeyCode> KeyInfos { get; set; }

    /// <summary>
    /// コンソールキー情報がブロック中かどうか
    /// </summary>
    private static bool IsBlockedKeyInfo { get; set; }

    /// <summary>
    /// 前回の勝ち数
    /// </summary>
    private static int[] LastWinCount { get; set; }

    /// <summary>
    /// 削除時のアニメーション累積フレーム数
    /// </summary>
    private static int[] EraseAnimationFrameElapsedCount { get; set; }

    /// <summary>
    /// AIプレイヤ
    /// </summary>
    private static AiPlayer[] AiPlayers { get; set; }

    // Use this for initialization
    void Start()
    {
        try
        {
            NativeClientDiRegister.Register();
            LastWinCount = new[] { 0, 0 };
            EraseAnimationFrameElapsedCount = new[] { 0, 0 };
            AudioManager = gameObject.AddComponent<AudioManager>();
            AudioManager.PlayBGM(BgmFileName);
            SoundEffectOutputter = DiProvider.GetContainer().GetInstance<SoundEffectOutputter>();
            SoundEffectOutputter.Initialize(AudioManager);
            SoundEffectAnalyzer = DiProvider.GetContainer().GetInstance<SoundEffectAnalyzer>();
            SoundEffectDecorationContainer = new SoundEffectDecorationContainer[Player.Length];
            InputManager = DiProvider.GetContainer().GetInstance<InputManager>();
            AiPlayers = new AiPlayer[Player.Length];

            this.Slimes = new Dictionary<Player.Index, List<GameObject>>();
            Player.ForEach(player =>
            {
                SoundEffectDecorationContainer[(int)player] = DiProvider.GetContainer().GetInstance<SoundEffectDecorationContainer>();
                this.Slimes.Add(player, new List<GameObject>());
            });
            this.FieldContextReflector = ScriptableObject.CreateInstance<FieldContextReflector>();
            this.FieldContextReflector.Initialize(this.SlimeObject);

            // 初期フィールド状態の取得
            NoneDirectionUpdateFrameElapsed = new[] { FieldContextConfig.NoneDirectionUpdateFrameCount, FieldContextConfig.NoneDirectionUpdateFrameCount };
            AiMoveFrameCountElapsed = new[] { FieldContextConfig.AiMoveFrameCount, FieldContextConfig.AiMoveFrameCount };
            Debug.Log("gamemanager 1");
            Receiver = DiProvider.GetContainer().GetInstance<NativeCommandReceiver>();
            Debug.Log("gamemanager 2");
            Sender = DiProvider.GetContainer().GetInstance<SimpleTextSender>();
            Debug.Log("gamemanager 3");
            UiDecorationContainerReceiver = DiProvider.GetContainer().GetInstance<UiDecorationContainerReceiver>();
            var command = DiProvider.GetContainer().GetInstance<NativeCommand>();
            command.Command = Command.Start;
            Context = Receiver.Receive(command);
            if (IsTest)
            {
                Context = DiProvider.GetContainer().GetInstance<FieldContextSimpleTextInitializer>().Initialize(Context);
            }
            Player.ForEach(player =>
            {
                SoundEffectDecorationContainer[(int)player].LastFieldContext = Context;

                if (AiVersions[player.ToInt()] != null)
                {
                    AiPlayers[player.ToInt()] = DiProvider.GetContainer().GetInstance<AiPlayer>();
                    AiPlayers[player.ToInt()].Inject(new AiPlayer.Config()
                    {
                        Version = AiVersions[player.ToInt()].Value,
                        UsingSlime = Context.UsingSlimes,
                    });
                }
            });

            KeyInfos = new List<KeyCode>();
            IsBlockedKeyInfo = false;
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (IsEnd(Context))
            {
                return;
            }

            // TODO:後でちゃんと修正。トレーニング用
            if (IsTraining)
            {
                ChangeForTraining(Context);
            }

            // 入力受付
            var keyInfos = new KeyCode[Player.Length][];
            Player.ForEach(player =>
            {
                keyInfos[(int)player] = InputManager.PullKeyCodes(player);
            });

            Player.ForEach(player =>
            {
                // 前回のフィールド状態を更新しておく
                SoundEffectDecorationContainer[(int)player].LastFieldContext = Context.DeepCopy();

                // フィールド状態の更新
                NoneDirectionUpdateFrameElapsed[(int)player]++;
                var requireNoneDirectionUpdate = (NoneDirectionUpdateFrameElapsed[(int)player] >= FieldContextConfig.NoneDirectionUpdateFrameCount || Context.Ground[(int)player]);
                if (requireNoneDirectionUpdate)
                {
                    NoneDirectionUpdateFrameElapsed[(int)player] = 0;
                }
                AiMoveFrameCountElapsed[(int)player]++;
                //var requireAiMove = (AiMoveFrameCountElapsed[(int)player] >= FieldContextConfig.NoneDirectionUpdateFrameCount);
                var requireAiMove = true;
                if (requireAiMove)
                {
                    AiMoveFrameCountElapsed[(int)player] = 0;
                }

                if (Context.FieldEvent[(int)player] == FieldEvent.None)
                {
                    UpdateDuringNoneEvent(player, keyInfos[(int)player], requireNoneDirectionUpdate, requireAiMove);
                }
                else
                {
                    if (Context.FieldEvent[(int)player] == FieldEvent.Erase)
                    {
                        EraseAnimationFrameElapsedCount[(int)player]++;
                        if (EraseAnimationFrameElapsedCount[(int)player] > FieldContextConfig.EraseAnimationFrameCount)
                        {
                            UpdateDuringOccurrenceEvent(player);
                            EraseAnimationFrameElapsedCount[(int)player] = 0;
                        }
                    }
                    else
                    {
                        UpdateDuringOccurrenceEvent(player);
                    }
                }
            });

            // 描画用情報の取得
            var container = UiDecorationContainerReceiver.Receive(Context);
            container.EraseAnimationFrameElapsedCount = EraseAnimationFrameElapsedCount;

            Player.ForEach(player =>
            {
                // 効果音
                SoundEffectDecorationContainer[(int)player] = SoundEffectAnalyzer.Analyze(Context, player, SoundEffectDecorationContainer[(int)player]);
                SoundEffectOutputter.Output(SoundEffectDecorationContainer[(int)player], player);
                DebugLog(string.Format("----- 効果音要求({0}) -----", player.GetName()));
                DebugLog(SoundEffectDecorationContainer[(int)player].ToString());

                // 画面描画
                foreach (var slime in this.Slimes[player])
                {
                    Destroy(slime);
                }
                this.Slimes[player] = this.FieldContextReflector.Update(player, container);
            });
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// イベントが発生しているときの更新処理を実行します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    private static void UpdateDuringOccurrenceEvent(Player.Index player)
    {
        // 移動方向無コマンドの実行
        if (AiVersions[player.ToInt()] != null)
        {
            // AIの操作
            AiMove(player, "----- AIの移動 -----");
        }
        else
        {
            Move(player, Direction.None, "----- 移動方向無コマンドの実行 -----");
        }
        NoneDirectionUpdateFrameElapsed[(int)player] = FieldContextConfig.NoneDirectionUpdateFrameCount;
    }

    /// <summary>
    /// イベントが発生していないときの更新処理を実行します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    /// <param name="keys">入力キーリスト</param>
    /// <param name="requireNoneDirectionUpdate">移動方向無での更新が必要かどうか</param>
    /// <param name="requireAiMove">AIを動かす必要があるかどうか</param>
    private static void UpdateDuringNoneEvent(Player.Index player, KeyCode[] keys, bool requireNoneDirectionUpdate, bool requireAiMove)
    {
        if (AiVersions[player.ToInt()] != null)
        {
            // AIの操作
            AiMove(player, "----- AIの移動 -----");
        } else if (requireNoneDirectionUpdate)
        {
            // 移動方向無コマンドの実行
            Move(player, Direction.None, "----- 移動方向無コマンドの実行 -----");
        }

        // 入力を受け付けたコマンドの実行
        foreach (var key in keys)
        {
            Debug.Assert(KeyMap.GetPlayer(key) == player, "受け付けたキーとプレイヤの関係が不正です。");
            Move(player, KeyMap.GetDirection(key), "----- 入力を受け付けたコマンドの実行 -----");
            //if (IsLearning)
            //{
            //    LogWriter.WriteState(Context);
            //}
        }
    }

    /// <summary>
    /// 移動を実行します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    /// <param name="direction">移動方向</param>
    /// <param name="debugLogTitle">デバッグ出力用タイトル</param>
    private static void Move(Player.Index player, Direction direction, string debugLogTitle)
    {
        Context.OperationPlayer = player;
        Context.OperationDirection = direction;
        var c = DiProvider.GetContainer().GetInstance<NativeCommand>();
        c.Command = Command.Move;
        c.Context = Context;
        DebugLog(debugLogTitle);
        DebugLog(Sender.Send(Context));
        Context = Receiver.Receive(c);
    }

    /// <summary>
    /// AIの移動を実行します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    /// <param name="debugLogTitle">デバッグ出力用タイトル</param>
    private static void AiMove(Player.Index player, string debugLogTitle)
    {
        Context.OperationPlayer = player;
        Context.OperationDirection = AiPlayers[player.ToInt()].Think(Context);
        var c = DiProvider.GetContainer().GetInstance<NativeCommand>();
        c.Command = Command.Move;
        c.Context = Context;
        DebugLog(debugLogTitle);
        DebugLog(Sender.Send(Context));
        Context = Receiver.Receive(c);
    }

    /// <summary>
    /// 勝負の結果が出たかどうかを判定します。
    /// </summary>
    /// <param name="context">フィールド状態</param>
    /// <returns>勝負の結果が出たかどうか</returns>
    private static bool IsEnd(FieldContext context)
    {
        if(End)
        {
            return End;
        }

        var firstWin = LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First];
        var secondWin = LastWinCount[(int)Player.Index.Second] != context.WinCount[(int)Player.Index.Second];
        End = firstWin || secondWin;
        if(End && IsLearning)
        {
            WriteResult(context);
        }
        return End;
    }

    /// <summary>
    /// デバッグログを出力します。
    /// </summary>
    /// <param name="log">ログ文字列</param>
    private static void DebugLog(string log)
    {
        if (IsDebug)
        {
            FileHelper.WriteLine(log);
        }
    }

    /// <summary>
    /// トレーニング用に移動可能スライムを改変します。
    /// </summary>
    /// <param name="context"></param>
    private static void ChangeForTraining(FieldContext context)
    {
        NextSlime.ForEach(next =>
        {
            MovableSlime.ForEach(movable =>
            {
                context.NextSlimes[(int)Player.Index.First][(int)next][(int)movable] = Slime.Blue;
            });
        });
    }

    private static void WriteResult(FieldContext context)
    {
        var win = GetWinPlayer(context);
        var score = (win == Player.Index.First) ? 1.0d : -1.0d;
        var scoreDiff = context.Score[(int)Player.Index.First] - context.Score[(int)Player.Index.Second];
        if (win == Player.Index.First && scoreDiff > 0)
        {
            score += scoreDiff * BonusRate;
        }
        else if (win == Player.Index.Second && scoreDiff < 0)
        {
            score += scoreDiff * BonusRate;
        }
        LogWriter.WriteWinResult(score);
    }

    private static Player.Index GetWinPlayer(FieldContext context)
    {
        var firstWin = LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First];
        return firstWin ? Player.Index.First : Player.Index.Second;
    }
}