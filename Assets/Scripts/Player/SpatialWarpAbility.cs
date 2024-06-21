using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialWarpAbility : MonoBehaviour
{
    private Player player;
    private Rigidbody rb;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    public void Teleport()
    {
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        player.onMovingSkill = true;
        rb.MovePosition(GameManager.Instance.GetMapGenerator().RandomPos(false));

        yield return new WaitForSeconds(0.1f);

        player.onMovingSkill = false;
    }
}
