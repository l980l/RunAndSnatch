using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected MonsterData monsterData;
    protected bool trace;
    protected bool stunned;


    protected GameObject player = GameManager.Instance.GetPlayer();
    private Transform transform;

    protected void Awake()
    {
        transform = GetComponent<Transform>();
    }
    protected float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }
}
