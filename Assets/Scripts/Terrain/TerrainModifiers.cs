using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TerrainModifiers : MonoBehaviour
{
    // ----------------------------
    // Inspector Defaults
    // ----------------------------
    [Header("Lingering Settings")]
    [SerializeField] private bool useLingeringEffectDefault = false;
    [SerializeField] private float lingeringDurationDefault = 0f;

    public virtual bool UseLingeringEffect => useLingeringEffectDefault;
    public virtual float LingeringDuration => lingeringDurationDefault;

    // ----------------------------
    // State Tracking
    // ----------------------------
    private readonly Dictionary<CharacterMovement, Coroutine> activeLinger = new Dictionary<CharacterMovement, Coroutine>();

    private Collider zoneCollider;

    protected virtual void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        zoneCollider.isTrigger = true;
    }

    // ----------------------------
    // Abstract Effect Methods
    // ----------------------------
    public abstract void ApplyEffect(GameObject target);
    public abstract void RemoveEffect(GameObject target);

    // ----------------------------
    // Trigger Handling
    // ----------------------------
    private void OnTriggerEnter(Collider other)
    {
        CharacterMovement move = other.GetComponentInParent<CharacterMovement>();
        if (move != null)
        {
            // cancel lingering on re-entry
            StopLingering(move);
        }

        ApplyEffect(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        ApplyEffect(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterMovement move = other.GetComponentInParent<CharacterMovement>();
        if (move == null)
            return;

        if (!UseLingeringEffect)
        {
            RemoveEffect(other.gameObject);
            return;
        }

        // start lingering
        StopLingering(move);
        Coroutine c = StartCoroutine(LingeringRoutine(move, LingeringDuration));
        activeLinger[move] = c;
    }

    // ----------------------------
    // Lingering Logic
    // ----------------------------
    private IEnumerator LingeringRoutine(CharacterMovement movement, float duration)
    {
        float time = duration;
        const float tick = 0.1f;

        while (time > 0f)
        {
            // effect method handles logic
            RemoveEffect(movement.gameObject);

            yield return new WaitForSeconds(tick);
            time -= tick;
        }

        activeLinger.Remove(movement);
    }

    private void StopLingering(CharacterMovement move)
    {
        if (activeLinger.TryGetValue(move, out Coroutine c))
        {
            StopCoroutine(c);
            activeLinger.Remove(move);
        }
    }

    private void OnDisable()
    {
        foreach (var kv in activeLinger)
            StopCoroutine(kv.Value);

        activeLinger.Clear();
    }

    private void OnDestroy()
    {
        foreach (var kv in activeLinger)
            StopCoroutine(kv.Value);

        activeLinger.Clear();
    }
}
