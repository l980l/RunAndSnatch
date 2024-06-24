using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPC : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public Vector3 NPCPosition; // CatTown에서 세팅
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool bCorrectPos;
    private Vector2 dir;
    private CapsuleCollider2D capsuleCollider2D;
    private PlayerMovement playerMovement;
    private PlayerStamina playerStamina;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int OnNPCHash = Animator.StringToHash("OnNPC");
    private static readonly int OffNPCHash = Animator.StringToHash("OffNPC");

    // AccountData로 얻어야 하는 정보들
    private bool playable;
    private int giftCount;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStamina = GetComponent<PlayerStamina>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        gameObject.layer = 11;
        bCorrectPos = false;
        playerMovement.enabled = false;
        playerStamina.enabled = false;
        capsuleCollider2D.isTrigger = true;
    }

    private void FixedUpdate()
    {
        // 아직 올바른 위치에 놓인게 아닌 경우
        if (!bCorrectPos)
        {
            dir = NPCPosition - transform.position;
            animator.SetFloat(SpeedHash, dir.sqrMagnitude);

            // 올바른 위치로 갔는지 확인
            if (Vector2.SqrMagnitude(dir) > 0.01f)
            {
                // 이동
                Vector2 nextVec = dir.normalized * playerData.speed * Time.fixedDeltaTime;
                rigidBody.MovePosition(rigidBody.position + nextVec);
            }
            // 제자리에 간 경우, NPC 애니메이션 세팅 및 충돌 세팅
            else
            {
                bCorrectPos = true;
                animator.SetFloat(SpeedHash, 0);
                animator.SetTrigger(OnNPCHash);
            }
        }
    }
    private void LateUpdate()
    {
        // 이동중인 경우
        if(!bCorrectPos)
        {
            // 이동 방향에 맞게 좌우 반전.
            if (dir.x != 0)
                spriteRenderer.flipX = dir.x < 0;

            // z좌표를 y좌표의 0.01프로로 설정
            Vector3 position = transform.position;
            position.z = position.y * 0.01f;
            transform.position = position;
        }
    }

    private void OnDisable()
    {
        gameObject.layer = 6;
        playerMovement.enabled = true;
        playerStamina.enabled = true;
        capsuleCollider2D.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bCorrectPos && collision.gameObject.layer == 6) 
        {
            // 테스트를 위해 바로 플레이어 변경이 되도록 하자.
            collision.GetComponent<PlayerNPC>().enabled = true;

            GameManager.Instance.ChangePlayer(gameObject);
            animator.SetTrigger(OffNPCHash);
            enabled = false;
        }
    }
}
