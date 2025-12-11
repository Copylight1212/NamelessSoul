using UnityEngine;

public class FireDamageTerrain : TerrainModifiers
{
    [Header("Unique Zone Effect")]
    [SerializeField] private float damagePerSecondInZone = 20f;
    [SerializeField] private float lingeringDamagePerSecond = 10f;

    [Header("Lingering Burn")]
    [SerializeField] private bool useLingeringEffectOverride = true;
    [SerializeField] private float lingeringDurationOverride = 5f;

    public override bool UseLingeringEffect => useLingeringEffectOverride;
    public override float LingeringDuration => lingeringDurationOverride;

    public override void ApplyEffect(GameObject target)
    {
        PlayerResources res = target.GetComponentInParent<PlayerResources>();
        if (res != null)
        {
            res.UseHealth(damagePerSecondInZone * Time.deltaTime);
        }
    }

    public override void RemoveEffect(GameObject target)
    {
        PlayerResources res = target.GetComponentInParent<PlayerResources>();
        if (res != null)
        {
            res.UseHealth(lingeringDamagePerSecond * 0.1f);
        }
    }
}