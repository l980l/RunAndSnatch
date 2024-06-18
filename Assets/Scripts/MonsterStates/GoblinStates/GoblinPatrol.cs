using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinPatrol : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float patrolTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();

        goblin.monsterState = MonsterState.Patrol;
        patrolTime = goblin.monsterData.patrolTime;
        // 애니메이션 재생 속도
        animator.speed = 0.5f;
        // 랜덤한 위치로 Patrol 목표 지점 설정
        animator.GetComponent<Navigator>().SetDesTilePos(GameManager.Instance.GetMapGenerator().RandomPos(false));
        animator.GetComponent<Navigator>().FindPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 패트롤 함수 사용
        patrolTime -= Time.deltaTime;
        if (patrolTime < 0)
        {
            animator.SetTrigger("Idle");
        }

        // 보이는 경우에만 트레이스 해
        if (goblin.DistanceToPlayer() < goblin.monsterData.farTraceRange && goblin.PlayerInSight() && !goblin.player.GetComponent<Player>().Stealth)
        {
            animator.SetTrigger("FarTrace");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        goblin.rigidBody.velocity = Vector3.zero;
    }
}
