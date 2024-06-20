using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int maxHP;
    private float maxStamina;
    private float staminaRegenSpeed;
    private float speed;
    private float dodgeSpeed;

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

    [SerializeField] private GameObject StealthArea;
    [SerializeField] private GameObject stunArea;

    public bool Stealth { get { return stealth; } set { stealth = value; } }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        motionTrail= GetComponent<MotionTrail>();

        // 던전에서만 능력치 향상이 적용되고, 마을로 가면 사라지게 하기 위해, SO에 직접 적용하지 않고, 값을 복사해서 적용.
        maxHP = playerData.maxHP;
        maxStamina = playerData.maxStamina;
        staminaRegenSpeed = playerData.staminaRegenSpeed;
        speed = playerData.speed;
        dodgeSpeed = playerData.dodgeSpeed;

        HP = playerData.maxHP;
        Stamina = playerData.maxStamina;
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
            if (Input.GetButtonDown("Fire3"))
            {
                DodgeStart();
            }
        }

        if(bIsDodging)
        {
            if (Input.GetButtonUp("Fire3") || Stamina <= 0)
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
        
        spriteRenderer.color = (Stealth == true) ? new Color(1, 1, 1, 0.5f) : Color.white;
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
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        float amount = (float)HP / (float)maxHP;
        GameManager.Instance.GetHeatlhHUD().UpdateHP(amount);
    }

    public void AddStemina(float _amount)
    {
        Stamina += _amount;
        if (Stamina > maxStamina)
        {
            Stamina = maxStamina;
        }
        float Amount = (float)Stamina / (float)maxStamina;
        GameManager.Instance.GetStaminaHUD().UpdateStamina(Amount);
    }

    public void StaminaRegenSpeedUp(float _amount)
    {
        staminaRegenSpeed += _amount;
    }

    public void MoveSpeedUp(float _amount)
    {
        speed += _amount;
        // 이동 속도만 오르면 이상하니까 달리기 속도도 같이 올려주자
        dodgeSpeed += _amount;
    }

    // 일정 시간 동안 stealth 변수를 true로 설정하는 함수
    public void SetStealthForDuration(float duration, float radius = -1)
    {
        StartCoroutine(StealthCoroutine(duration, radius));
    }

    private IEnumerator StealthCoroutine(float duration, float radius = -1)
    {
        // stealth 활성화
        stealth = true;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        if(radius > 0) 
            StealthArea.GetComponent<CircleCollider2D>().radius = radius;
        StealthArea.SetActive(true);

        // duration 동안 대기
        yield return new WaitForSeconds(duration);

        // stealth 비활성화
        stealth = false;
        spriteRenderer.color = Color.white;
        StealthArea.SetActive(false);
    }

    public void SetStunAreaForDuration(float duration, float radius = -1)
    {
        StartCoroutine(StunAreaCoroutine(duration, radius));
    }

    private IEnumerator StunAreaCoroutine(float duration, float radius = -1)
    {
        stunArea.GetComponent<StunArea>().StunTime = duration;
        if (radius > 0)
            StealthArea.GetComponent<CircleCollider2D>().radius = radius;
        stunArea.SetActive(true);

        // 0.1초 동안 충돌체 유지
        yield return new WaitForSeconds(0.1f);

        stunArea.SetActive(false);
    }
}
