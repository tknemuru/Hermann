using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hermann.Models;
using Hermann.Contexts;

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
        Player.ForEach(player =>
        {
            for (var unit = 0; unit < FieldContextConfig.FieldUnitCount; unit++)
            {
                var slime = Slime.None;
                if (unit == 2)
                {
                    slime = Slime.Blue;
                }
                else if (unit == 3)
                {
                    slime = Slime.Green;
                }
                else if (unit == 4)
                {
                    slime = Slime.Yellow;
                }
                else
                {
                    slime = Slime.Red;
                }

                for (var index = 0; index < FieldContextConfig.FieldUnitBitCount; index++)
                {
                    var slimeObj = Instantiate(this.SlimeObject);
                    var uiSlime = slimeObj.GetComponent<UiSlime>();
                    uiSlime.Initialize(slimeObj, player, unit, index, slime);
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}