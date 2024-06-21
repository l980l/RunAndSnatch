using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRest : StateMachineBehaviour
{
    private MonSkeleton skeleton;
    private SpriteRenderer spriteRenderer;
    private float restTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeleton = animator.GetComponent<MonSkeleton>();
        spriteRenderer = skeleton.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0.5f, 0.5f, 1f, 1);

        skeleton.monsterState = MonsterState.Rest;
        restTime = skeleton.monsterData.restTime;

        // 애니메이션 재생 속도
        animator.speed = 0.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        restTime -= Time.deltaTime;
        if (restTime < 0)
        {
            animator.SetTrigger("Idle");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer.color = Color.white;
        animator.speed = 1;
    }
}
