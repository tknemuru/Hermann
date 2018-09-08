using System.Collections;
using System.Collections.Generic;
using Hermann.Ai;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ModeSelector : MonoBehaviour, IPointerDownHandler {
    
    /// <summary>
    /// ひとりで遊ぶかどうか
    /// </summary>/
    public bool IsSinglePaly; 

	// Use this for initialization
	void Start () {
		
	}
	
    // Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Ons the click.
    /// </summary>
    public void OnClick()
    {
        this.SelectMode();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.SelectMode();
    }

    /// <summary>
    /// マウス押下時のコールバック関数
    /// </summary>
    void OnMouseDown()
    {
        this.SelectMode();
    }

    /// <summary>
    /// モードを選択します。
    /// </summary>
    private void SelectMode() {
        Debug.Log("Button click!");
        if (this.IsSinglePaly)
        {
            GameManager.AiVersions = new AiPlayer.Version?[] { null, AiPlayer.Version.V1_0 };
        }
        else
        {
            GameManager.AiVersions = new AiPlayer.Version?[] { null, null };
        }

        SceneManager.LoadScene("Main");
    }
}
