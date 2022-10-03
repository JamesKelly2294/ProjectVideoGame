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
            AudioManager.Instance.Play("SFX/ForcefieldFire",
                pitchMin: 0.9f, pitchMax: 1.1f,
                volumeMin: 0.5f, volumeMax: 0.5f,
                position: transform.position,
                minDistance: 10, maxDistance: 20);

            ForcefieldParticles.Play();
            _elapsedTimeSinceLastTrigger = 0;
            Vector3 explosionPos = transform.position + Vector3.up * 1;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, ForcefieldRadius, 1 << LayerConstants.Enemy);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if (rb.gameObject.layer != LayerConstants.Enemy) { continue; }
                    rb.AddExplosionForce(ForcefieldPower, explosionPos, ForcefieldRadius, ForcefieldUpwardsModifier);
                }
            }
        }

        if (_elapsedTime > ForcefieldLifetime)
        {
            var animation = gameObject.AddComponent<EquipmentDestructionAnimation>();
            animation.scaleCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
            animation.duration = 0.5f;
            Destroy(this);
            return;
        }
    }
}
