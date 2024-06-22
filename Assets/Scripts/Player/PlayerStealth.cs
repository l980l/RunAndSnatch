using System.Collections;
using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    [SerializeField] private GameObject stealthArea;
    private CircleCollider2D circleCollider2D;

    public bool Stealth { get; private set; }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = stealthArea.GetComponent<CircleCollider2D>();
    }

    // 일정 시간 동안 stealth 변수를 true로 설정하는 함수
    public void SetStealthForDuration(float duration, float radius = -1)
    {
        StartCoroutine(StealthCoroutine(duration, radius));
    }

    private IEnumerator StealthCoroutine(float duration, float radius = -1)
    {
        // stealth 활성화
        Stealth = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        if (radius > 0)
            circleCollider2D.radius = radius;
        stealthArea.SetActive(true);

        // FixedUpdate 주기 동안 대기
        yield return new WaitForFixedUpdate();

        // stealthArea 비활성화
        stealthArea.SetActive(false);

        // duration 동안 대기
        yield return new WaitForSeconds(duration);

        // stealth 비활성화
        Stealth = false;
        spriteRenderer.color = Color.white;
    }
}
       