using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerResources : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 500f;
    [SerializeField] private float currentHealth;
    
    [Header("Mana")]
    public float maxMana = 200f;
    [SerializeField] private float currentMana;
    
    [Header("Stamina")]
    public float maxStamina = 30f;
    [SerializeField] private float currentStamina;
    
    [Header("Movement Settings")]
    public float walkSpeed = 100f;
    public float runSpeed = 200f;
    public float jumpForce = 500f;
    public float gravity = 1500f;
    private float verticalVelocity;
    
    [Header("Dash Settings")]
    public float dashDistance = 200f;
    public float dashDuration = 0.2f;
    
    [Header("Resource Costs")]
    public float jumpDrainCost = 5f;
    public float dashDrainCost = 5f;
    public float staminaDrainRate = 5f;
    
    [Header("Regeneration Settings")]
    public float staminaRegenRate = 5f;
    public float manaRegenRate = 3f;
    public float manaRegenDelay = 2f;
    public float healthRegenRate = 5f;
    public float healthRegenDelay = 5f;
    
    private float lastManaUseTime = 0f;
    private float lastDamageTime = 0f;
    
    [Header("Skill Settings")]
    public float skillInitialManaCost = 20f;
    public float skillManaPerSecondCost = 5f;
    public string skillTriggerName = "Skill";
    public float skillCastTime = 2.0f;
    
    [Header("Skill Visual")]
    public GameObject skillVeilObject;
    public GameObject skillCapeObject;
    public float delayedAppearTime = 5f;
    
    [Header("UI References")]
    public Image healthBarFillImage;
    public Image manaBarFillImage;
    public Image staminaBarFillImage;    
    
    public TextMeshProUGUI healthText; 
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI staminaText;
    
    // Property for vertical velocity
    public float VerticalVelocity => verticalVelocity;
    
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        
        UpdateResourceUI();
    }

    public void UpdateResourceUI()
    {
        UpdateHealthBarUI();
        UpdateManaBarUI();
        UpdateStaminaBarUI();
    }
    
    // ========== HEALTH ==========
    
    private void UpdateHealthBarUI()
    {
        if (healthBarFillImage != null)
        {
            healthBarFillImage.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth:F0}/{maxHealth}";
        }
    }

    public bool CanUseHealth(float amount)
    {
        return currentHealth >= amount;
    }

    public void UseHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        lastDamageTime = Time.time;
        UpdateResourceUI();
    }
    
    public void RegenHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateResourceUI();
    }
    
    public void HandleHealthRegen(float deltaTime)
    {
        if (Time.time >= lastDamageTime + healthRegenDelay)
        {
            RegenHealth(healthRegenRate * deltaTime);
        }
    }
    
    public float HealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    public void ApplyDamage(float amount)
    {
        UseHealth(amount);
    }
    
    // ========== MANA ==========
    
    private void UpdateManaBarUI()
    {
        if (manaBarFillImage != null)
        {
            manaBarFillImage.fillAmount = currentMana / maxMana;
        }

        if (manaText != null)
        {
            manaText.text = $"{currentMana:F0}/{maxMana}";
        }
    }

    public bool CanUseMana(float amount)
    {
        return currentMana >= amount;
    }

    public void UseMana(float amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        UpdateResourceUI();
    }

    public void RegenMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        UpdateResourceUI();
    }
    
    public void RecordManaUse()
    {
        lastManaUseTime = Time.time;
    }
    
    public void HandleManaRegen(float deltaTime)
    {
        if (Time.time >= lastManaUseTime + manaRegenDelay)
        {
            RegenMana(manaRegenRate * deltaTime);
        }
    }

    public float ManaPercentage()
    {
        return currentMana / maxMana;
    }
    
    // ========== STAMINA ==========
    
    private void UpdateStaminaBarUI()
    {
        if (staminaBarFillImage != null)
        {
            staminaBarFillImage.fillAmount = currentStamina / maxStamina;
        }

        if (staminaText != null)
        {
            staminaText.text = $"{currentStamina:F0}/{maxStamina}";
        }
    }
    
    public bool CanUseStamina(float amount)
    {
        return currentStamina >= amount;
    }
    
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateResourceUI();
    }
    
    public void RegenStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateResourceUI();
    }
    
    public float StaminaPercentage()
    {
        return currentStamina / maxStamina;
    }
    
    // ========== PHYSICS ==========
    
    public void ApplyGravity(float deltaTime)
    {
        verticalVelocity -= gravity * deltaTime;
    }
    
    public void SetVerticalVelocity(float value)
    {
        verticalVelocity = value;
    }
    
    public void ResetVerticalVelocity()
    {
        verticalVelocity = -20f;
    }
}