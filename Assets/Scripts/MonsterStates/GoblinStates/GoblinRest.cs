using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRest : StateMachineBehaviour
{
    private MonGoblin goblin;
    private SpriteRenderer spriteRenderer;
    private float restTime;

    private static readonly int IdleHash = Animator.StringToHash("Idle");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin = animator.GetComponent<MonGoblin>();
        spriteRenderer = goblin.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0.5f, 0.5f, 1f, 1);

        goblin.monsterState = MonsterState.Rest;
        restTime = goblin.monsterData.restTime;

        // 애니메이션 재생 속도
        animator.speed = 0.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        restTime -= Time.deltaTime;
        if (restTime < 0)
        {
            animator.SetTrigger(IdleHash);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spriteRenderer.color = Color.white;
        animator.speed = 1;
    }
}
