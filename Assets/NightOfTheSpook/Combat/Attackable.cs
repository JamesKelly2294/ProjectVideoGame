using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    /// <summary>
    /// The number of points of health the attackable entity can have. The range is somewhat arbitrary.
    /// </summary>
    public float TotalHealth = 3.0f;
    
    /// <summary>
    /// The AudioSource that is attached to this component's GameObject. Used to play sound effects (coordinated with the <see cref="AudioManager"/>).
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Identifier for the sound effect that is played when the attackable is killed (e.g. "SFX/EntityNameDeath")
    /// </summary>
    public string DeathSoundID = "";
    public float deathSoundChance, deathSoundVolume;

    /// <summary>
    /// Identifier for the sound effect that is played when the attackable is hurt (e.g. "SFX/EntityNameHurt")
    /// </summary>
    public string HurtSoundID = "";
    public float hurtSoundChance, hurtSoundVolume;

    /// <summary>
    /// Check to see if the attackable entity is still alive.
    /// </summary>
    public bool IsAlive
    {
        get { return Health > 0; }
    }

    /// <summary>
    /// The attackable entity's health. If this hits zero, the entity is dead.
    /// </summary>
    public float Health = 3f;

    /// <summary>
    /// Is this Attackable the player?
    /// </summary>
    public bool IsPlayer = false;

    /// <summary>
    /// The Renderer component that is attached to the Attackable GameObject.
    /// Used for playing with emissivity.
    /// </summary>
    public Renderer innerRenderer;

    private Color _cachedEmissiveColor;
    private float wasDamagedCooldown = 0f;
    private AnimationCurve wasDamagedAnimationCurve;
    private Color damagedColor = new Color(1f, 0, 0, 1f);

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

        if (wasDamagedCooldown <= 0 && audioSource != null) {
            if(ShouldPlayDeathSound)
            {
                PlayDeathSound();
            }
            else if (ShouldPlayHurtSound) {
                PlayHurtSound();
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

    private bool ShouldPlayDeathSound
    {
        get { return Health <= 0.0f && (UnityEngine.Random.Range(0, 1) <= deathSoundChance) && DeathSoundID != ""; }
    }

    private bool ShouldPlayHurtSound
    {
        get { return Health > 0.0f && (UnityEngine.Random.Range(0, 1) <= hurtSoundChance) && HurtSoundID != ""; }
    }

    private void PlayDeathSound()
    {
        AudioManager.Instance.Play(DeathSoundID,
            pitchMin: 0.9f, pitchMax: 1.1f,
            volumeMin: deathSoundVolume, volumeMax: deathSoundVolume,
            position: transform.position,
            minDistance: 10, maxDistance: 20);
    }

    private void PlayHurtSound()
    {
        AudioManager.Instance.Play(HurtSoundID,
            pitchMin: 0.9f, pitchMax: 1.1f,
            volumeMin: hurtSoundVolume, volumeMax: hurtSoundVolume,
            position: transform.position,
            minDistance: 10, maxDistance: 20);
    }
}
