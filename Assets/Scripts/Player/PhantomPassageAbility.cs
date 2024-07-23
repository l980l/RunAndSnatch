using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PhantomPassageAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    [SerializeField] private Light2D mistyLight;
    private MapGenerator mapGenerator;
    private Grid grid;
    [SerializeField] private ParticleSystem bgPS;
    [SerializeField] private ParticleSystem catPS;
    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(0.1f);
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        mapGenerator= GameManager.Instance.GetMapGenerator();
        if(mapGenerator)
            grid = mapGenerator.GetGrid();
    }

    public void UsePhantomPassage(float duration)
    {
        SoundManager.Instance.PlaySFX(SFX.PhantomPassageSkillSFX, Camera.main.transform.position);
        bgPS.Play();
        catPS.Play();
        StartCoroutine(PhantomPassageCoroutine(duration));
    }

    private IEnumerator PhantomPassageCoroutine(float duration)
    {
        // Phantom 레이어로
        gameObject.layer = 14;
        // mistyLight 밝히기
        mistyLight.enabled = true;

        yield return new WaitForSeconds(duration);

        // 끝나면 Player 레이어로
        gameObject.layer = 6;
        // mistyLight 끄기
        mistyLight.enabled = false;

        // 탈출
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        // 위치 이동
        playerMovement.onMovingSkill = true;
        Vector3Int nowPos = grid.WorldToCell(transform.position);
        // 현재 위치가 벽인 경우. 가장 가까운 Road 찾고, Cell 중간 길이 더해주고.
        transform.position = grid.CellToWorld((Vector3Int)mapGenerator.GetNearestRoad(nowPos.x, nowPos.y)) + grid.GetLayoutCellCenter();

        yield return waitForSeconds;

        playerMovement.onMovingSkill = false;
        bgPS.Stop();
        catPS.Stop();
    }
}
