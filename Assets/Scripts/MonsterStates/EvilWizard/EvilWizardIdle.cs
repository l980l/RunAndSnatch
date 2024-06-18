using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardIdle : StateMachineBehaviour
{
    private MonEvilWizard wizard;
    private float idleTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wizard = animator.GetComponent<MonEvilWizard>();

        wizard.monsterState = MonsterState.Idle;
        idleTime = wizard.monsterData.idleTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            animator.SetTrigger("Patrol");
        }
        // 보이는 경우에만 트레이스 해
        if (wizard.DistanceToPlayer() < wizard.monsterData.farTraceRange && wizard.PlayerInSight() && !wizard.player.GetComponent<Player>().Stealth)
        {
            animator.SetTrigger("FarTrace");
        }
    }
}
