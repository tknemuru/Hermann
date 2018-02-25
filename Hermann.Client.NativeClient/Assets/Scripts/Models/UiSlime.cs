using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hermann.Models;
using Assets.Scripts.Helpers;
using Hermann.Contexts;

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
    /// <param name="slime">スライムオブジェクト</param>
    /// <param name="player">プレイヤ</param>
    /// <param name="unit">フィールドユニット</param>
    /// <param name="index">ユニット内のインデックス</param>
    /// <param name="color">スライム</param>
    public void Initialize(GameObject slime, Player.Index player, int unit, int index, Slime color)
    {
        // スライム画像の読み込み
        var sprite = SpriteHelper.GetSprite(GetSlimeSpliteName(color));
        var field = GameObject.Find(GetFieldName(player));
        slime.AddComponent<Image>().sprite = sprite;
        slime.transform.SetParent(GameObject.Find(GetFieldName(player)).transform);

        // スライム画像のサイズを調整
        AdjustImageSize(field, slime);

        // サイズ・位置から座標を取得し、セット
        var position = FieldPositionConverter.GetSlimeFieldPosition(TransformHelper.GetScaledSize(field.GetComponent<RectTransform>()).x,
            TransformHelper.GetScaledSize(slime.GetComponent<Image>()).x,
            unit,
            index);
        TransformHelper.SetPosition(slime.GetComponent<RectTransform>(), position);
    }

    private static void AdjustImageSize(GameObject field, GameObject slime)
    {
        var imageScale = TransformHelper.GetScale(slime.GetComponent<Image>());
        var imageSize = TransformHelper.GetSize(slime.GetComponent<Image>());

        // 一度画像のサイズをリセットする
        imageSize = new Vector2(imageSize.x * imageScale.x, imageSize.y * imageScale.y);
        TransformHelper.SetSize(slime.GetComponent<Image>(), imageSize);

        // フィールドの横幅から比率を算出して更新
        var scale = TransformHelper.GetSize(field.GetComponent<RectTransform>()).x / (imageSize.x * FieldContextConfig.VerticalLineLength);
        TransformHelper.SetScale(slime.GetComponent<Image>(), new Vector3(scale, scale, -1f));
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
