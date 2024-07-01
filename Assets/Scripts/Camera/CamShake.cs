using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{
    public static CamShake Instance;
    private Coroutine prevShake;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    public void ShakeCamera(float intensity, float time)
    {
        if (prevShake != null)
            StopCoroutine(prevShake);
        prevShake = StartCoroutine(ShakeCamCoroutine(intensity, time));
    }

    private IEnumerator ShakeCamCoroutine(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        yield return new WaitForSecondsRealtime(time);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }
}
