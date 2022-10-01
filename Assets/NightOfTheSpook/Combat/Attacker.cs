using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [Range(1, 100)]
    public int DamagePerAttack = 1;

    private AttackerTasks _currentTask;
    private bool _collidingWithTarget;

    // The next viable attack time, which is the last time this entity attacked + a cooldown.
    private float _nextAttackTime = 0.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAttackStateMachine();
        HandleAttack();
    }

    void OnCollisionEnter(Collision collision)
    {
        var target = GetAttackTarget().gameObject;
        var other = collision.gameObject;
        Debug.Log($"{name} entered collision with ({other.gameObject.name})");
        if (ReferenceEquals(other, target))
        {
            _collidingWithTarget = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var target = GetAttackTarget().gameObject;
        var other = collision.gameObject;
        Debug.Log($"{name} exited collision with ({other.gameObject.name})");
        if (ReferenceEquals(other, target))
        {
            _collidingWithTarget = false;
        }
    }

    private Attackable GetAttackTarget()
    {
        return PrimaryTarget.GetComponent<Attackable>();
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
                break;
            case AttackerTasks.MovingToTarget:
                // TODO: Collisions with props

                // TODO: switch to rigid body velocity based movement so we can do physicsy things
                float magnitude = MovementSpeedInUnitsPerSecond * Time.deltaTime;
                var heading = target.transform.position - transform.position;
                heading.y = 0.0f;
                var movementVector = magnitude * heading.normalized;
                transform.Translate(movementVector);
                break;
            case AttackerTasks.Attacking:
                // Perform an attack if we're in range.
                if(_nextAttackTime < Time.time)
                {
                    target.InflictDamage(DamagePerAttack);
                    _nextAttackTime = Time.time + AttackCoolDownInSeconds;
                    Debug.Log($"{name} inflicting {DamagePerAttack}; next attack at {_nextAttackTime}");
                }
                break;
            default:
                break;
        };
    }

    private void SwitchToTask(AttackerTasks newTask, String reason = "")
    {
#if DEBUG
        var current = TaskToTaskName(_currentTask);
        var next = TaskToTaskName(newTask);
        if(reason != "")
        {
            Debug.Log($"{name} changing from task {current} to {next} because {reason}");
        }
        else
        {
            Debug.Log($"{name} changing from task {current} to {next}");
        }
#endif
        _currentTask = newTask;
    }
}