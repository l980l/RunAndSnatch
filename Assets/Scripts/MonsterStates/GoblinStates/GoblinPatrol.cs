using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinPatrol : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float patrolTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin.monsterState = MonsterState.Patrol;
        goblin = animator.GetComponent<MonGoblin>();
        patrolTime = goblin.monsterData.patrolTime;
        // 애니메이션 재생 속도
        animator.speed = 0.5f;
        // 랜덤한 위치로 Patrol 목표 지점 설정
        animator.GetComponent<Navigator>().SetDesTilePos(GameManager.Instance.GetMapGenerator().RandomPos(false));
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 패트롤 함수 사용

        patrolTime -= Time.deltaTime;
        if (patrolTime < 0)
        {
            animator.SetTrigger("Idle");
        }

        if (goblin.DistanceToPlayer() < goblin.monsterData.farTraceRange)
        {
            animator.SetTrigger("FarTrace");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
    }
}
