using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerResources : MonoBehaviour
{
    public float maxHealth = 500f;
    [SerializeField] private float currentHealth; // [SerializeField] makes it visible in the Inspector

    public float maxMana = 200f;
    [SerializeField] private float currentMana;
    
    public float maxStamina = 30f;
    [SerializeField] private float currentStamina;
    
    // Add Image variables here to hold references to the UI elements
    // We will assign these in the Inspector later
    public Image healthBarFillImage;
    public Image manaBarFillImage;
    public Image staminaBarFillImage;    
    
    public TextMeshProUGUI healthText; 
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI staminaText;
    
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
    
    private void UpdateHealthBarUI()
    {
        if (healthBarFillImage != null)
        {
            healthBarFillImage.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
            // healthText.text = $"{(currentHealth / maxHealth * 100):F0}%"; displays %
        }
    }

    private void UpdateManaBarUI()
    {
        if (manaBarFillImage != null)
        {
            manaBarFillImage.fillAmount = currentMana / maxMana;
        }

        if (manaText != null)
        {
            manaText.text = $"{currentMana}/{maxMana}";
        }
    }
    
    private void UpdateStaminaBarUI()
    {
        if (staminaBarFillImage != null)
        {
            staminaBarFillImage.fillAmount = currentStamina / maxStamina;
        }

        if (staminaText != null)
        {
            staminaText.text = $"{currentStamina}/{maxStamina}";
        }
    }
    
    public void UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            UpdateResourceUI();
        }
    }

    public void RegenStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateResourceUI(); // Keep UI updated
    }
    
    public float StaminaPercentage()
    {
        return currentStamina / maxStamina;
    }

    
    
    
    public void TakeDamage(float amount)
        {
            //consider overhealth/shield/temphp
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevents health going below 0 or above max
            UpdateResourceUI(); // Crucial: Call this function every time the data changes
        }

    public void UseMana(float amount)
        {
            //consider neg. mana as game mechanic
            currentMana -= amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            UpdateResourceUI();
        }
        
   
}
    
    
    

