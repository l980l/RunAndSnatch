using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : StateMachineBehaviour
{
    private float duration;
    private static readonly int Action1Hash = Animator.StringToHash("Action1");
    private static readonly int Action2Hash = Animator.StringToHash("Action2");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        duration = Random.Range(3f, 5f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        duration -= Time.deltaTime;
        if (duration < 0f)
        {
            int nextAction = Random.Range(0, 2); // 다음 애니메이션 세팅
            if (nextAction == 0)
                animator.SetTrigger(Action1Hash);
            else
                animator.SetTrigger(Action2Hash);
        }
    }
}
