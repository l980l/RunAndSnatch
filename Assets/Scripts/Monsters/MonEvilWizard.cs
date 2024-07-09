using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonEvilWizard : Monster
{
    [SerializeField] private GameObject projectilePrefab;
    private List<GameObject> projectiles;
    private List<EWProjectile> EWProjectiles;
    [SerializeField] private Transform zapPosition;

    protected override void Awake()
    {
        base.Awake();
        projectiles = new List<GameObject>();
        EWProjectiles = new List<EWProjectile>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(monsterState == MonsterState.Attack)
        {
            StareAtPos(player.transform.position);
        }
    }

    protected override void Attack()
    {
        SoundManager.Instance.PlaySFX(SFX.EvilWizardAttackSFX, transform.position);

        bool addNew = true;

        int Length = projectiles.Count;
        for (int i = 0; i < Length; ++i)
        {
            // 이미 만들어진 비사용중인 projectile이 있다면.
            if (!projectiles[i].activeSelf)
            {
                addNew = false;

                projectiles[i].SetActive(true);
                projectiles[i].transform.position = FirePos();
                Vector2 dir = (player.transform.position - projectiles[i].transform.position).normalized;
                EWProjectiles[i].Dir = dir;
                EWProjectiles[i].StareAtDirection(dir);
                EWProjectiles[i].IsMove = true;
                break;
            }
        }

        // 새로 생성. 자식으로 만들어 줌. 그래야 플레이어에서 투사체 데미지를 부모에게서 얻으면 돼서 편함
        if(addNew)
        {
            GameObject newOne = Instantiate(projectilePrefab, transform);
            projectiles.Add(newOne);
            EWProjectile NewEWProjectile = newOne.GetComponent<EWProjectile>();
            EWProjectiles.Add(NewEWProjectile);

            newOne.transform.position = FirePos();
            Vector2 dir = (player.transform.position - newOne.transform.position).normalized;

            NewEWProjectile.Dir = dir;
            NewEWProjectile.StareAtDirection(dir);
            NewEWProjectile.IsMove = true;
        }
    }

    public override void OnStunSkill(float _stunTime)   // 플레이어가 스턴 스킬을 사용하면 호출
    {
        base.OnStunSkill(_stunTime);
        animator.SetTrigger(StunnedHash);
    }
    
    // EvilWizard는 Raycast를 지팡이 위치에서 시작해야 한다.
    public override bool PlayerInSight()
    {
        // 플레이어까지의 방향 벡터를 계산
        Vector3 directionToPlayer = player.transform.position - FirePos();

        // 레이캐스트를 사용하여 몬스터와 플레이어 사이에 장애물이 있는지 확인. 12번 레이어가 Wall임.
        int layerMask = 1 << 12;

        if (!Physics2D.Raycast(FirePos(), directionToPlayer, directionToPlayer.magnitude, layerMask))
        {
            // 레이캐스트가 아무것도 맞추지 않았다면 true
            return true;
        }
        return false;
    }

    private Vector3 FirePos()
    {
        Vector3 zapLocation = zapPosition.localPosition;
        zapLocation = spriteRenderer.flipX ? new Vector3(-zapLocation.x, zapLocation.y, zapLocation.z) : zapLocation;

        return transform.position + zapLocation;
    }
}
