using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hermann.Models;
using Assets.Scripts.Helpers;

/// <summary>
/// スライム
/// </summary>
public class UiSlime : MonoBehaviour {
    /// <summary>
    /// スライム
    /// </summary>
    private Slime Slime { get; set; }

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update ()
    {
		
	}

    /// <summary>
    /// 初期化処理を行います。
    /// </summary>
    /// <param name="obj">スライムオブジェクト</param>
    /// <param name="player">プレイヤ</param>
    /// <param name="unit">フィールドユニット</param>
    /// <param name="index">ユニット内のインデックス</param>
    /// <param name="slime">スライム</param>
    public void Initialize(GameObject obj, Player.Index player, int unit, int index, Slime slime)
    {
        obj.transform.parent = GameObject.Find(GetFieldName(player)).transform;
        obj.transform.localPosition = new Vector3(0, 0, -1);

        var sprite = SpriteHelper.GetSprite(GetSlimeSpliteName(slime));
        obj.AddComponent<Image>().sprite = sprite;
        obj.GetComponent<Image>().SetNativeSize();
    }

    /// <summary>
    /// フィールド名を取得します。
    /// </summary>
    /// <param name="player">プレイヤ</param>
    /// <returns>フィールド名</returns>
    private static string GetFieldName(Player.Index player)
    {
        return player.GetName() + "Field";
    }

    /// <summary>
    /// スライムのスプライト名を取得します。
    /// </summary>
    /// <param name="slime">スライム</param>
    /// <returns>スライムのスプライト名</returns>
    private static string GetSlimeSpliteName(Slime slime)
    {
        return slime.GetName().ToLower() + "_default";
    }
}
