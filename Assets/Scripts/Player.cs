using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float DodgeSpeed;
    [SerializeField] private int MaxHP;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaRegenSpeed;
    private int HP;
    private float Stamina;
    private Vector2 inputVec;
    private bool bIsDodging;
    private int AcquiredItemValue;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MotionTrail motionTrail;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        motionTrail= GetComponent<MotionTrail>();
        HP = MaxHP;
        Stamina = MaxStamina;
    }

    void Update()
    {
        if(HP > 0)
            GetInput();
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude);
        
        if(HP > 0)
        {
            // 회피 중이면 
            if (bIsDodging)
            {
                // 이동
                Vector2 nextVec = inputVec.normalized * DodgeSpeed * Time.fixedDeltaTime;
                rigidBody.MovePosition(rigidBody.position + nextVec);

                // 스테미나 소모
                if (Stamina < 0)
                    Stamina = 0;
                else
                    Stamina -= Time.fixedDeltaTime;
            }
            else
            {
                Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
                rigidBody.MovePosition(rigidBody.position + nextVec);

                // 스테미나 회복
                if (Stamina < MaxStamina)
                    Stamina += Time.fixedDeltaTime * StaminaRegenSpeed;
                else
                    Stamina = MaxStamina;

            }
            // StaminaHUD 세팅
            float Amount = (float)Stamina / (float)MaxStamina;
            GameManager.Instance.GetStaminaHUD().UpdateStamina(Amount);
        }
    }

    private void LateUpdate()
    {
        // 이동 방향에 맞게 좌우 반전.
        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
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
                    AcquiredItemValue += item.GetItemValue();
                    GameManager.Instance.SetItemValue(AcquiredItemValue);
                    break;
                case (ItemType.DamageTest):
                    Vector2 AttackPos = new Vector2(collision.transform.position.x, collision.transform.position.y);
                    OnDamage(50, AttackPos);
                    break;
            }
            Destroy(item.gameObject);
        }
    }

    private void GetInput()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        // 회피 상태가 아니고, 회피가 가능한 상태.
        if (!bIsDodging && Stamina > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                DodgeStart();
            }
        }

        if(bIsDodging)
        {
            if (Input.GetButtonUp("Jump") || Stamina <= 0)
            {
                DodgeEnd();
            }
        }
    }

    public void SetInvincible(bool bInvincible)
    {
        if (bInvincible)
            gameObject.layer = 11;
        else
            gameObject.layer = 6;
    }
    private void OnDamage(int Damage, Vector2 AttackPos)
    {
        HP -= Damage;
        // HealthHUD 세팅
        float amount = (float)HP / (float)MaxHP;
        GameManager.Instance.GetHeatlhHUD().UpdateHP(amount);

        if (HP <= 0)
            Die();

        SetInvincible(true);
        spriteRenderer.color = new Color(1, 0f, 0f, 1f);

        Invoke("OffDamage", 1);
    }

    private void OffDamage()
    {
        SetInvincible(false);
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void Die()  
    {
        animator.SetTrigger("Dead");
        
        rigidBody.simulated = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void DodgeStart()
    {
        SetInvincible(true);
        bIsDodging = true;
        animator.SetBool("Dodge", true);
        motionTrail.MotionTrailStart();
    }   
    
    private void DodgeEnd()
    {
        // 무적 해제, 회피 상태 해제, 애니메이션 변경, 잔상 해제
        SetInvincible(false);
        bIsDodging = false;
        animator.SetBool("Dodge", false);
        motionTrail.MotionTrailEnd();
    }
}
