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
    /// スライム
    /// </summary>
    public GameObject SlimeObject;

    // Use this for initialization
    void Start()
    {
        var slimeObj = Instantiate(this.SlimeObject);
        var uiSlime = slimeObj.GetComponent<UiSlime>();
        uiSlime.Initialize(slimeObj, new Vector3(0, 0, -1), Slime.Red);
    }

    // Update is called once per frame
    void Update()
    {
    }
}