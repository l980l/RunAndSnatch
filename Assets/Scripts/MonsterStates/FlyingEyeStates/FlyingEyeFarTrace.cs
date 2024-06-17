using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeFarTrace : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();

        flyingEye.monsterState = MonsterState.FarTrace;
        // 애니메이션 재생 속도
        animator.speed = flyingEye.monsterData.FTSCoef;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = flyingEye.DistanceToPlayer();

        // 플레이어로의 거리 < nearTraceRange
        if (distance < flyingEye.monsterData.nearTraceRange)
        {
            animator.SetTrigger("NearTrace");
        }

        // 플레이어로의 거리 > farTraceRange
        if (distance > flyingEye.monsterData.farTraceRange)
        {
            animator.SetTrigger("Patrol");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        flyingEye.rigidBody.velocity = Vector3.zero;
    }
}
