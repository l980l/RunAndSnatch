using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStunned : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float stunTime;

    private static readonly int IdleHash = Animator.StringToHash("Idle");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();

        goblin.monsterState = MonsterState.Stunned;
        stunTime = goblin.StunTime;
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
