using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNearTrace : StateMachineBehaviour
{
    private MonGoblin goblin;
    private float nearTraceTime;

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int RestHash = Animator.StringToHash("Rest");
    private static readonly int FarTraceHash = Animator.StringToHash("FarTrace");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();

        goblin.monsterState = MonsterState.NearTrace;
        nearTraceTime = goblin.monsterData.nearTraceTime;
        // 애니메이션 재생 속도
        animator.speed = goblin.monsterData.NTSCoef;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = goblin.DistanceToPlayer();

        // nearTraceRange < 플레이어와의 거리 < farTraceRange
        if (distance > goblin.monsterData.nearTraceRange && distance < goblin.monsterData.farTraceRange)
        {
            animator.SetTrigger(FarTraceHash);
        }

        // 일정 시간 이상 전력질주 했으면 Rest
        nearTraceTime -= Time.deltaTime;
        if (nearTraceTime < 0)
        {
            animator.SetTrigger(RestHash);
        }

        // 플레이어와의 거리 < attackRange
        if (distance < goblin.monsterData.attackRange)
        {
            // 사이에 벽이 없어야 함.
            if (goblin.PlayerInSight())
            {
                animator.SetTrigger(AttackHash);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 재생 속도
        animator.speed = 1;
        goblin.rigidBody.velocity = Vector3.zero;
    }
}
