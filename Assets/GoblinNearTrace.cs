using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNearTrace : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float nearTraceTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();
        nearTraceTime = goblin.monsterData.nearTraceTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = goblin.DistanceToPlayer();

        // 플레이어와의 거리 < attackRange
        if (distance < goblin.monsterData.attackRange)
        {
            animator.SetTrigger("Attack");
        }

        // nearTraceRange < 플레이어와의 거리 < farTraceRange
        if (distance > goblin.monsterData.nearTraceRange && distance < goblin.monsterData.farTraceRange)
        {
            animator.SetTrigger("FarTrace");
        }

        // 일정 시간 이상 전력질주 했으면 Rest
        nearTraceTime -= Time.deltaTime;
        if (nearTraceTime < 0)
        {
            animator.SetTrigger("Rest");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
