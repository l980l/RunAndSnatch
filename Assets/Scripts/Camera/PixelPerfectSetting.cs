using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelPerfectSetting : MonoBehaviour
{
    private PixelPerfectCamera pixelPerfectCamera;

    void Start()
    {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        if (pixelPerfectCamera == null)
            return;

        // 현재 화면 해상도의 1/6로 세팅
        int screenWidth = Screen.width / 6;
        int screenHeight = Screen.height / 6;

        pixelPerfectCamera.refResolutionX = screenWidth;
        pixelPerfectCamera.refResolutionY = screenHeight;

        // Pixel Perfect Camera 설정을 적용
        pixelPerfectCamera.enabled = false;
        pixelPerfectCamera.enabled = true;
    }
}