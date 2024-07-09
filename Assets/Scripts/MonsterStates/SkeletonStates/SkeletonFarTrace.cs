using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFarTrace : StateMachineBehaviour
{
    private MonSkeleton skeleton;

    private static readonly int PatrolHash = Animator.StringToHash("Patrol");
    private static readonly int NearTraceHash = Animator.StringToHash("NearTrace");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();

        skeleton.monsterState = MonsterState.FarTrace;
        // 애니메이션 재생 속도
        animator.speed = skeleton.monsterData.FTSCoef;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = skeleton.DistanceToPlayer();

        // 플레이어로의 거리 < nearTraceRange
        if (distance < skeleton.monsterData.nearTraceRange)
        {
            animator.SetTrigger(NearTraceHash);
        }

        // 플레이어로의 거리 > farTraceRange
        if (distance > skeleton.monsterData.farTraceRange)
        {
            animator.SetTrigger(PatrolHash);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        skeleton.rigidBody.velocity = Vector3.zero;
    }
}
