using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonFlyingEye : Monster
{
    [SerializeField] private CircleCollider2D atkCircle;
    private Vector2 dir;
    public Vector2 Dir { set { dir = value; } }

    protected override void Awake()
    {
        base.Awake();
        atkCircle.enabled = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(monsterState == MonsterState.Attack)
        {
            StareAtPos(player.transform.position);

            if (player != null)
            {
                Vector2 nextVec = dir * monsterData.speed * monsterData.NTSCoef * 1.2f * Time.deltaTime;
                // 플레이어 방향으로 이동
                rigidBody.MovePosition(rigidBody.position + nextVec);
            }
        }
    }

    protected override void Attack()
    {
        SoundManager.Instance.PlaySFX(SFX.FlyingEyeAttackSFX, transform.position);
        if (atkCircle != null)
        {
            atkCircle.enabled = true;
            StartCoroutine(DisableAtkCircle(0.5f));
        }
    }

    public override void OnStunSkill(float _stunTime)   // 플레이어가 스턴 스킬을 사용하면 호출
    {
        base.OnStunSkill(_stunTime);
        animator.SetTrigger(StunnedHash);
    }

    private IEnumerator DisableAtkCircle(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (atkCircle != null)
        {
            atkCircle.enabled = false;
        }
    }
}
