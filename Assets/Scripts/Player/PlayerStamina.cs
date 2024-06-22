using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private float maxStamina;
    private float staminaRegenSpeed;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    public float Stamina { get; private set; }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();

        // 원본 데이터 세팅
        maxStamina = playerData.maxStamina;
        Stamina = playerData.maxStamina;
        staminaRegenSpeed = playerData.staminaRegenSpeed;
    }

    private void FixedUpdate()
    {
        if (playerHealth.HP > 0)
        {
            if (!playerMovement.isDodging)
            {
                Stamina = Mathf.Min(maxStamina, Stamina + Time.fixedDeltaTime * staminaRegenSpeed);
            }
            else
            {
                Stamina = Mathf.Max(0, Stamina - Time.fixedDeltaTime);
            }

            // StaminaHUD 세팅
            float Amount = (float)Stamina / (float)maxStamina;
            GameManager.Instance.GetStaminaHUD().UpdateStamina(Amount);
        }
    }

    public void AddStamina(float amount)
    {
        Stamina += amount;
        Stamina = Mathf.Min(maxStamina, Stamina);
        
        float Amount = (float)Stamina / (float)maxStamina;
        GameManager.Instance.GetStaminaHUD().UpdateStamina(Amount);
    }

    public void StaminaRegenSpeedUp(float _amount)
    {
        staminaRegenSpeed += _amount;
    }
}
