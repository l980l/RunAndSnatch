using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SkeletonShield : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private float shieldTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.PlaySFX(SFX.SkeletonShieldSFX, animator.transform.position);

        skeleton = animator.GetComponent<MonSkeleton>();

        skeleton.monsterState = MonsterState.Rest;
        shieldTime = 0.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shieldTime -= Time.deltaTime;
        if (shieldTime < 0)
        {
            animator.SetTrigger("Idle");
        }
    }
}
