using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinPatrol : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float patrolTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();
        patrolTime = goblin.monsterData.patrolTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 패트롤 함수 사용

        patrolTime -= Time.deltaTime;
        if (patrolTime < 0)
        {
            animator.SetTrigger("Idle");
        }

        if (goblin.DistanceToPlayer() < goblin.monsterData.farTraceRange)
        {
            animator.SetTrigger("FarTrace");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
