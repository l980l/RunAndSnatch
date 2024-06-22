using System.Collections;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    [SerializeField] private GameObject stunAreaObj;
    private CircleCollider2D circleCollider2D;
    private StunArea stunArea;

    private void Awake()
    {
        circleCollider2D = stunAreaObj.GetComponent<CircleCollider2D>();
        stunArea = stunAreaObj.GetComponent<StunArea>();
    }

    public void SetStunAreaForDuration(float duration, float radius = -1)
    {
        StartCoroutine(StunAreaCoroutine(duration, radius));
    }

    private IEnumerator StunAreaCoroutine(float duration, float radius = -1)
    {
        stunArea.StunTime = duration;
        if (radius > 0)
            circleCollider2D.radius = radius;
        stunAreaObj.SetActive(true);

        yield return new WaitForFixedUpdate();

        stunAreaObj.SetActive(false);
    }
}
