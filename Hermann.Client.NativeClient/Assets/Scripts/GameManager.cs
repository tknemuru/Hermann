﻿using System.Collections;
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

/// <summary>
/// ゲーム管理機能を提供します。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// デバッグモードかどうか
    /// </summary>
    public static bool IsDebug = false;

    /// <summary>
    /// スライムオブジェクト
    /// </summary>
    public GameObject SlimeObject;

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
    private static CommandReceiver<NativeCommand, FieldContext> Receiver;

    /// <summary>
    /// UI飾り情報の受信機能
    /// </summary>
    private static UiDecorationContainerReceiver UiDecorationContainerReceiver;

    /// <summary>
    /// フィールドの送信機能
    /// </summary>
    private static FieldContextSender<string> Sender;

    /// <summary>
    /// 方向：無で更新するフレーム回数累計
    /// </summary>
    private static int[] NoneDirectionUpdateFrameElapsed { get; set; }

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

    // Use this for initialization
    void Start()
    {
        LastWinCount = new[] { 0, 0 };

        this.Slimes = new Dictionary<Player.Index, List<GameObject>>();
        Player.ForEach(player =>
        {
            this.Slimes.Add(player, new List<GameObject>());
        });
        this.FieldContextReflector = ScriptableObject.CreateInstance<FieldContextReflector>();
        this.FieldContextReflector.Initialize(this.SlimeObject);

        // 初期フィールド状態の取得
        NoneDirectionUpdateFrameElapsed = new[] { 0, 0 };
        Receiver = NativeClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
        Sender = NativeClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
        UiDecorationContainerReceiver = NativeClientDiProvider.GetContainer().GetInstance<UiDecorationContainerReceiver>();
        var command = NativeClientDiProvider.GetContainer().GetInstance<NativeCommand>();
        command.Command = Command.Start;
        Context = Receiver.Receive(command);

        KeyInfos = new List<KeyCode>();
        IsBlockedKeyInfo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnd(Context))
        {
            return;
        }

        // 入力受付
        var keyInfos = new KeyCode[Player.Length][];
        Player.ForEach(player =>
        {
            keyInfos[(int)player] = KeyMap.GetKeys().Select(k => k).
                Where(k => Input.GetKeyDown(k) && KeyMap.GetPlayer(k) == player).
                ToArray();
        });

        Player.ForEach(player =>
        {
            // フィールド状態の更新
            NoneDirectionUpdateFrameElapsed[(int)player]++;
            var requireNoneDirectionUpdate = (NoneDirectionUpdateFrameElapsed[(int)player] >= FieldContextConfig.NoneDirectionUpdateFrameCount || Context.Ground[(int)player]);
            if (requireNoneDirectionUpdate)
            {
                NoneDirectionUpdateFrameElapsed[(int)player] = 0;
            }

            if (Context.FieldEvent[(int)player] == FieldEvent.None)
            {
                UpdateDuringNoneEvent(player, keyInfos[(int)player], requireNoneDirectionUpdate);
            }
            else
            {
                UpdateDuringOccurrenceEvent(player);
            }
        });

        // 描画用情報の取得
        var container = UiDecorationContainerReceiver.Receive(Context);

        // 画面描画
        Player.ForEach(player =>
        {
            foreach (var slime in this.Slimes[player])
            {
                Destroy(slime);
            }
            this.Slimes[player] = this.FieldContextReflector.Update(player, container);
        });
    }

    /// <summary>
    /// イベントが発生していないときの更新処理を実行します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    private static void UpdateDuringOccurrenceEvent(Player.Index player)
    {
        // 移動方向無コマンドの実行
        Move(player, Direction.None, "----- 移動方向無コマンドの実行 -----");
        NoneDirectionUpdateFrameElapsed[(int)player] = FieldContextConfig.NoneDirectionUpdateFrameCount;
    }

    /// <summary>
    /// イベントが発生しているときの更新処理を実行します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    /// <param name="keys">入力キーリスト</param>
    /// <param name="requireNoneDirectionUpdate">移動方向無での更新が必要かどうか</param>
    private static void UpdateDuringNoneEvent(Player.Index player, KeyCode[] keys, bool requireNoneDirectionUpdate)
    {
        // 移動方向無コマンドの実行
        if (requireNoneDirectionUpdate)
        {
            Move(player, Direction.None, "----- 移動方向無コマンドの実行 -----");
        }

        // 入力を受け付けたコマンドの実行
        foreach (var key in keys)
        {
            Debug.Assert(KeyMap.GetPlayer(key) == player, "受け付けたキーとプレイヤの関係が不正です。");
            Move(player, KeyMap.GetDirection(key), "----- 入力を受け付けたコマンドの実行 -----");
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
        var c = NativeClientDiProvider.GetContainer().GetInstance<NativeCommand>();
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
        return (LastWinCount[(int)Player.Index.First] != context.WinCount[(int)Player.Index.First] ||
            LastWinCount[(int)Player.Index.Second] != context.WinCount[(int)Player.Index.Second]);
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
}