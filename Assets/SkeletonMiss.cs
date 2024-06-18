using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMiss : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private float missTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();
        skeleton.monsterState = MonsterState.Miss;
        missTime = 1f;

        skeleton.ShowQuestionMark(true);
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
        skeleton.ShowQuestionMark(false);
    }
}
