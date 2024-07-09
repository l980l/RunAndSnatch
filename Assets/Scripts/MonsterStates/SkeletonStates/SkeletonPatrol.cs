using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPatrol : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private float patrolTime;
    private PlayerStealth playerStealth;

    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int FarTraceHash = Animator.StringToHash("FarTrace");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();
        playerStealth = skeleton.player.GetComponent<PlayerStealth>();

        skeleton.monsterState = MonsterState.Patrol;
        patrolTime = skeleton.monsterData.patrolTime;
        // 애니메이션 재생 속도
        animator.speed = 0.5f;
        // 랜덤한 위치로 Patrol 목표 지점 설정
        animator.GetComponent<Navigator>().SetDesTilePos(GameManager.Instance.GetMapGenerator().RandomPos(false));
        animator.GetComponent<Navigator>().FindPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 패트롤 함수 사용
        patrolTime -= Time.deltaTime;
        if (patrolTime < 0)
        {
            animator.SetTrigger(IdleHash);
        }

        // 보이는 경우에만 트레이스 해
        if (skeleton.DistanceToPlayer() < skeleton.monsterData.farTraceRange && skeleton.PlayerInSight() && !playerStealth.Stealth)
        {
            animator.SetTrigger(FarTraceHash);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        skeleton.rigidBody.velocity = Vector3.zero;
    }
}
