using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTurretEquipment : Equipment
{
    public GameObject target;
    public GameObject turretShootyBit;

    [Range(0, 20.0f)]
    public float maxAttackRange = 12.0f;
    [Range(0, 20.0f)]
    public float minAttackRange = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget > maxAttackRange || distanceToTarget < minAttackRange)
            {
                target = null;
            }
        }

        if (target == null)
        {
            AcquireTarget();
        }

        LookTowardsTarget();
    }

    void AcquireTarget()
    {
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

        if (target != null)
        {
            Debug.Log("Turret acquired target = " + target);
        }
    }

    void LookTowardsTarget()
    {
        var damping = 2;
        var lookPos = target != null ? target.transform.position - turretShootyBit.transform.position : turretShootyBit.transform.position + transform.forward;
        var rotation = Quaternion.LookRotation(lookPos);
        turretShootyBit.transform.rotation = Quaternion.Slerp(turretShootyBit.transform.rotation, rotation, Time.deltaTime * damping);
    }
}
