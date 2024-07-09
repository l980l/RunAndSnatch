using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEyeIdle : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;
    private PlayerStealth playerStealth;
    private float idleTime;

    private static readonly int PatrolHash = Animator.StringToHash("Patrol");
    private static readonly int FarTraceHash = Animator.StringToHash("FarTrace");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();
        playerStealth = flyingEye.player.GetComponent<PlayerStealth>();

        flyingEye.monsterState = MonsterState.Idle;
        idleTime = flyingEye.monsterData.idleTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            animator.SetTrigger(PatrolHash);
        }
        // 보이는 경우에만 트레이스 해
        if (flyingEye.DistanceToPlayer() < flyingEye.monsterData.farTraceRange && flyingEye.PlayerInSight() && !playerStealth.Stealth)
        {
            animator.SetTrigger(FarTraceHash);
        }
    }
}
