using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    /// <summary>
    /// The number of points of health the attackable entity can have. The range is somewhat arbitrary.
    /// </summary>
    public float TotalHealth = 3f;


    public AudioClip deathSound;
    public float deathSoundChance, deathSoundVolume;
    public AudioClip hurtSound;
    public float hurtSoundChance, hurtSoundVolume;
    public AudioSource audioSource;

    /// <summary>
    /// Check to see if the attackable entity is still alive.
    /// </summary>
    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
    }

    /// <summary>
    /// The attackable entity's health. If this hits zero, the entity is dead.
    /// </summary>
    public float Health = 3f;

    public bool IsPlayer = false;

    private float wasDamagedCooldown = 0f;
    private AnimationCurve wasDamagedAnimationCurve;
    private Color damagedColor = new Color(1f,0,0,1f);
    private Renderer innerRenderer;

    private Color _cachedEmissiveColor;

    void Start() {
        Health = TotalHealth;
        innerRenderer = gameObject.GetComponentInChildren<Renderer>();
        wasDamagedAnimationCurve = new AnimationCurve(
            new Keyframe(0,     0),
            new Keyframe(.2f, 10)
        );
    }

    bool _cachedEmission;
    bool _animatingDamage;
    void Update() {
        if (wasDamagedCooldown > 0) {
            wasDamagedCooldown -= Time.deltaTime;
            if (wasDamagedCooldown <= 0) {
                if (!_cachedEmission)
                {
                    innerRenderer.material.DisableKeyword("_EMISSION");
                }
                else
                {
                    innerRenderer.material.SetColor("_EmissionColor", _cachedEmissiveColor);
                    _animatingDamage = false;
                }
            } else {
                innerRenderer.material.SetColor ("_EmissionColor", new Color(1f,0,0,1f) * wasDamagedAnimationCurve.Evaluate(wasDamagedCooldown));
            }
        }
    }

    /// <summary>
    /// Inflict damage upon the attackable entity.
    /// </summary>
    /// <param name="amount">The amount of damage to inflict.</param>
    public void InflictDamage(float amount, Beam beam)
    {
        if (Time.timeScale == 0) { return; }
        Health = Math.Max(0.0f, Health - amount);

        // JAMES Enable the particle system here...
        // if (wasDamagedCooldown <= 0 && beam != null) {
        //     GameObject particles = GameObject.Instantiate(beam.beamParticleSystemPrefab);
        //     particles.transform.position = gameObject.transform.position + Vector3.up;
        //     particles.transform.LookAt(beam.gameObject.transform.position * -1);
        // }

        float ran = UnityEngine.Random.Range(0, 1);
        if (wasDamagedCooldown <= 0 && audioSource != null) {
            if (Health <= 0.0f && ran <= deathSoundChance && deathSound != null) {
                audioSource.PlayOneShot(deathSound, deathSoundVolume);
            } else if (Health > 0.0f && ran <= hurtSoundChance && hurtSound != null) {
                audioSource.PlayOneShot(hurtSound, hurtSoundChance);
            }
        }

        if ( Health <= 0.0f ) {
            innerRenderer.material.DisableKeyword("_EMISSION");

            if (GetComponent<DeathAnimation>() == null) {
                gameObject.AddComponent<DeathAnimation>();

                if (IsPlayer) {
                    GetComponent<Player>().Died();
                }
            }
        } else if (wasDamagedCooldown <= 0) {
            if (!innerRenderer.material.IsKeywordEnabled("_EMISSION"))
            {
                innerRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                if (!_animatingDamage)
                {
                    _cachedEmissiveColor = innerRenderer.material.GetColor("_EmissionColor");
                    _cachedEmission = true;
                }
            }

            _animatingDamage = true;
            wasDamagedCooldown = 0.2f;
        }
    }
}
