using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    //[SerializeField] private float speed;
    //[SerializeField] private float DodgeSpeed;
    //[SerializeField] private int maxHP;
    //[SerializeField] private float maxStamina;
    //[SerializeField] private float StaminaRegenSpeed;
    public PlayerData playerData;

    private int HP;
    private float Stamina;
    private Vector2 inputVec;
    private bool bIsDodging; 
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MotionTrail motionTrail;
    private bool stealth;

    public bool Stealth { get { return stealth; } set { stealth = value; } }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        motionTrail= GetComponent<MotionTrail>();
        HP = playerData.maxHP;
        Stamina = playerData.maxStamina;
    }

    void Update()
    {
        if(HP > 0)
            GetInput();
        if (Input.GetMouseButtonDown(0))
            Stealth = !Stealth;
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
                Vector2 nextVec = inputVec.normalized * playerData.dodgeSpeed * Time.fixedDeltaTime;
                rigidBody.MovePosition(rigidBody.position + nextVec);

                // 스테미나 소모
                if (Stamina < 0)
                    Stamina = 0;
                else
                    Stamina -= Time.fixedDeltaTime;
            }
            else
            {
                Vector2 nextVec = inputVec.normalized * playerData.speed * Time.fixedDeltaTime;
                rigidBody.MovePosition(rigidBody.position + nextVec);

                // 스테미나 회복
                if (Stamina < playerData.maxStamina)
                    Stamina += Time.fixedDeltaTime * playerData.staminaRegenSpeed;
                else
                    Stamina = playerData.maxStamina;

            }
            // StaminaHUD 세팅
            float Amount = (float)Stamina / (float)playerData.maxStamina;
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

        // z좌표를 y좌표dml 0.01프로로 설정
        Vector3 position = transform.position;
        position.z = position.y * 0.01f;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.gameObject.layer == 9)   // MonsterAttack Layer
        {
            if(collision.gameObject.GetComponentInParent<Monster>() != null)
            {
                int Damage = collision.gameObject.GetComponentInParent<Monster>().monsterData.damage;
                OnDamage(0);
                //OnDamage(Damage);
            }
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
    private void OnDamage(int damage)
    {
        AddHP(-damage);

        if (HP <= 0)
            Die();

        SetInvincible(true);
        spriteRenderer.color = new Color(1, 0f, 0f, 1f);

        StartCoroutine(OffDamageAfterDelay(0.5f)); // 1초 후 OffDamage 실행
    }

    private IEnumerator OffDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
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

    public void AddHP(int _amount)
    {
        HP += _amount;
        if (HP > playerData.maxHP)
        {
            HP = playerData.maxHP;
        }
        float amount = (float)HP / (float)playerData.maxHP;
        GameManager.Instance.GetHeatlhHUD().UpdateHP(amount);
    }

    public void AddStemina(float _amount)
    {
        Stamina += _amount;
        if (Stamina > playerData.maxStamina)
        {
            Stamina = playerData.maxStamina;
        }
        float Amount = (float)Stamina / (float)playerData.maxStamina;
        GameManager.Instance.GetStaminaHUD().UpdateStamina(Amount);
    }

    public void StaminaRegenSpeedUp(float _amount)
    {
        playerData.staminaRegenSpeed += _amount;
    }

    public void MoveSpeedUp(float _amount)
    {
        playerData.speed += _amount;
        // 이동 속도만 오르면 이상하니까 달리기 속도도 같이 올려주자
        playerData.dodgeSpeed += _amount;
    }
}
