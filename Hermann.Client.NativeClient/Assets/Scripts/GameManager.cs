using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム管理機能を提供します。
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// スライム
    /// </summary>
    public GameObject Slime;

	// Use this for initialization
	void Start () {
        Instantiate(this.Slime, new Vector3(0, 0, -1), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
