using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack1 : StateMachineBehaviour
{
    private MonSkeleton skeleton;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();

        skeleton.monsterState = MonsterState.Attack;
    }
}
