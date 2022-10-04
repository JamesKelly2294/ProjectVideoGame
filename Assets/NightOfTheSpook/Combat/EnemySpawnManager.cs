using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies offscreen.
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    [Serializable]
    public class PeriodicSpawnerEntry//<T1, T2, T3>
    {
        [field: SerializeField]
        public EnemySpawner Spawner { get; set; }

        [field: SerializeField]
        public float PeriodInSeconds { get; set; }

        [field: SerializeField]
        public GameObject EnemyPrefab { get; set; }

        [field: SerializeField]
        public int MaxEnemiesPerSpawn { get; set; }

        //public PeriodicSpawnerEntry(EnemySpawner spawner, float periodInSeconds, GameObject enemyPrefab, int maxEnemiesPerSpawn)
        //{
        //    Spawner = spawner;
        //    PeriodInSeconds = periodInSeconds;
        //    EnemyPrefab = enemyPrefab;
        //    MaxEnemiesPerSpawn = maxEnemiesPerSpawn;
        //}
    }

    /// <summary>
    /// The attackable entity the spawn manager will preferentially hit?
    /// </summary>
    public Attackable AttackerTarget;

    /// <summary>
    /// Details for how to spawn enemies on a timer (e.g. every x seconds).
    /// </summary>
    public List<PeriodicSpawnerEntry> PeriodicSpawners = new();

    private List<EnemySpawner> _periodicSpawners = new();
    private Dictionary<EnemySpawner, Func<bool>> _specialSpawnerRuleLookup = new();

    /// <summary>
    /// Requests that the spawner trigger. This might be skipped if the spawner is still cooling down, is visible, etc.
    /// </summary>
    /// <param name="spawner">The spawner to trigger.</param>
    /// <param name="count">The number of enemies that should be spawned.</param>
    public void TriggerSpawner(EnemySpawner spawner, int count)
    {
        Debug.Log("I should try...");
        Debug.Log(spawner.gameObject.activeInHierarchy);
        Debug.Log(spawner.enabled);
        Debug.Log(spawner.isActiveAndEnabled);
        Debug.Log(spawner.SpawnWhenVisible);
        Debug.Log(spawner.IsReadyToSpawn);
        if (spawner != null && spawner.isActiveAndEnabled && (spawner.SpawnWhenVisible ? spawner.IsEligibleForSpawn : spawner.IsReadyToSpawn))
        {
            Debug.Log("I tried...");
            spawner.TriggerGroupSpawn(count);
        }
    }

    /// <summary>
    /// Forces the spawner to trigger now, resetting its cooldown in the process.
    /// </summary>
    /// <param name="spawner">The spawner to forcibly trigger.</param>
    /// <param name="count">The number of enemies that should be spawned.</param>
    public void ForceTriggerSpawner(EnemySpawner spawner, int count)
    {
        if (spawner != null)
        {
            spawner.TriggerGroupSpawn(count);
        }
    }

    public void RegisterSpecialSpawnTrigger(EnemySpawner spawner, Func<bool> fx)
    {
        _specialSpawnerRuleLookup.Add(spawner, fx);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // calculate visibility
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        foreach (var spawner in _periodicSpawners)
        {
            if (!spawner.IsEligibleForSpawn)
            {
                continue;
            }

            var boxCollider = spawner.GetComponent<BoxCollider>();
            if (boxCollider)
            {
                var isVisible = GeometryUtility.TestPlanesAABB(planes, boxCollider.bounds);

                if (isVisible && spawner.IsVisible == false)
                {
                    Debug.Log(boxCollider + " became visible!");
                }
                else if (!isVisible && spawner.IsVisible == true)
                {
                    Debug.Log(boxCollider + " became invisible!");
                }

                spawner.IsVisible = isVisible;
            }
        }

        foreach (var spawner in _periodicSpawners)
        {
            // TODO: Tweak the enemy count here to account for things like the player being low on health.
            var enemyCount = spawner.MaxEnemiesPerSpawn;
            TriggerSpawner(spawner, enemyCount);
        }
        foreach (var kv in _specialSpawnerRuleLookup)
        {
            if(kv.Value())
            {
                var spawner = kv.Key;
                TriggerSpawner(spawner, spawner.MaxEnemiesPerSpawn);
            }
        }
    }

    void Awake()
    {
        foreach (var entry in PeriodicSpawners)
        {
            // Override spawner enemy prefab only if it isn't set.
            entry.Spawner.EnemyPrefab = entry.Spawner.EnemyPrefab == null ? entry.EnemyPrefab : entry.Spawner.EnemyPrefab;
            entry.Spawner.AttackerTarget = AttackerTarget;
            entry.Spawner.MaxEnemiesPerSpawn = entry.MaxEnemiesPerSpawn;
            entry.Spawner.CoolDownTime = entry.PeriodInSeconds;
            _periodicSpawners.Add(entry.Spawner);
        }
    }
}
