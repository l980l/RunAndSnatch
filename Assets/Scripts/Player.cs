using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float SlideSpeed;
    [SerializeField] private int MaxHP;
    [SerializeField] private int HP;
    [SerializeField] private float SlidingTime;
    [SerializeField] private float SlidingCoolTime;

    private Vector2 inputVec;
    private bool bInvincible;    // 슬라이딩 할 때와 피격 시 잠시 동안.

    private bool bIsSliding; 
    private bool bEnableToSlide = true;
    private Vector2 SlideVec;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int GetMaxHP() { return MaxHP; }
    public int GetHP() { return HP; }

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
         
        // 슬라이딩 상태가 아니고, 슬라이딩 쿨타임이 찬 상태.
        if(!bIsSliding && bEnableToSlide)
        {
            if(Input.GetButtonDown("Jump"))
            {
                bIsSliding = true;
                bInvincible = true;
                bEnableToSlide = false;
                animator.SetBool("Slide", true);
                SlideVec = inputVec.normalized;
                Invoke("SlideEnd", SlidingTime);
            }
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude);

        // 슬라이딩 중이면 
        if (bIsSliding)
        {
            Vector2 nextVec = SlideVec.normalized * SlideSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + nextVec);
        }
        else
        {
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + nextVec);
        }
    }

    private void LateUpdate()
    {
        // 이동 방향에 맞게 좌우 반전.
        if (!bIsSliding)
        {
            if (inputVec.x != 0)
            {
                spriteRenderer.flipX = inputVec.x < 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.ItemType)
            {
                case (ItemType.BlueShell):
                    break;
                case (ItemType.DamageTest):
                    HP -= 10;
                    GameManager.Instance.GetHeatlhHUD().UpdateHP(); 
                    break;
            }
            Destroy(item.gameObject);
        }
    }

    private void Die()  // 임시로 짠 Die 함수.
    {
        animator.SetTrigger("Dead");
        rigidBody.simulated = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    private void SlideEnd()
    {
        // 무적 해제, 슬라이딩 상태 해제, 애니메이션 변경, 슬라이딩 쿨타임 돌리기
        bIsSliding = false;
        bInvincible = false;
        animator.SetBool("Slide", false);
        Invoke("CoolDownSliding", SlidingCoolTime);
    }

    private void CoolDownSliding()  // 슬라이딩 가능하게 하는 함수. Invoke로 호출하여 쿨타임 적용.
    {
        bEnableToSlide = true;
    }
}
