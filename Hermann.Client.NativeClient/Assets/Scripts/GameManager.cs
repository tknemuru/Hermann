using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hermann.Models;

/// <summary>
/// ゲーム管理機能を提供します。
/// </summary>
public class GameManager : MonoBehaviour
{

    /// <summary>
    /// スライムオブジェクト
    /// </summary>
    public GameObject SlimeObject;

    // Use this for initialization
    void Start()
    {
        var slimeObj = Instantiate(this.SlimeObject);
        var uiSlime = slimeObj.GetComponent<UiSlime>();
        uiSlime.Initialize(slimeObj, Player.Index.First, 0, 0, Slime.Red);
    }

    // Update is called once per frame
    void Update()
    {
    }
}