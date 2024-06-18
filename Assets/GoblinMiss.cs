using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinMiss : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float missTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();
        goblin.monsterState = MonsterState.Miss;
        missTime = 1f;

        goblin.ShowQuestionMark(true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        missTime -= Time.deltaTime;
        if (missTime < 0)
        {
            animator.SetTrigger("Idle");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin.ShowQuestionMark(false);
    }
}
