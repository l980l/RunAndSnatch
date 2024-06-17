using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonSkeleton : Monster
{
    [SerializeField] private BoxCollider2D atkBox;
    protected override void Awake()
    {
        base.Awake();
        atkBox.enabled = false;
    }

    protected override void Attack()
    {
        if (atkBox != null)
        {
            FlipCollider(GetComponent<SpriteRenderer>().flipX);
            atkBox.enabled = true;
            StartCoroutine(DisableAtkBox(0.1f));
        }
    }

    public override void OnStunSkill()   // 플레이어가 스턴 스킬을 사용하면 호출
    {
        // 플레이어가 스켈레톤의 좌측에 있는지.
        bool LeftSidePlayer = true;
        if(player.transform.position.x - transform.position.x >0)
            LeftSidePlayer = false;
        
        // 플레이어를 스켈레톤이 바라보고 있었다면 막기.
        if(LeftSidePlayer == GetComponent<SpriteRenderer>().flipX)
            GetComponent<Animator>().SetTrigger("Shield");
        else
            GetComponent<Animator>().SetTrigger("Stunned");
    }

    private IEnumerator DisableAtkBox(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (atkBox != null)
        {
            atkBox.enabled = false;
        }
    }

    protected void FlipCollider(bool _FlipX)
    {
        if (atkBox != null)
        {
            if (_FlipX)
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
