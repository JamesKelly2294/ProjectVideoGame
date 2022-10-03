using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    /// <summary>
    /// Point to the attackable that we're upgrading to a boss. Needed to access its health.
    /// </summary>
    public Attackable Attackable;

    /// <summary>
    /// List of locations that the boss can teleport to.
    /// </summary>
    public List<Transform> TeleportLocations;

    /// <summary>
    /// How much DPS the boss takes before jumping somewhere.
    /// </summary>
    public float DamagePerSecondBeforeTeleport = 10.0f;

    /// <summary>
    /// How long the teleport animation plays for. Defaults to a second.
    /// </summary>
    public float TeleportAnimationDuration = 1.0f;

    /// <summary>
    /// Animation curve to use when playing the teleport animation arbl garbl warble.
    /// </summary>
    public AnimationCurve TeleportAnimationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    private BossTeleportAnimation _teleportAnimation;
    private float _previousDPSCheckTime = 0.0f;
    private float _previousHealthValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _previousHealthValue = Attackable.Health;
        _previousDPSCheckTime = Time.time;
        ConfigureTeleportAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTeleport();
    }

    private bool ShouldTeleport()
    {
        // This will result in a check every second, so there will be a slight delay before the boss jumps.
        if ((_previousDPSCheckTime + 1.0f) > Time.time)
        {
            return false;
        }

        var result = (_previousHealthValue - Attackable.Health) > DamagePerSecondBeforeTeleport;
        _previousHealthValue = Attackable.Health;
        _previousDPSCheckTime = Time.time;
        return result;
    }

    private void UpdateTeleport()
    {
        if(IsTeleporting)
        {
            // need to wait for the callback to complete.
            return;
        }

        if(ShouldTeleport())
        {
            _teleportAnimation.enabled = true;
        }
    }

    private bool IsTeleporting
    {
        get { return _teleportAnimation != null && _teleportAnimation.enabled; }
    }

    private void HandleTeleportAnimationFinished()
    {
        var nextLocationIndex = Random.Range(0, TeleportLocations.Count);
        var nextLocation = TeleportLocations[nextLocationIndex];
        gameObject.transform.position = nextLocation.position;

        // The previous animation will destroy itself so we need a new one.
        ConfigureTeleportAnimation();
    }

    private void ConfigureTeleportAnimation()
    {
        _teleportAnimation = gameObject.AddComponent<BossTeleportAnimation>();
        _teleportAnimation.scaleCurve = TeleportAnimationCurve;
        _teleportAnimation.duration = TeleportAnimationDuration;
        _teleportAnimation.OnAnimationFinished = HandleTeleportAnimationFinished;
        _teleportAnimation.enabled = false;
    }
}