using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticForcefieldEquipment : Equipment
{
    public float ForcefieldRadius; // how big the range of the forcefield is
    public float ForcefieldPower; // how powerful the forcefield pushback is
    public float ForcefieldUpwardsModifier; // how powerful the forcefield pushback is
    public float ForcefieldLifetime; // how long does the equipment last
    public float ForcefieldCooldown; // how long between blasts

    private float _elapsedTime;
    private float _elapsedTimeSinceLastTrigger;

    public ParticleSystem ForcefieldParticles;

    // Start is called before the first frame update
    void Start()
    {
        ForcefieldParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        _elapsedTimeSinceLastTrigger += Time.deltaTime;

        if (_elapsedTimeSinceLastTrigger > ForcefieldCooldown)
        {
            ForcefieldParticles.Play();
            _elapsedTimeSinceLastTrigger = 0;
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, ForcefieldRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(ForcefieldPower, explosionPos, ForcefieldRadius, ForcefieldUpwardsModifier);
                }
            }
        }

        if (_elapsedTime > ForcefieldLifetime)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
