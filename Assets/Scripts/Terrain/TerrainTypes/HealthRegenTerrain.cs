using UnityEngine;

public class HealthRegenTerrain : TerrainModifiers
{
    [Header("Unique Zone Effect")]
    [SerializeField] private float healthRegenPerSecondInZone = 10;
    [SerializeField] private float lingeringHealthRegenPerSecond = 5f;
    
    [Header("Lingering Health Regen")]
    [SerializeField] private bool useLingeringEffectOverride = true;
    [SerializeField] private float lingeringDurationOverride = 5f;
    
    public override bool UseLingeringEffect => useLingeringEffectOverride;
    public override float LingeringDuration => lingeringDurationOverride;

    public override void ApplyEffect(GameObject target)
    {
       
        PlayerResources res = target.GetComponentInParent<PlayerResources>();
        if (res != null)
        {
            res.RegenHealth(healthRegenPerSecondInZone * Time.deltaTime);
        }
    }
    
    public override void RemoveEffect(GameObject target)
    {
        

        PlayerResources res = target.GetComponentInParent<PlayerResources>();
        if (res != null)
        {
            res.RegenHealth(lingeringHealthRegenPerSecond * 0.1f);
        }
    }
    
}
