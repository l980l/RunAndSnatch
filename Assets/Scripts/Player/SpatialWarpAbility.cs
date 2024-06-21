using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialWarpAbility : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Teleport()
    {
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        player.onMovingSkill = true;
        rb.MovePosition(GameManager.Instance.GetMapGenerator().RandomPos(false));

        yield return new WaitForFixedUpdate();

        player.onMovingSkill = false;
    }
}
