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
    private FieldContextReflector FieldContextReflector { get; set; }

    /// <summary>
    /// フィールドに追加したスライムのリスト
    /// </summary>
    private Dictionary<Player.Index, List<GameObject>> Slimes { get; set; }

    // Use this for initialization
    void Start()
    {
        this.Slimes = new Dictionary<Player.Index, List<GameObject>>();
        Player.ForEach(player =>
        {
            this.Slimes.Add(player, new List<GameObject>());
        });
        this.FieldContextReflector = ScriptableObject.CreateInstance<FieldContextReflector>();
        this.FieldContextReflector.Initialize(this.SlimeObject);
    }

    // Update is called once per frame
    void Update()
    {
        Player.ForEach(player =>
        {
            foreach (var slime in this.Slimes[player])
            {
                Destroy(slime);
            }
            this.Slimes[player] = this.FieldContextReflector.Update(player, new FieldContext());
        });
    }
}