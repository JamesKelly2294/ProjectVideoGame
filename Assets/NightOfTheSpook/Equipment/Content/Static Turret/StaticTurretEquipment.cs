using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
    }

    private float _deltaTime;

    // Update is called once per frame
    void Update()
    {
        _deltaTime += Time.deltaTime;

        ValidateTarget();
        AcquireTarget();
        LookTowardsTarget();
        FireWeapon();
    }

    void ValidateTarget()
    {
        if (target == null)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget > maxAttackRange || distanceToTarget < minAttackRange)
        {
            target = null;
            _deltaTime = 0;
        }
    }

    void FireWeapon()
    {
        if (target == null)
        {
            return;
        }

        if (_deltaTime < attackCooldown)
        {
            return;
        }

        _deltaTime = 0;

        var projectileGO = Instantiate(projectilePrefab);
        projectileGO.transform.parent = transform;
        projectileGO.transform.position = projectileSpawnPosition.position;
        projectileGO.transform.rotation = turretShootyBit.transform.rotation;

        var projectile = projectileGO.GetComponent<Projectile>();
        projectile.damage = attackDamage;

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
                    smallestDistance = distance;
                    target = hit.gameObject;
                }
            }
        }
    }

    void LookTowardsTarget()
    {
        var damping = 2;
        var lookPos = target != null ? (target.transform.position + Vector3.up) - turretShootyBit.transform.position : turretShootyBit.transform.position + transform.forward * 10.0f;
        var rotation = Quaternion.LookRotation(lookPos);
        turretShootyBit.transform.rotation = Quaternion.Slerp(turretShootyBit.transform.rotation, rotation, Time.deltaTime * damping);
    }
}
