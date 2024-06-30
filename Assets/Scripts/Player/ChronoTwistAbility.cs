using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoTwistAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private ParticleSystem chronoTwistPS;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void UseChronoTwist(float duration)
    {
        SoundManager.Instance.PlaySFX(SFX.ChronoTwistSFX, Camera.main.transform.position);
        chronoTwistPS.Play();

        StartCoroutine(ChronoTwistCoroutine(duration));
    }

    private IEnumerator ChronoTwistCoroutine(float duration)
    {
        Time.timeScale = 0.5f; // 게임의 흐름을 절반으로 느리게 설정
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // FixedUpdate의 호출 간격을 조정

        float originSpeed = playerMovement.speed;
        playerMovement.MoveSpeedUp(originSpeed); // 플레이어의 이동 속도를 2배로 설정

        yield return new WaitForSecondsRealtime(duration);

        // 원래 상태로 복구합니다.
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        playerMovement.MoveSpeedUp(-originSpeed); // 플레이어의 이동 속도를 원래대로 복구
        chronoTwistPS.Stop();
    }
}
