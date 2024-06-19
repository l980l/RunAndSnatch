using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUI : MonoBehaviour
{
    public void SetPortrait(Sprite _sprite)
    {
        GetComponent<Image>().sprite = _sprite;
    }
}
