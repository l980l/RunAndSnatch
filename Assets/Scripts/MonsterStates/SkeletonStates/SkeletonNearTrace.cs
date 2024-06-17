using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonNearTrace : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private float nearTraceTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();

        skeleton.monsterState = MonsterState.NearTrace;
        nearTraceTime = skeleton.monsterData.nearTraceTime;
        // 애니메이션 재생 속도
        animator.speed = skeleton.monsterData.NTSCoef;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = skeleton.DistanceToPlayer();

        // nearTraceRange < 플레이어와의 거리 < farTraceRange
        if (distance > skeleton.monsterData.nearTraceRange && distance < skeleton.monsterData.farTraceRange)
        {
            animator.SetTrigger("FarTrace");
        }

        // 일정 시간 이상 전력질주 했으면 Rest
        nearTraceTime -= Time.deltaTime;
        if (nearTraceTime < 0)
        {
            animator.SetTrigger("Rest");
        }

        // 플레이어와의 거리 < attackRange
        if (distance < skeleton.monsterData.attackRange)
        {
            // 사이에 벽이 없어야 함.
            if (skeleton.PlayerInSight())
            {
                animator.SetTrigger("Attack");
            }
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        skeleton.rigidBody.velocity = Vector3.zero;
    }
}
