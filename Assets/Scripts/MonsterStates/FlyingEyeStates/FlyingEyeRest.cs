using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeRest : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;
    private float restTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();
        flyingEye.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f, 1);

        flyingEye.monsterState = MonsterState.Rest;
        restTime = flyingEye.monsterData.restTime;

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
        flyingEye.GetComponent<SpriteRenderer>().color = Color.white;
        animator.speed = 1;
    }
}
