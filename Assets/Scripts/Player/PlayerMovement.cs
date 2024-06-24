using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public PlayerData PlayerData { get { return playerData; } }

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float speed { get; private set; }
    private float dodgeSpeed;
    [HideInInspector] public bool onMovingSkill;

    private Vector2 inputVec;
    public bool isDodging { get; private set; }
    private MotionTrail motionTrail;

    private PlayerStamina playerStamina;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        motionTrail = GetComponent<MotionTrail>();
        playerStamina = GetComponent<PlayerStamina>();
        playerHealth = GetComponent<PlayerHealth>();

        // 원본 데이터 세팅
        speed = playerData.speed;
        dodgeSpeed = playerData.dodgeSpeed;
    }

    private void Update()
    {
        if (playerHealth.HP > 0)
            GetInput();
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", inputVec.sqrMagnitude);

        if (playerHealth.HP > 0)
        {
            // 이동 관련 스킬을 사용 중이지 않은 경우
            if (!onMovingSkill)
            {
                // 회피 중이면 
                if (isDodging)
                {
                    // 이동
                    Vector2 nextVec = inputVec.normalized * dodgeSpeed * Time.fixedDeltaTime;
                    rigidBody.MovePosition(rigidBody.position + nextVec);
                }
                else
                {
                    Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
                    rigidBody.MovePosition(rigidBody.position + nextVec);
                }
            }
        }
    }

    private void LateUpdate()
    {
        // 이동 방향에 맞게 좌우 반전.
        if (inputVec.x != 0)
            spriteRenderer.flipX = inputVec.x < 0;

        // z좌표를 y좌표의 0.01프로로 설정
        Vector3 position = transform.position;
        position.z = position.y * 0.01f;
        transform.position = position;
    }

    private void OnDisable()
    {
        DodgeEnd();
    }

    private void GetInput()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (!isDodging && playerStamina.Stamina > 0)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                DodgeStart();
            }
        }

        if (isDodging)
        {
            if (Input.GetButtonUp("Fire3") || playerStamina.Stamina <= 0)
            {
                DodgeEnd();
            }
        }
    }

    private void DodgeStart()
    {
        playerHealth.SetInvincible(true);
        isDodging = true;
        animator.SetBool("Dodge", true);
        motionTrail.MotionTrailStart();
    }

    private void DodgeEnd()
    {
        playerHealth.SetInvincible(false);
        isDodging = false;
        animator.SetBool("Dodge", false);
        motionTrail.MotionTrailEnd();
    }

    public void MoveSpeedUp(float _amount)
    {
        speed += _amount;
        // 이동 속도만 오르면 이상하니까 달리기 속도도 같이 올려주자. 기본 속도에 비한 현재 속도를 구해서 적용.
        dodgeSpeed = playerData.dodgeSpeed * (speed / playerData.speed);
        // 애니메이터 재생 속도를 기본 속도일때를 1로 잡고 세팅.
        animator.speed = speed / playerData.speed;
    }
}
