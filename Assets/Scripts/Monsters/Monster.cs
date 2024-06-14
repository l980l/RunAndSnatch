using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Patrol,
    FarTrace,
    NearTrace,
    Attack,
    Rest,
    Stunned,
    Max
}

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    [HideInInspector] public MonsterState monsterState;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Rigidbody2D rigidbody;
    [HideInInspector] public Navigator Nav;

    private Vector3 prevPathDest;   // 마지막 길찾기 당시 목표 위치
    public int curPathIndex;

    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Nav = GetComponent<Navigator>();
        player = GameManager.Instance.GetPlayer();
    }

    private void FixedUpdate()
    {
        switch (monsterState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Patrol:
                // 목표 설정은 Patrol 초기에 한번만 세팅하면 되니까 각 몬스터의 PatrolState에서 Enter시에 하자
                // Patrol은 전부 0.5배속으로 통일하자
                TracePath(monsterData.speed * 0.5f);
                break;
            case MonsterState.FarTrace: 
                // 플레이어가 보이면 해당 위치 기억
                if(PlayerInSight())
                {
                    // 마지막 도착 위치와 플레이어와의 거리
                    if (LastDestToPlayerDis() > 3f)
                    {
                        prevPathDest = player.transform.position;
                        Nav.SetDesTilePos(prevPathDest);
                        Nav.FindPath();
                        curPathIndex = 0;
                    }
                }
                TracePath(monsterData.speed * monsterData.FTSCoef);
                break;
            case MonsterState.NearTrace:
                // 가까울 때는 무조건 좇아감
                if (LastDestToPlayerDis() > 1f)
                {
                    prevPathDest = player.transform.position;
                    Nav.SetDesTilePos(prevPathDest);
                    Nav.FindPath();
                    curPathIndex = 0;
                }
                TracePath(monsterData.speed * monsterData.NTSCoef);
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
        // 방향 전환
        FaceToDir(rigidbody.velocity);
    }

    public void FaceToDir(Vector3 _dir)
    {
        if(_dir.x>0)
            GetComponent<SpriteRenderer>().flipX = false;
        else if(_dir.x<0)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }

    private float LastDestToPlayerDis()
    {
        return Vector2.Distance(prevPathDest, player.transform.position);
    }

    // 플레이어와 몬스터 사이에 벽이 없는지 확인하는 함수. 이 함수는 FarTrace에서 목표 지점 세팅에 쓰일 수 있고, 몬스터가 공격 가능한지 판단하는데에도 사용할 수 있다. 
    public bool PlayerInSight()
    {
        // 플레이어까지의 방향 벡터를 계산
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // 레이캐스트를 사용하여 몬스터와 플레이어 사이에 장애물이 있는지 확인. 12번 레이어가 Wall임.
        if (!Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, 12))
        {
            // 레이캐스트가 아무것도 맞추지 않았다면 true
            return true;
        }
        return false;
    }

    // prevPathDest로의 길로 이동하는 함수
    private void TracePath(float _speed)
    {
        if (Nav.totalWorldPath == null || Nav.totalWorldPath.Count == 0)
        {
            return;
        }

        if (curPathIndex >= Nav.totalWorldPath.Count)
        {
            UnityEngine.Debug.Log("경로를 모두 따라갔습니다.");
            return;
        }

        Vector3 targetPosition = Nav.totalWorldPath[curPathIndex];
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distanceToMove = _speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToMove);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            curPathIndex++;
        }

        Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, targetPosition, distanceToMove);

        rigidbody.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            curPathIndex++;
        }
    }
}
