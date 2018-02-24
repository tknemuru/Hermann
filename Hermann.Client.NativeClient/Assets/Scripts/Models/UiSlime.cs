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
        obj.transform.parent = GameObject.Find("1PField").transform;
        obj.transform.localPosition = new Vector3(0, 0, -1);

        var sprite = SpriteHelper.GetSprite("red_default");
        obj.AddComponent<Image>().sprite = sprite;
        obj.GetComponent<Image>().SetNativeSize();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
