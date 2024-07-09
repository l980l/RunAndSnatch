using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardStunned : StateMachineBehaviour
{
    private MonEvilWizard wizard;
    private float stunTime;

    private static readonly int IdleHash = Animator.StringToHash("Idle");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wizard = animator.GetComponent<MonEvilWizard>();

        wizard.monsterState = MonsterState.Stunned;
        stunTime = wizard.StunTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stunTime -= Time.deltaTime;
        if (stunTime < 0)
        {
            animator.SetTrigger(IdleHash);
        }
    }
}
