using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardMiss : StateMachineBehaviour
{
    private MonEvilWizard wizard;
    private float missTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wizard = animator.GetComponent<MonEvilWizard>();
        wizard.monsterState = MonsterState.Miss;
        missTime = 1f;

        wizard.ShowQuestionMark(true);
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
        wizard.ShowQuestionMark(false);
    }
}
