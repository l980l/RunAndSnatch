using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialWarpAbility : MonoBehaviour
{
    public void Teleport()
    {
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        GetComponent<Player>().onMovingSkill = true;
        GetComponent<Rigidbody2D>().MovePosition(GameManager.Instance.GetMapGenerator().RandomPos(false));

        yield return new WaitForSeconds(0.1f);

        GetComponent<Player>().onMovingSkill = false;
    }
}
