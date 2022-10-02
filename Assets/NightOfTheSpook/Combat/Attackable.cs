using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    /// <summary>
    /// The number of points of health the attackable entity can have. The range is somewhat arbitrary.
    /// </summary>
    [Range(1f, 10f)]
    public float TotalHealth = 3f;

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

    void Start()
    {
        Health = TotalHealth;
    }

    /// <summary>
    /// Inflict damage upon the attackable entity.
    /// </summary>
    /// <param name="amount">The amount of damage to inflict.</param>
    public void InflictDamage(float amount)
    {
        if (Time.timeScale == 0) { return; }
        Health = Math.Max(0.0f, Health - amount);

        // TODO: Make an event / pub-sub system for death
        if ( Health <= 0.0f ) {
            if (GetComponent<DeathAnimation>() == null) {
                gameObject.AddComponent<DeathAnimation>();

                if (IsPlayer) {
                    GetComponent<Player>().Died();
                }
            }
        }
    }
}
