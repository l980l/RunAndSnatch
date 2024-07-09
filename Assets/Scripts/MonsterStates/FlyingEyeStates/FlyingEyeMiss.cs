using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeMiss : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;
    private float missTime;

    private static readonly int IdleHash = Animator.StringToHash("Idle");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();
        flyingEye.monsterState = MonsterState.Miss;
        missTime = 1f;

        flyingEye.ShowQuestionMark(true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        missTime -= Time.deltaTime;
        if (missTime < 0)
        {
            animator.SetTrigger(IdleHash);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye.ShowQuestionMark(false);
    }
}
