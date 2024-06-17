using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeStunned : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;
    private float stunTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();

        flyingEye.monsterState = MonsterState.Stunned;
        stunTime = 2f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stunTime -= Time.deltaTime;
        if (stunTime < 0)
        {
            animator.SetTrigger("Idle");
        }
    }
}
