using System.Collections.Generic;
using UnityEngine;

public class SlowTerrain : TerrainModifiers
{
    [Header("Unique Zone Effects")]
    [Range(0f, 1f)]
    [SerializeField] private float slowPercentInZone = 0.6f; // 60% slow
    [SerializeField] private float lingeringRecoveryPercentPerTick = 0.10f; // 10% recovery per tick
    
    [Header("Lingering Recovery")]
    [SerializeField] private bool useLingeringOverride = true;
    [SerializeField] private float lingeringDurationOverride = 2f;

    public override bool UseLingeringEffect => useLingeringOverride;
    public override float LingeringDuration => lingeringDurationOverride;

    private struct CachedSpeeds
    {
        public float walk;
        public float run;
        public float dash;
    }
    
    private readonly Dictionary<PlayerResources, CachedSpeeds> cache = new Dictionary<PlayerResources, CachedSpeeds>();

    public override void ApplyEffect(GameObject target)
    {
        // Get PlayerResources instead of CharacterMovement
        PlayerResources resources = target.GetComponentInParent<PlayerResources>();
        if (resources == null) return;

        // Cache original speeds if not already cached
        if (!cache.ContainsKey(resources))
        {
            cache[resources] = new CachedSpeeds
            {
                walk = resources.walkSpeed,
                run = resources.runSpeed,
                dash = resources.dashDistance
            };
        }

        CachedSpeeds data = cache[resources];

        // Apply slow continuously
        resources.walkSpeed = data.walk * (1f - slowPercentInZone);
        resources.runSpeed = data.run * (1f - slowPercentInZone);
        resources.dashDistance = data.dash * (1f - slowPercentInZone);
    }

    public override void RemoveEffect(GameObject target)
    {
        PlayerResources resources = target.GetComponentInParent<PlayerResources>();
        if (resources == null) return;
        if (!cache.ContainsKey(resources)) return;

        CachedSpeeds data = cache[resources];

        // If not lingering → restore fully
        if (!UseLingeringEffect)
        {
            resources.walkSpeed = data.walk;
            resources.runSpeed = data.run;
            resources.dashDistance = data.dash;
            cache.Remove(resources);
            return;
        }

        // Lingering recovery (small step per tick)
        float step = lingeringRecoveryPercentPerTick;

        // Current remaining slow = original - current
        float remainingWalkPercent = 1f - (resources.walkSpeed / data.walk);

        if (remainingWalkPercent <= 0f)
        {
            // Fully restored
            resources.walkSpeed = data.walk;
            resources.runSpeed = data.run;
            resources.dashDistance = data.dash;
            cache.Remove(resources);
            return;
        }

        // Recover X% of remaining slow
        float newWalk = Mathf.Lerp(resources.walkSpeed, data.walk, step);
        float newRun = Mathf.Lerp(resources.runSpeed, data.run, step);
        float newDash = Mathf.Lerp(resources.dashDistance, data.dash, step);

        resources.walkSpeed = newWalk;
        resources.runSpeed = newRun;
        resources.dashDistance = newDash;

        // When fully restored, clean up
        if (Mathf.Abs(resources.walkSpeed - data.walk) < 0.01f)
        {
            resources.walkSpeed = data.walk;
            resources.runSpeed = data.run;
            resources.dashDistance = data.dash;
            cache.Remove(resources);
        }
    }
}