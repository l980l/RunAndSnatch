using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    // ΩÃ±€≈Ê¿∏∑Œ ¡ˆ¡§.
    #region Singleton
    public static ToolTip Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public ItemType ItemType;
    public Text ItemName;
    public Text ItemTip;
    public Image ItemImage;
    public int ClickedSlotIndex;

#if UNITY_STANDALONE_WIN
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.SetActive(false);
        }
    }
#endif

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
 