using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGoblin : Monster
{
    [SerializeField] private BoxCollider2D atkBox;
    private WaitForSeconds waitForSeconds;

    protected override void Awake()
    {
        waitForSeconds = new WaitForSeconds(0.1f);
        base.Awake();
        atkBox.enabled = false;
    }

    protected override void Attack()
    {
        SoundManager.Instance.PlaySFX(SFX.GoblinAttackSFX, transform.position);

        if (atkBox != null)
        {
            FlipCollider(GetComponent<SpriteRenderer>().flipX);
            atkBox.enabled = true;
            StartCoroutine(DisableAtkBox());
        }
    }

    public override void OnStunSkill(float _stunTime)   // 플레이어가 스턴 스킬을 사용하면 호출
    {
        base.OnStunSkill(_stunTime);
        animator.SetTrigger(StunnedHash);
    }

    private IEnumerator DisableAtkBox()
    {
        yield return waitForSeconds;
        if (atkBox != null)
        {
            atkBox.enabled = false;
        }
    }

    protected void FlipCollider(bool _FlipX)
    {
        if(atkBox != null)
        {
            if(_FlipX)
            {
                if (atkBox.offset.x > 0)
                {
                    Vector3 colliderPos = atkBox.offset;
                    colliderPos.x = -colliderPos.x;
                    atkBox.offset = colliderPos;
                }
            }
            else
            {
                if (atkBox.offset.x < 0)
                {
                    Vector3 colliderPos = atkBox.offset;
                    colliderPos.x = -colliderPos.x;
                    atkBox.offset = colliderPos;
                }
            }
        }
    }
}
