using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdle : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private float idleTime;
    private PlayerStealth playerStealth;

    private static readonly int PatrolHash = Animator.StringToHash("Patrol");
    private static readonly int FarTraceHash = Animator.StringToHash("FarTrace");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();
        playerStealth = skeleton.player.GetComponent<PlayerStealth>();

        skeleton.monsterState = MonsterState.Idle;
        idleTime = skeleton.monsterData.idleTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            animator.SetTrigger(PatrolHash);
        }
        // 보이는 경우에만 트레이스 해
        if (skeleton.DistanceToPlayer() < skeleton.monsterData.farTraceRange && skeleton.PlayerInSight() && !playerStealth.Stealth)
        {
            animator.SetTrigger(FarTraceHash);
        }
    }
}
