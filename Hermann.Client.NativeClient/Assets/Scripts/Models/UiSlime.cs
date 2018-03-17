using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hermann.Models;
using Assets.Scripts.Helpers;
using Hermann.Contexts;

/// <summary>
/// UIスライム
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
    /// <param name="color">スライム</param>
    /// <param name="joinState">結合状態</param>
    public void Initialize(GameObject slime, Player.Index player, Slime color, SlimeJoinState joinState)
    {
        // スライム画像の読み込み
        slime.AddComponent<Image>().sprite = SpriteHelper.GetSprite(GetSlimeSpliteName(color, joinState));

        // スライム画像のサイズを調整
        AdjustImageSize(UiFieldHelper.GetPlayerField(player), slime);
    }

    /// <summary>
    /// スライムの画像サイズを調整します。
    /// </summary>
    /// <param name="field">フィールド</param>
    /// <param name="slime">スライム</param>
    private static void AdjustImageSize(GameObject field, GameObject slime)
    {
        var imageScale = TransformHelper.GetScale(slime.GetComponent<Image>());
        var imageSize = TransformHelper.GetSize(slime.GetComponent<Image>());

        // 一度画像のサイズをリセットする
        imageSize = new Vector2(imageSize.x * imageScale.x, imageSize.y * imageScale.y);
        TransformHelper.SetSize(slime.GetComponent<Image>(), imageSize);

        // フィールドの横幅から比率を算出して更新
        var scale = TransformHelper.GetSize(field.GetComponent<RectTransform>()).x / (imageSize.x * FieldContextConfig.VerticalLineLength);
        TransformHelper.SetScale(slime.GetComponent<Image>(), new Vector3(scale, scale, 0f));
    }

    /// <summary>
    /// スライムのスプライト名を取得します。
    /// </summary>
    /// <param name="slime">スライム</param>
    /// <param name="joinState">結合状態</param>
    /// <returns>スライムのスプライト名</returns>
    private static string GetSlimeSpliteName(Slime slime, SlimeJoinState joinState)
    {
        return slime.GetName().ToLower() + "_" + joinState.GetName().ToLower();
    }
}
