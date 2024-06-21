using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PhantomPassageAbility : MonoBehaviour
{
    private Player player;
    [SerializeField] private Light2D mistyLight;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void UsePhantomPassage(float duration)
    {
        StartCoroutine(PhantomPassageCoroutine(duration));
    }

    private IEnumerator PhantomPassageCoroutine(float duration)
    {
        // Phantom 레이어로
        gameObject.layer = 14;
        // mistyLight 밝히기
        mistyLight.enabled = true;

        yield return new WaitForSecondsRealtime(duration);

        // 끝나면 Player 레이어로
        gameObject.layer = 6;
        // mistyLight 끄기
        mistyLight.enabled = false;
    }
}
