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
    /// <param name="slime"></param>
    public void Initialize(GameObject obj, Vector3 vector, Slime slime)
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
