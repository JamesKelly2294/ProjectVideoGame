using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    /// <summary>
    /// The number of points of health the attackable entity can have. The range is somewhat arbitrary.
    /// </summary>
    [Range(1, 10000)]
    public int TotalHealth = 100;

    /// <summary>
    /// Check to see if the attackable entity is still alive.
    /// </summary>
    public bool IsAlive
    {
        get
        {
            return _health > 0;
        }
    }

    private int _health;

    void Start()
    {
        _health = TotalHealth;
    }

    /// <summary>
    /// Inflict damage upon the attackable entity.
    /// </summary>
    /// <param name="amount">The amount of damage to inflict.</param>
    public void InflictDamage(int amount)
    {
        _health = Math.Max(0, amount);
    }
}
