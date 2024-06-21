using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomPassageAbility : MonoBehaviour
{
    private Player player;

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

        yield return new WaitForSecondsRealtime(duration);

        // 끝나면 Player 레이어로
        gameObject.layer = 6;
    }
}
