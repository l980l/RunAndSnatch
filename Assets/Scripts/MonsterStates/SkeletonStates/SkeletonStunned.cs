using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunned : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private float stunTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();

        skeleton.monsterState = MonsterState.Stunned;
        stunTime = skeleton.StunTime;
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
