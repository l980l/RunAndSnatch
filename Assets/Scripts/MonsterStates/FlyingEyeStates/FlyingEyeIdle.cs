using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEyeIdle : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;
    private Player player;
    private float idleTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();
        player = flyingEye.player.GetComponent<Player>();

        flyingEye.monsterState = MonsterState.Idle;
        idleTime = flyingEye.monsterData.idleTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            animator.SetTrigger("Patrol");
        }
        // 보이는 경우에만 트레이스 해
        if (flyingEye.DistanceToPlayer() < flyingEye.monsterData.farTraceRange && flyingEye.PlayerInSight() && !player.Stealth)
        {
            animator.SetTrigger("FarTrace");
        }
    }
}
