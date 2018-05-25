using UnityEngine;

[DisallowMultipleComponent]
public sealed class UIStretchCustom : MonoBehaviour
{
    //==============================================================================
    // 定数
    //==============================================================================
    private static readonly float DEFAULT_WIDTH = 667; // ゲーム画面の横幅
    private static readonly float DEFAULT_HEIGHT = 357;  // ゲーム画面の縦幅

    private static readonly float DEFAULT_ASPECT = DEFAULT_WIDTH / DEFAULT_HEIGHT;
    private static readonly float WIDTH = Screen.width;
    private static readonly float HEIGHT = Screen.height;
    private static readonly float ASPECT = WIDTH / HEIGHT;
    private static readonly float ASPECT_OFFSET = Mathf.Min(1, ASPECT / DEFAULT_ASPECT);
    private static readonly float INVERSE_ASPECT_OFFSET = 1 / ASPECT_OFFSET;

    //==============================================================================
    // 変数(SerializeField)
    //==============================================================================
    [HideInInspector] [SerializeField] private bool m_isInit = false;

    //==============================================================================
    // 関数
    //==============================================================================
    /// <summary>
    /// 初期化される時に呼び出されます
    /// </summary>
    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null) return;

        if (m_isInit) return;
        m_isInit = true;

        var sizeDelta = rectTransform.sizeDelta;
        var oldWidth = sizeDelta.x;
        var oldHeight = sizeDelta.y;
        var newWidth = Mathf.RoundToInt(oldWidth * INVERSE_ASPECT_OFFSET);
        var newHeight = Mathf.RoundToInt(oldHeight * INVERSE_ASPECT_OFFSET);

        sizeDelta.y = newHeight;
        sizeDelta.x = newWidth;

        rectTransform.sizeDelta = sizeDelta;
    }
}