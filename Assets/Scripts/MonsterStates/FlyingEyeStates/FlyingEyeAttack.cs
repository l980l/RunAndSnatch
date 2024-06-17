using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeAttack : StateMachineBehaviour
{
    private MonFlyingEye flyingEye;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flyingEye = animator.GetComponent<MonFlyingEye>();
        flyingEye.monsterState = MonsterState.Attack;

        // 플레이어 방향 세팅
        if (flyingEye.player != null)
        {
            flyingEye.Dir = (flyingEye.player.transform.position - flyingEye.transform.position).normalized;
        }
    }
}
