using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubListener))]
public class StaticForcefieldEquipment : Equipment
{
    public float ForcefieldRadius; // how big the range of the forcefield is
    public float ForcefieldPower; // how powerful the forcefield pushback is
    public float ForcefieldUpwardsModifier; // how powerful the forcefield pushback is
    public float ForcefieldLifetime; // how long does the equipment last
    public float ForcefieldCooldown; // how long between blasts

    public EquipmentConfiguration Configuration;

    private UpgradeManager _upgradeManager;

    private float _elapsedTime;
    private float _elapsedTimeSinceLastTrigger;

    public ParticleSystem ForcefieldParticles;

    private Vector3 _startingScale;

    // Start is called before the first frame update
    void Start()
    {
        ForcefieldParticles.Stop();

        _startingScale = transform.localScale;

        var spookyGameManager = FindObjectOfType<SpookyGameManager>();
        _upgradeManager = spookyGameManager.GetComponent<UpgradeManager>();

        RecalculateUpgrades();
    }

    private float _trueCooldown = 1.0f;
    private float _trueRadius = 1.0f;
    private float _truePower = 1.0f;
    private float _trueLifetime = 1.0f;
    public void RecalculateUpgrades()
    {
        RecalculateTrueLifetime();
        RecalculateTruePower();
        RecalculateTrueCooldown();
    }

    private void RecalculateTrueLifetime()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(1.5f, upgrade.lifetime);

        _trueLifetime = ForcefieldLifetime * multiplier;
    }

    private void RecalculateTruePower()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var radiusMultiplier = Mathf.Pow(1.25f, upgrade.power);
        var powerMultiplier = Mathf.Pow(1.25f, upgrade.power);

        _trueRadius = ForcefieldRadius * radiusMultiplier;
        _truePower = ForcefieldPower * powerMultiplier;
        transform.localScale = _startingScale * radiusMultiplier;
    }

    private void RecalculateTrueCooldown()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(0.5f, upgrade.speed);

        _trueCooldown = ForcefieldCooldown * multiplier;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        _elapsedTimeSinceLastTrigger += Time.deltaTime;

        if (_elapsedTimeSinceLastTrigger > _trueCooldown)
        {
            AudioManager.Instance.Play("SFX/ForcefieldFire",
                pitchMin: 0.9f, pitchMax: 1.1f,
                volumeMin: 0.5f, volumeMax: 0.5f,
                position: transform.position,
                minDistance: 10, maxDistance: 20);

            ForcefieldParticles.Play();
            _elapsedTimeSinceLastTrigger = 0;
            Vector3 explosionPos = transform.position + Vector3.up * 1;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, _trueRadius, 1 << LayerConstants.Enemy);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if (rb.gameObject.layer != LayerConstants.Enemy) { continue; }
                    rb.AddExplosionForce(_truePower, explosionPos, _trueRadius, ForcefieldUpwardsModifier);
                }
            }
        }

        if (_elapsedTime > _trueLifetime)
        {
            var animation = gameObject.AddComponent<EquipmentDestructionAnimation>();
            animation.scaleCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
            animation.duration = 0.5f;
            Destroy(this);
            return;
        }
    }
}
