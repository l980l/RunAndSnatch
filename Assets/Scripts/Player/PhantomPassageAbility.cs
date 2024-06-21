using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PhantomPassageAbility : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    [SerializeField] private Light2D mistyLight;
    private MapGenerator mapGenerator;
    private Grid grid;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        mapGenerator= GameManager.Instance.GetMapGenerator();
        if(mapGenerator)
            grid = mapGenerator.GetGrid();
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

        // 탈출
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        // 위치 이동
        player.onMovingSkill = true;
        Vector3Int nowPos = grid.WorldToCell(transform.position);
        // 현재 위치가 벽인 경우. 가장 가까운 Road 찾고, Cell 중간 길이 더해주고.
        if (mapGenerator.isWallAtPos(nowPos.x, nowPos.y) == true)
        {
            rb.MovePosition(grid.CellToWorld((Vector3Int)mapGenerator.GetNearestRoad(nowPos.x, nowPos.y)) + grid.GetLayoutCellCenter());
        }

        yield return new WaitForFixedUpdate();

        player.onMovingSkill = false;
    }
}
