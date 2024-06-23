using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : StateMachineBehaviour
{
    private int nextAction;
    private float duration;
    private static readonly int ActionHash = Animator.StringToHash("Action");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextAction = Random.Range(0, 2) + 1; // 다음 애니메이션 세팅
        duration = 4f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        duration -= Time.deltaTime;
        if (duration < 0f)
        {
            animator.SetInteger(ActionHash, nextAction);
        }
    }
}
