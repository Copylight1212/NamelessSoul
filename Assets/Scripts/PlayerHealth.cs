using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 10;
    public int currentHealth = 4;
    public bool inCombat = false;

    void Start()
    {
        StartCoroutine(HealthRegen());
    }

    private Coroutine healthRegenCoroutine;

    public void OnCombatStateChanged(bool inCombat)
    {
        this.inCombat = inCombat;
        
        if (this.inCombat)
        {
            if (healthRegenCoroutine != null)
            {
                StopCoroutine(healthRegenCoroutine);
                healthRegenCoroutine = null;
            }
        }
        else
        {
            if (healthRegenCoroutine == null)
            {
                healthRegenCoroutine = StartCoroutine(HealthRegen());
            }
        }
    }

    IEnumerator HealthRegen()
    {
        while (true)
        { 
            while (!inCombat && currentHealth < maxHealth)
            { 
                currentHealth += 1;
                yield return new WaitForSeconds(1f);
            }
            healthRegenCoroutine = null;
        }
    }
    

}
