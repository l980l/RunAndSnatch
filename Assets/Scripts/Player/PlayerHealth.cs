using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    
    private int maxHP;
    public int HP {  get; private set; }
    private bool invincible;
    private SpriteRenderer spriteRenderer;
    private PlayerStealth playerStealth;
    [SerializeField] private ParticleSystem smallHealPS;
    [SerializeField] private ParticleSystem largeHealPS;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStealth = GetComponent<PlayerStealth>();

        // 원본 데이터 세팅
        maxHP = playerData.maxHP;
        HP = playerData.maxHP;
    }

    public void PlaySmallHealPS()
    {
        smallHealPS.Play();
    }

    public void PlayLargeHealPS()
    {
        largeHealPS.Play();
    }

    public void AddHP(int amount)
    {
        HP += amount;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        float healthAmount = (float)HP / (float)maxHP;
        GameManager.Instance.GetHeatlhHUD().UpdateHP(healthAmount);
    }

    public void SetInvincible(bool value)
    {
        invincible = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible && collision.gameObject.layer == 9)   // MonsterAttack Layer
        {
            if (collision.gameObject.GetComponentInParent<Monster>() != null)
            {
                int damage = collision.gameObject.GetComponentInParent<Monster>().monsterData.damage;
                OnDamage(0);
                // OnDamage(damage);
            }
        }
    }

    private void OnDamage(int damage)
    {
        AddHP(-damage);

        if (HP <= 0)
        {
            Die();
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFX.CatHitSFX, Camera.main.transform.position);

            SetInvincible(true);
            spriteRenderer.color = new Color(1, 0f, 0f, 1f);
            StartCoroutine(OffDamageAfterDelay(0.5f));
        }
    }

    private IEnumerator OffDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetInvincible(false);
        spriteRenderer.color = (playerStealth.Stealth == true) ? new Color(1, 1, 1, 0.5f) : Color.white;
    }

    private void Die()
    {
        SoundManager.Instance.PlaySFX(SFX.DeathSFX, Camera.main.transform.position);

        DeathUI.Instance.Death();
    }
}
