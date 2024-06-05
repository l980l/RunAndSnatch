using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Vector2 inputVec;
    [SerializeField] private float speed;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude);

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + nextVec);
    }

    private void LateUpdate()
    {
        if(inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }
}
