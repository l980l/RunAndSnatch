using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeNearTrace : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;
    private float nearTraceTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();

        flyingEye.monsterState = MonsterState.NearTrace;
        nearTraceTime = flyingEye.monsterData.nearTraceTime;
        // 애니메이션 재생 속도
        animator.speed = flyingEye.monsterData.NTSCoef;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = flyingEye.DistanceToPlayer();

        // nearTraceRange < 플레이어와의 거리 < farTraceRange
        if (distance > flyingEye.monsterData.nearTraceRange && distance < flyingEye.monsterData.farTraceRange)
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
        if (distance < flyingEye.monsterData.attackRange)
        {
            // 사이에 벽이 없어야 함.
            if (flyingEye.PlayerInSight())
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        flyingEye.rigidBody.velocity = Vector3.zero;
    }
}
