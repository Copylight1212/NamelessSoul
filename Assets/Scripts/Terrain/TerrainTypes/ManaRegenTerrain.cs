using UnityEngine;

public class ManaRegenTerrain : TerrainModifiers
{
    [Header("Unique Zone Effect")]
    [SerializeField] private float manaRegenPerSecondInZone = 10;
    [SerializeField] private float lingeringManaRegenPerSecond = 5f;
    
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
            res.RegenMana(manaRegenPerSecondInZone * Time.deltaTime);
        }

       
    }
    
    public override void RemoveEffect(GameObject target)
    {

        PlayerResources res = target.GetComponentInParent<PlayerResources>();
        if (res != null)
        {
            res.RegenMana(lingeringManaRegenPerSecond * 0.1f);
        }
    }

}
