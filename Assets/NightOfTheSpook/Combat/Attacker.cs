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

    /**
     * How much time (in seconds) should pass between each attack to register.
     */
    [Range(0.1f, float.MaxValue)]
    public double SecondsBetweenAttacks;

    private AttackerTasks _currentTask;
    private bool _collidingWithTarget;


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
        UpdateMode();
        HandleMode();
    }

    void OnCollisionEnter(Collision collision)
    {
        var target = GetAttackTarget().gameObject;
        var other = collision.gameObject;
        if(ReferenceEquals(other, target))
        {
            _collidingWithTarget = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var target = GetAttackTarget().gameObject;
        var other = collision.gameObject;
        Debug.Log($"Exited collision with ({other.gameObject.name})");
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

    private void UpdateMode()
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
                Debug.LogWarning("Attack mode is undefined! Try setting it to something (e.g. 'Idle')");
                break;
        }
    }

    private void HandleMode()
    {

    }

    private void SwitchToTask(AttackerTasks newTask, String reason = "")
    {
#if DEBUG
        var current = TaskToTaskName(_currentTask);
        var next = TaskToTaskName(newTask);
        if(reason != "")
        {
            Debug.Log($"Changing from task {current} to {next} because {reason}");
        }
        else
        {
            Debug.Log($"Changing from task {current} to {next}");
        }
#endif
        _currentTask = newTask;
    }
}