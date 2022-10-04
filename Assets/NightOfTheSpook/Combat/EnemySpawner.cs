using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// If populated, enemies will be spawned within a random spot in the collider volume.
    /// </summary>
    [Header("Required")]
    public Collider ColliderForBounds;

    /// <summary>
    /// Overrides whatever enemy prefab is set by the manager.
    /// Don't assume this is set to anything! Check before using.
    /// </summary>
    [Header("Overrides - only touch if you know what's going on.")]
    public GameObject EnemyPrefab;

    /// <summary>
    /// The attackable that the spawner will target. Please set this programmatically.
    /// </summary>
    public Attackable AttackerTarget;

    /// <summary>
    /// The maximum number of enemies that can spawn from here. Can be overriden by the manager is present.
    /// </summary>
    public int MaxEnemiesPerSpawn;

    /// <summary>
    /// How long it takes for the spawner to recharge after triggering. Can be overriden by the manager is present.
    /// </summary>
    public float CoolDownTime;

    /// <summary>
    /// Waits this many seconds before spawning anything.
    /// </summary>
    public float DelaySpawningUntilAfterSeconds = 0.0f;

    /// <summary>
    /// Stops spawning enemies after this many seconds.
    /// </summary>
    public float StopSpawningAfterSeconds = 0.0f;

    /// <summary>
    /// The spawner will create enemies even when the player can see it.
    /// </summary>
    public bool SpawnWhenVisible = false;

    // Determines when the spawner can be invoked again. The Manager can forcibly invoke the spawner if needed.
    private float _nextSpawnTime = 0.0f;

    // HACK: true on start to prevent onscreen spawners from doing things
    // on start before the visibility check has occured.
    public bool IsVisible = true;

    private GameObject _enemiesContainer;

    private void Start()
    {
        _enemiesContainer = GameObject.FindGameObjectWithTag("Enemies");

        if (AttackerTarget == null)
        {
            AttackerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Attackable>();
        }
    }

    /// <summary>
    /// Spawn a wave of enemies.
    /// </summary>
    /// <param name="count">How many to spawn</param>
    /// <returns>A GameObject containing all of the spawned enemies.</returns>
    public GameObject TriggerGroupSpawn(int count)
    {
        if (ColliderForBounds == null)
        {
            Debug.Log("{name} no spawn area specified; see Bounds");
            return null;
        }

        var bounds = ColliderForBounds.bounds;
        for (int i = 0; i < count; ++i)
        {
            var rx = Random.Range(bounds.min.x, bounds.max.x);
            var ry = 0.0f; // the ground
            var rz = Random.Range(bounds.min.z, bounds.max.z);
            var position = new Vector3(rx, ry, rz);
            _ = TriggerSpawn(position); // probably do something to specify a parent here.
        }
        Debug.Log($"{name} spawned {count} enemies of type {EnemyPrefab.name}");

        // TODO: Hack for now, need to add plumbing to specify a parent above
        return gameObject;
    }

    /// <summary>
    /// Spawn a single instance of an enemy at the given location.
    /// </summary>
    /// <param name="where">The place where the enemy should be spawned.</param>
    /// <returns></returns>
    public GameObject TriggerSpawn(Vector3 where)
    {
        var newEnemy = Instantiate(EnemyPrefab, where, Quaternion.identity, _enemiesContainer != null ? _enemiesContainer.transform : gameObject.transform);
        newEnemy.GetComponent<Attacker>().PrimaryTarget = AttackerTarget;
        newEnemy.SetActive(true);
        
        _nextSpawnTime = Time.time + CoolDownTime;
        return newEnemy;
    }

    /// <summary>
    /// Checks if the spawner is ready to swim upstream and do its duty.
    /// </summary>
    public bool IsReadyToSpawn
    {
        get
        {
            return !IsVisible && IsEligibleForSpawn;
        }
    }

    /// <summary>
    /// Like IsReadyToSpawn, but without the visibility check
    /// </summary>
    public bool IsEligibleForSpawn
    {
        get
        {
            bool isNotCoolingDown = _nextSpawnTime < Time.time;
            bool pastDelay = DelaySpawningUntilAfterSeconds < Time.time;
            bool beforeShutoffTime = StopSpawningAfterSeconds > Time.time;
            return pastDelay && beforeShutoffTime && isNotCoolingDown;
        }
    }
}
