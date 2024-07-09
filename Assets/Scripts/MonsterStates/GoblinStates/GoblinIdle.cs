using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinIdle : StateMachineBehaviour
{
    private MonGoblin goblin;
    private PlayerStealth playerStealth;
    private float idleTime;

    private static readonly int PatrolHash = Animator.StringToHash("Patrol");
    private static readonly int FarTraceHash = Animator.StringToHash("FarTrace");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();
        playerStealth = goblin.player.GetComponent<PlayerStealth>();

        goblin.monsterState = MonsterState.Idle;
        idleTime = goblin.monsterData.idleTime;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            animator.SetTrigger(PatrolHash);
        }
        // 보이는 경우에만 트레이스 해
        if (goblin.DistanceToPlayer() < goblin.monsterData.farTraceRange && goblin.PlayerInSight() && !playerStealth.Stealth)
        {
            animator.SetTrigger(FarTraceHash);
        }
    }
}
