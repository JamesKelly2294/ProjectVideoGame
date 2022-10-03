using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementModifier
{
    public enum Type
    {
        Goo = 1
    }

    public MonoBehaviour source;
    public Type type;
    public delegate float MultiplierDelegate();

    public MultiplierDelegate multiplier;

    public bool Equals(MovementModifier other)
    {
        return other.source == source;
    }

    public override int GetHashCode()
    {
        return source.GetHashCode();
    }
}

public class Attacker : MonoBehaviour
{
    /// <summary>
    /// The entity that this one will target for attack. Should be an instance of <see cref="Attackable"/>.
    /// </summary>
    public Attackable PrimaryTarget;

    /// <summary>
    /// How quickly the attacker moves toward the target, in units per second.
    /// </summary>
    [Range(0.1f, 10.0f)]
    public float MovementSpeedInUnitsPerSecond;

    /// <summary>
    /// How many seconds it takes for the attacker to be ready to attack again.
    /// </summary>
    [Range(0.1f, 10.0f)]
    public float AttackCoolDownInSeconds;

    /// <summary>
    /// How much damage is done in a single attack.
    /// </summary>
    [Range(0.001f, 100)]
    public float DamagePerAttack = 1.0f;

    private Vector3 _movement = Vector3.zero;

    private AttackerTasks _currentTask;
    private bool _collidingWithTarget;

    // The next viable attack time, which is the last time this entity attacked + a cooldown.
    private float _nextAttackTime = 0.0f;

    private Dictionary<MovementModifier.Type, List<MovementModifier>> _movementModifiers = new Dictionary<MovementModifier.Type, List<MovementModifier>>();

    private enum AttackerTasks
    {
        Idle,
        MovingToTarget,
        Attacking
    }

    void Start()
    {
        // Default to idle until we have a target. Might be null, dead, etc.
        _currentTask = AttackerTasks.Idle;
        _collidingWithTarget = false;

        if (PrimaryTarget == null)
        {
            // try to find the player as a backup
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go)
            {
                PrimaryTarget = go.GetComponent<Attackable>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) { return; }
        UpdateAttackStateMachine();
        HandleAttack();
    }

    public List<MovementModifier> MovementModifiersForType(MovementModifier.Type type)
    {
        return _movementModifiers[type];
    }

    public void ApplyMovementModifier(MovementModifier modifier)
    {
        if (!_movementModifiers.ContainsKey(modifier.type))
        {
            _movementModifiers[modifier.type] = new List<MovementModifier>();
        }
        _movementModifiers[modifier.type].Add(modifier);
    }

    public void RemoveMovementModifier(MovementModifier modifier)
    {
        if (_movementModifiers.ContainsKey(modifier.type))
        {
            _movementModifiers[modifier.type].RemoveAll(m => m.source == modifier.source);
        }
        else
        {
            Debug.LogError("Trying to remove movement modifier... but the modifier isn't applied!");
        }
    }

    public float MovementModifier
    {
        get
        {
            float movementModifierVal = 1.0f;
            foreach (var entry in _movementModifiers)
            {
                if (entry.Value.Count == 0) { continue; }

                // currently, we just take the most powerful modifier for each type
                float currentBest = 0.0f;
                List<MovementModifier> staleModifiers = new List<MovementModifier>();
                foreach (var modifier in entry.Value)
                {
                    if(modifier.source == null)
                    {
                        // The source has been Destroy(...)'d. ignore it, and purge it.
                        staleModifiers.Add(modifier);
                        continue;
                    }

                    var multiplier = modifier.multiplier();
                    if (multiplier > 0 && multiplier > currentBest)
                    {
                        // bigger is better for positive multipliers
                        currentBest = multiplier;
                    }
                    else if (multiplier < 0 && multiplier < currentBest)
                    {
                        // smaller is better for negative multipliers
                        currentBest = multiplier;
                    }
                }

                foreach(var staleModifier in staleModifiers)
                {
                    //Debug.Log("Removing stale modifier with " + staleModifier);
                    entry.Value.RemoveAll(m => m.source == staleModifier.source);
                }

                movementModifierVal *= currentBest;
            }

            return movementModifierVal;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var target = GetAttackTarget().gameObject;
        var other = collision.gameObject;
        // Debug.Log($"{name} entered collision with ({other.name})");
        if (ReferenceEquals(other, target))
        {
            _collidingWithTarget = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var target = GetAttackTarget().gameObject;
        var other = collision.gameObject;
        // Debug.Log($"{name} exited collision with ({other.name})");
        if (ReferenceEquals(other, target))
        {
            _collidingWithTarget = false;
        }
    }

    private Attackable GetAttackTarget()
    {
        return PrimaryTarget != null ? PrimaryTarget.GetComponent<Attackable>() : null;
    }

    private string TaskToTaskName(AttackerTasks task)
    {
        switch (task)
        {
            case AttackerTasks.Idle:
                return "Idle";
            case AttackerTasks.MovingToTarget:
                return "Moving To Target";
            case AttackerTasks.Attacking:
                return "Attacking";
            default:
                return "Unknown";
        }
    }

    private void UpdateAttackStateMachine()
    {
        var target = GetAttackTarget();
        if (target == null && _currentTask != AttackerTasks.Idle)
        {
            SwitchToTask(AttackerTasks.Idle, "no target");
            return;
        }
        else if (target == null)
        {
            // Handle case where we're already ideal here to avoid log spew.
            return;
        }

        if (!target.IsAlive && _currentTask != AttackerTasks.Idle)
        {
            SwitchToTask(AttackerTasks.Idle, "target is dead");
            return;
        }

        switch (_currentTask)
        {
            case AttackerTasks.Idle:
                if (target.IsAlive)
                {
                    SwitchToTask(AttackerTasks.MovingToTarget, "target is alive");
                }

                // Pick a random point to move to. This should occur the first time we go idle,
                // or when we reach a certain distance of the existing target point.
                // TODO: the above
                break;
            case AttackerTasks.MovingToTarget:
                if (_collidingWithTarget)
                {
                    SwitchToTask(AttackerTasks.Attacking);
                }
                break;
            case AttackerTasks.Attacking:
                if (!_collidingWithTarget)
                {
                    SwitchToTask(AttackerTasks.MovingToTarget);
                }
                break;
            default:
                Debug.LogWarning(@"{name} state is undefined! Try setting it to something (e.g. 'Idle')");
                break;
        }
    }

    private void HandleAttack()
    {
        var target = GetAttackTarget();
        switch (_currentTask)
        {
            case AttackerTasks.Idle:
                // TODO: move to the random location we previously picked in UpdateMode
                _movement = Vector3.zero;
                break;
            case AttackerTasks.MovingToTarget:
                // TODO: Collisions with props
                float magnitude = MovementSpeedInUnitsPerSecond * MovementModifier * Time.deltaTime;
                var heading = target.transform.position - transform.position;
                heading.y = 0.0f;
                _movement = magnitude * heading.normalized;
                transform.position += (_movement);
                break;
            case AttackerTasks.Attacking:
                // Perform an attack if we're in range.
                if(_nextAttackTime < Time.time)
                {
                    target.InflictDamage(DamagePerAttack, null);
                    _nextAttackTime = Time.time + AttackCoolDownInSeconds;
                    //Debug.Log($"{name} inflicting {DamagePerAttack}; next attack at {_nextAttackTime}");
                }
                break;
            default:
                break;
        };
    }

    private void SwitchToTask(AttackerTasks newTask, String reason = "")
    {
#if DEBUG
        //var current = TaskToTaskName(_currentTask);
        //var next = TaskToTaskName(newTask);
        //if(reason != "")
        //{
        //    Debug.Log($"{name} changing from task {current} to {next} because {reason}");
        //}
        //else
        //{
        //    Debug.Log($"{name} changing from task {current} to {next}");
        //}
#endif
        _currentTask = newTask;
    }
}