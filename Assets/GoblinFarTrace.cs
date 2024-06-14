using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFarTrace : StateMachineBehaviour
{
    private MonGoblin goblin;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = goblin.DistanceToPlayer();
       
        // 플레이어로의 거리 < nearTraceRange
        if (distance < goblin.monsterData.nearTraceRange)
        {
            animator.SetTrigger("NearTrace");
        }

        // 플레이어로의 거리 > farTraceRange
        if (distance > goblin.monsterData.farTraceRange)
        {
            animator.SetTrigger("Patrol");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
