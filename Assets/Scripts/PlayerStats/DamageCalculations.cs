using UnityEngine;

public class DamageCalculations : MonoBehaviour
{

    public PlayerResources PlayerResources; // Drag your Player object here in the Inspector

    public void DeveloperDMGTestButton()
    {
        if (PlayerResources != null)
        {
            PlayerResources.TakeDamage(10);
        }
    }
    
    
}
