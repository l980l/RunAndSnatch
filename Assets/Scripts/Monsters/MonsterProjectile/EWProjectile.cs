using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWProjectile : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector2 dir;
    private float speed;
    private bool isMove;
    public Vector2 Dir { set { dir = value; } }
    public bool IsMove { set { isMove = value; } }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = GetComponentInParent<Monster>().monsterData.projectileSpeed;
    }

    private void FixedUpdate()
    {
        if (rigidBody != null && isMove)
        {
            Vector2 nextVec = dir * speed * Time.deltaTime;
            rigidBody.MovePosition(rigidBody.position + nextVec);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽이나 플레이어
        if(collision.gameObject.layer == 6 || collision.gameObject.layer == 12)
        {
            isMove = false;
            animator.SetTrigger("Explode");
        }
    }

    public void StareAtDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rigidBody.MoveRotation(angle);
    }
}
