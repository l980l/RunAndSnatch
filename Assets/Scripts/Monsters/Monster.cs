using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState    // 공통으로 사용하는 상태만 
{
    Idle,
    Patrol,
    FarTrace,
    NearTrace,
    Attack,
    Rest,
    Stunned,
    Miss,
    Max
}

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    public MonsterState monsterState;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public Navigator nav;

    [SerializeField] private GameObject questionMark;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    private float pathUpdateTimer = 0f;
    private float pathUpdateInterval = 1f; // 1초마다 경로 갱신
    private float stunTime;
    public float StunTime { get { return stunTime; } }

    protected static readonly int StunnedHash = Animator.StringToHash("Stunned");
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int PatrolHash = Animator.StringToHash("Patrol");
    private static readonly int MissHash = Animator.StringToHash("Miss");

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        nav = GetComponent<Navigator>();
        player = GameManager.Instance.GetPlayer();
        questionMark.SetActive(false);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        pathUpdateTimer += Time.fixedDeltaTime;

        switch (monsterState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Patrol:
                // 목표 설정은 Patrol 초기에 한번만 세팅하면 되니까 각 몬스터의 PatrolState에서 Enter시에 하자
                // Patrol은 전부 0.5배속으로 통일하자
                TracePath(monsterData.speed * 0.5f);
                // 목적지에 도착했는데, Patrol라면 Idle로 돌아가자
                if (nav.curPathIndex >= nav.totalWorldPath.Count)
                {
                    animator.SetTrigger(IdleHash);
                }
                break;
            case MonsterState.FarTrace:
                // 시야에 들어오고, 1초가 지났으면 탐색.
                if (PlayerInSight() && pathUpdateTimer >= pathUpdateInterval)
                {
                    nav.SetDesTilePos(player.transform.position);
                    nav.FindPath();
                    pathUpdateTimer = 0f; // 타이머 리셋
                }
                TracePath(monsterData.speed * monsterData.FTSCoef);
                // 목적지에 도착했는데, FarTrace라면 Patrol로 돌아가자
                if (!PlayerInSight() && nav.curPathIndex >= nav.totalWorldPath.Count)
                {
                    animator.SetTrigger(PatrolHash);
                }
                break;
            case MonsterState.NearTrace:
                // 시야에 들어오고, 1초가 지났으면 탐색.
                if (PlayerInSight() && pathUpdateTimer >= pathUpdateInterval)
                {
                    nav.SetDesTilePos(player.transform.position);
                    nav.FindPath();
                    pathUpdateTimer = 0f; // 타이머 리셋
                }
                TracePath(monsterData.speed * monsterData.NTSCoef);
                // 목적지에 도착했는데, NearTrace라면 플레이어로 A* 없이 그냥 이동한다. 
                if (nav.curPathIndex >= nav.totalWorldPath.Count)
                {
                    MoveToPlayer(monsterData.speed * monsterData.NTSCoef);
                }
                break;
            case MonsterState.Attack:
                break;
            case MonsterState.Rest:
                break;
            case MonsterState.Stunned:
                break;
        }
    }

    private void LateUpdate()
    {
        // z좌표를 y좌표dml 0.01프로로 설정
        Vector3 position = transform.position;
        position.z = position.y * 0.01f;
        transform.position = position;
    }

    virtual protected void Attack() {}

    // Player가 스턴 스킬을 사용하면 호출될 함수
    virtual public void OnStunSkill(float _stunTime) 
    {
        stunTime = _stunTime;
    }

    // Player가 은신 스킬을 사용하면 호출될 함수
    public void Miss()
    {
        // FarTrace, NearTrace, Attack, Rest에만 Transition을 걸어두었기 때문에, 다른 상태에서는 넘어가지지 않는다.
        animator.SetTrigger(MissHash);
    }

    // 은신 State에서 호출될 함수
    public void ShowQuestionMark(bool _show)
    {
        questionMark.SetActive(_show);
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }

    // 플레이어와 몬스터 사이에 벽이 없는지 확인하는 함수.이 함수는 FarTrace에서 목표 지점 세팅에 쓰일 수 있고, 몬스터가 공격 가능한지 판단하는데에도 사용할 수 있다.
    virtual public bool PlayerInSight()
    {
        // 플레이어까지의 방향 벡터를 계산
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // 레이캐스트를 사용하여 몬스터와 플레이어 사이에 장애물이 있는지 확인. 12번 레이어가 Wall임.
        int layerMask = 1 << 12;
        if (!Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask))
        {
            // 레이캐스트가 아무것도 맞추지 않았다면 true
            return true;
        }
        return false;
    }

    //prevPathDest로의 길로 이동하는 함수
    private void TracePath(float _speed)
    {
        if (nav.totalWorldPath == null || nav.totalWorldPath.Count == 0)
        {
            Logging.Log("경로 없음");
            return;
        }

        if (nav.curPathIndex >= nav.totalWorldPath.Count)
        {
            Logging.Log("경로를 모두 따라갔습니다.");
            return;
        }

        Vector3 targetPosition = nav.totalWorldPath[nav.curPathIndex];
        float distanceToMove = _speed * Time.fixedDeltaTime;


        Vector2 newPosition = Vector2.MoveTowards(rigidBody.position, targetPosition, distanceToMove);
        rigidBody.MovePosition(newPosition);

        StareAtPos(targetPosition);

        if (Vector3.Distance(rigidBody.position, targetPosition) < 0.1f)
        {
            nav.curPathIndex++;
        }
    }

    private void MoveToPlayer(float _speed)
    {
        if (player != null)
        {
            // 플레이어의 위치로 이동
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Vector2 newPosition = rigidBody.position + direction * _speed * Time.fixedDeltaTime;
            rigidBody.MovePosition(newPosition);

            StareAtPos(player.transform.position);
        }
    }

    protected void StareAtPos(Vector3 _pos)
    {
        // 이동 방향에 따라 spriteRenderer의 FlipX 설정
        if (_pos.x < transform.position.x)
        {
            spriteRenderer.flipX = true; // 왼쪽을 바라봄
        }
        else if (_pos.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // 오른쪽을 바라봄
        }
    }
}
