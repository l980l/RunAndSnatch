using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    // 싱글톤으로 지정.
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

    public Text ItemName;
    public Text ItemTip;
    public Image ItemImage;
    public int ClickedSlotIndex;

    private void Update()
    {
        // esc를 누르거나 마우스 좌클릭을 하거나.
        if (Input.GetButtonDown("Cancel") || Input.GetMouseButtonUp(0))
        {
            gameObject.SetActive(false);
        }
    }
}
 