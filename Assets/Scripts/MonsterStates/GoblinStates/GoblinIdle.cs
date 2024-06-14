using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinIdle : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float idleTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();

        goblin.monsterState = MonsterState.Idle;
        idleTime = goblin.monsterData.idleTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            animator.SetTrigger("Patrol");
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
