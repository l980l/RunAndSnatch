using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWProjectileExplode : StateMachineBehaviour
{
    private EWProjectile projectile;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        projectile = animator.GetComponent<EWProjectile>();
        projectile.IsMove = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
