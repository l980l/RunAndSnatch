using System.Collections;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    [SerializeField] private GameObject stunAreaObj;
    private CircleCollider2D circleCollider2D;
    private StunArea stunArea;
    [SerializeField] private ParticleSystem stunPS;
    private WaitForSeconds waitForSeconds;

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(0.1f);
        circleCollider2D = stunAreaObj.GetComponent<CircleCollider2D>();
        stunArea = stunAreaObj.GetComponent<StunArea>();
    }

    public void SetStunAreaForDuration(float duration, float radius = -1)
    {
        stunPS.Play();  // 사운드는 Bell과 FerociousHowl 따로 재생.

        StartCoroutine(StunAreaCoroutine(duration, radius));
    }

    private IEnumerator StunAreaCoroutine(float duration, float radius = -1)
    {
        stunArea.StunTime = duration;
        if (radius > 0)
            circleCollider2D.radius = radius;
        stunAreaObj.SetActive(true);

        yield return waitForSeconds;

        stunAreaObj.SetActive(false);
    }
}
