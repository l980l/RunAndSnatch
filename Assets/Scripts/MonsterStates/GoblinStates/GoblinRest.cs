using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRest : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float restTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();

        goblin.monsterState = MonsterState.Rest;
        restTime = goblin.monsterData.restTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        restTime -= Time.deltaTime;
        if (restTime < 0)
        {
            animator.SetTrigger("Idle");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
