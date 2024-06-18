using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardAttack : StateMachineBehaviour
{
    private MonEvilWizard wizard;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wizard = animator.GetComponent<MonEvilWizard>();

        wizard.monsterState = MonsterState.Attack;
    }
}
