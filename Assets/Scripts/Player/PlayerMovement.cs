using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private ParticleSystem speedUpPS;
    
    private FloatingJoystick joystick;

    private static readonly int DodgeHash = Animator.StringToHash("Dodge");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

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

#if UNITY_ANDROID
        // 모바일 키 세팅
        SetMobileKeys();
#endif
    }

    private void Update()
    {
        if (playerHealth.HP > 0)
            GetInput();
    }

    private void FixedUpdate()
    {
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
        animator.SetFloat(SpeedHash, inputVec.sqrMagnitude);

        // 이동 방향에 맞게 좌우 반전.
        if (inputVec.x != 0)
            spriteRenderer.flipX = inputVec.x < 0;
    }

    private void OnDisable()
    {
        DodgeEnd();
    }

    private void GetInput()
    {
#if UNITY_STANDALONE_WIN
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
#endif


#if UNITY_ANDROID
        inputVec.x = joystick.Horizontal;
        inputVec.y = joystick.Vertical;

        if (!isDodging && playerStamina.Stamina > 0)
        {
            if (MobileKeyManager.instance.RunButtonDown)
            {
                DodgeStart();
            }
        }

        if (isDodging)
        {
            if (!MobileKeyManager.instance.RunButtonDown || playerStamina.Stamina <= 0)
            {
                // 체력이 한번 다 닳면 버튼을 다시 눌러야 달리기 가능. 안 그러면 글리치 남
                MobileKeyManager.instance.RunButtonDown = false;
                DodgeEnd();
            }
        }
#endif
    }

    private void DodgeStart()
    {
        playerHealth.SetInvincible(true);
        isDodging = true;
        animator.SetBool(DodgeHash, true);
        motionTrail.MotionTrailStart();
    }

    private void DodgeEnd()
    {
        playerHealth.SetInvincible(false);
        isDodging = false;
        animator.SetBool(DodgeHash, false);
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

    public void PlaySpeedUpPS()
    {
        speedUpPS.Play();
    }

    private void SetMobileKeys()
    {
        joystick = MobileKeyManager.instance.Joystick;
    }
}
