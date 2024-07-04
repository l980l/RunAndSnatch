using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileKeyManager : MonoBehaviour
{
    public static MobileKeyManager instance;
    private void Awake()
    {
        // 윈도우인 경우 모바일 키 비활성화
#if UNITY_STANDALONE_WIN
        Destroy(gameObject);
        return;
#endif
#if UNITY_ANDROID
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
#endif
    }

    public bool RunButtonDown;
    [SerializeField] private FloatingJoystick joystick;
    public FloatingJoystick Joystick { get { return joystick; } }

    public void OnPointerDown()
    {
        RunButtonDown = true;
    }

    public void OnPointerUp()
    {
        RunButtonDown = false;
    }
}
