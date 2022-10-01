using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    /// <summary>
    /// The number of points of health the attackable entity can have. The range is somewhat arbitrary.
    /// </summary>
    [Range(1, 10)]
    public int TotalHealth = 3;

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
    [HideInInspector]
    public int Health = 3;

    void Start()
    {
        Health = TotalHealth;
    }

    /// <summary>
    /// Inflict damage upon the attackable entity.
    /// </summary>
    /// <param name="amount">The amount of damage to inflict.</param>
    public void InflictDamage(int amount)
    {
        Health = Math.Max(0, Health - amount);
    }
}
