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

/// <summary>
/// ゲーム管理機能を提供します。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// デバッグモードかどうか
    /// </summary>
    public static bool Debug = false;

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
    /// 方向：無で更新するフレーム回数
    /// </summary>
    private static int NoneDirectionUpdateFrameCount { get; set; }

    /// <summary>
    /// 入力されたコンソールキー情報
    /// </summary>
    private static List<KeyCode> KeyInfos { get; set; }

    /// <summary>
    /// コンソールキー情報がブロック中かどうか
    /// </summary>
    private static bool IsBlockedKeyInfo { get; set; }

    /// <summary>
    /// 無移動のプレイヤ
    /// </summary>
    private static Player.Index NoneMovePlayer { get; set; }

    /// <summary>
    /// 前回の勝ち数
    /// </summary>
    private static int[] LastWinCount { get; set; }

    // Use this for initialization
    void Start()
    {
        NoneMovePlayer = Player.Index.First;
        LastWinCount = new[] { 0, 0 };

        this.Slimes = new Dictionary<Player.Index, List<GameObject>>();
        Player.ForEach(player =>
        {
            this.Slimes.Add(player, new List<GameObject>());
        });
        this.FieldContextReflector = ScriptableObject.CreateInstance<FieldContextReflector>();
        this.FieldContextReflector.Initialize(this.SlimeObject);

        // 初期フィールド状態の取得
        NoneDirectionUpdateFrameCount = 0;
        Receiver = NativeClientDiProvider.GetContainer().GetInstance<CommandReceiver<NativeCommand, FieldContext>>();
        Sender = NativeClientDiProvider.GetContainer().GetInstance<FieldContextSender<string>>();
        UiDecorationContainerReceiver = NativeClientDiProvider.GetContainer().GetInstance<UiDecorationContainerReceiver>();
        var command = NativeClientDiProvider.GetContainer().GetInstance<NativeCommand>();
        command.Command = Command.Start;
        Context = Receiver.Receive(command);

        // キー変更イベントの購読
        KeyInfos = new List<KeyCode>();
        IsBlockedKeyInfo = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 入力受付
        KeyInfos = KeyInfos.Concat(KeyMap.GetKeys().Select(k => k).Where(k => Input.GetKeyDown(k))).ToList();

        // 移動方向無コマンドの実行
        Context.OperationDirection = Direction.None;
        var c = NativeClientDiProvider.GetContainer().GetInstance<NativeCommand>();
        c.Command = Command.Move;
        c.Context = Context.DeepCopy();
        c.Context.OperationPlayer = NoneMovePlayer;
        NoneMovePlayer = Player.GetOppositeIndex(NoneMovePlayer);
        DebugLog("----- 移動方向無コマンドの実行 -----");
        DebugLog(Sender.Send(Context));
        Context = Receiver.Receive(c);

        // 入力を受け付けたコマンドの実行
        IsBlockedKeyInfo = true;
        var keys = KeyInfos.Select(k => k).ToList();
        KeyInfos.Clear();
        IsBlockedKeyInfo = false;

        foreach (var key in keys)
        {
            if (!KeyMap.ContainsKey(key))
            {
                continue;
            }

            Context.OperationPlayer = KeyMap.GetPlayer(key);
            Context.OperationDirection = KeyMap.GetDirection(key);
            c = NativeClientDiProvider.GetContainer().GetInstance<NativeCommand>();
            c.Command = Command.Move;
            c.Context = Context.DeepCopy();
            DebugLog("----- 入力を受け付けたコマンドの実行 -----");
            DebugLog(Sender.Send(Context));
            Context = Receiver.Receive(c);
        }

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
    /// デバッグログを出力します。
    /// </summary>
    /// <param name="log">ログ文字列</param>
    private static void DebugLog(string log)
    {
        if (Debug)
        {
            FileHelper.WriteLine(log);
        }
    }
}