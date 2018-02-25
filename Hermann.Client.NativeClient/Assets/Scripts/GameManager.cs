using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hermann.Models;
using Hermann.Contexts;
using Assets.Scripts.Updater;

/// <summary>
/// ゲーム管理機能を提供します。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// スライムオブジェクト
    /// </summary>
    public GameObject SlimeObject;

    /// <summary>
    /// フィールド情報のUIフィールドへの反映機能
    /// </summary>
    public FieldContextReflector FieldContextReflector { get; set; }

    // Use this for initialization
    void Start()
    {
        this.FieldContextReflector = ScriptableObject.CreateInstance<FieldContextReflector>();
        this.FieldContextReflector.Initialize(this.SlimeObject);

        Player.ForEach(player =>
        {
            this.FieldContextReflector.Update(player, new FieldContext());
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}