using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PubSubListener))]
public class StaticTurretEquipment : Equipment
{
    public GameObject target;
    public GameObject turretShootyBit;
    public GameObject projectilePrefab;

    public Transform projectileSpawnPosition;

    [Range(0, 20.0f)]
    public float maxAttackRange = 12.0f;
    [Range(0, 20.0f)]
    public float minAttackRange = 3.0f;

    [Range(0, 5.0f)]
    public float attackCooldown = 1.0f;

    [Range(0, 50.0f)]
    public float attackDamage = 5.0f;

    [Range(0, 60.0f)]
    public float lifetime = 30.0f;

    public EquipmentConfiguration Configuration;

    private UpgradeManager _upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        var spookyGameManager = FindObjectOfType<SpookyGameManager>();
        _upgradeManager = spookyGameManager.GetComponent<UpgradeManager>();

        RecalculateUpgrades();
    }

    private float _deltaTime;
    private float _elapsedTime;

    // Update is called once per frame
    void Update()
    {
        _deltaTime += Time.deltaTime;
        _elapsedTime += Time.deltaTime;

        ValidateTarget();
        AcquireTarget();
        LookTowardsTarget();
        FireWeapon();
        EvaluateLifetime();
    }

    private float _trueCooldown = 1.0f;
    private float _trueDamage = 1.0f;
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

        _trueLifetime = lifetime * multiplier;
    }

    private void RecalculateTruePower()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(2, upgrade.power);

        _trueDamage = attackDamage * multiplier;
    }

    private void RecalculateTrueCooldown()
    {
        var upgrade = _upgradeManager.UpgradeOrZero(Configuration);
        var multiplier = Mathf.Pow(0.5f, upgrade.speed);

        _trueCooldown = attackCooldown * multiplier;
    }


    void EvaluateLifetime()
    {
        if (_elapsedTime > _trueLifetime)
        {
            var animation = gameObject.AddComponent<EquipmentDestructionAnimation>();
            animation.scaleCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
            animation.duration = 0.5f;
            Destroy(this);
            return;
        }
    }

    void ValidateTarget()
    {
        if (target == null)
        {
            return;
        }

        var attackable = target.GetComponent<Attackable>();
        if (attackable != null && attackable.Health <= 0)
        {
            target = null;
            _deltaTime = 0;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget > maxAttackRange || distanceToTarget < minAttackRange)
        {
            target = null;
            _deltaTime = 0;
            return;
        }
    }

    void FireWeapon()
    {
        if (target == null)
        {
            return;
        }

        if (_deltaTime < _trueCooldown)
        {
            return;
        }

        _deltaTime = 0;

        var projectileGO = Instantiate(projectilePrefab);
        projectileGO.transform.parent = transform;
        projectileGO.transform.position = projectileSpawnPosition.position;
        projectileGO.transform.rotation = turretShootyBit.transform.rotation;

        var projectile = projectileGO.GetComponent<Projectile>();
        projectile.damage = _trueDamage;

        AudioManager.Instance.Play("SFX/TurretFire",
                pitchMin: 0.9f, pitchMax: 1.1f,
                volumeMin: 0.5f, volumeMax: 0.5f,
                position: projectileGO.transform.position,
                minDistance: 10, maxDistance: 20);
    }

    void AcquireTarget()
    {
        if (target != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, maxAttackRange, 1 << LayerConstants.Enemy);
        float smallestDistance = Mathf.Infinity;
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < smallestDistance && distance > minAttackRange)
                {
                    var attackable = hit.GetComponent<Attackable>();
                    if (attackable == null) { continue; }
                    if (attackable.Health > 0)
                    {
                        smallestDistance = distance;
                        target = hit.gameObject;
                    } 
                }
            }
        }
    }

    void LookTowardsTarget()
    {
        var damping = 5;
        var lookPos = target != null ? (target.transform.position) - turretShootyBit.transform.position : turretShootyBit.transform.position + transform.forward * 10.0f;
        var rotation = Quaternion.LookRotation(lookPos);
        turretShootyBit.transform.rotation = Quaternion.Slerp(turretShootyBit.transform.rotation, rotation, Time.deltaTime * damping);
    }
}
