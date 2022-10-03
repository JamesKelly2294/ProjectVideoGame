using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject equipmentPickupPrefab;
    public EquipmentSpawnTable equipmentSpawnTable;
    public GameObject spawnPoint;

    public bool spawnPickupOnStart;

    [Range(0, 120)]
    public float spawnCooldown = 60;
    [Range(-120, 120)]
    public float spawnCooldownJitter = 30;

    [Header("Bounce Animation")]
    [Range(0.0f, 5.0f)]
    public float verticalMovementTime = 1.0f;
    [Range(0.0f, 1.0f)]
    public float verticalMovementDistance = 0.5f;
    [Range(0.0f, 1.0f)]
    public float verticalOffset = 0.2f;
    public AnimationCurve verticalMovement;

    [Header("Bookkeeping")]
    public float targetElapsedTime;
    public float elapsedTime;

    private GameObject _spawnedPickup;
    private bool _spawnedPickupLastFrame;


    // Start is called before the first frame update
    void Start()
    {
        CalculateNextSpawn();

        if (spawnPickupOnStart)
        {
            elapsedTime = targetElapsedTime;
        }   
    }

    void CalculateNextSpawn()
    {
        targetElapsedTime = elapsedTime + spawnCooldown + Random.Range(-spawnCooldownJitter, spawnCooldownJitter);
    }

    void SpawnPickup()
    {
        if (_spawnedPickup != null)
        {
            return;
        }

        var totalWeights = 0;
        foreach(var spawnRate in equipmentSpawnTable.spawnRates)
        {
            totalWeights += spawnRate.spawnWeight;
        }

        var randomRoll = Random.Range(0, totalWeights);
        var curCount = 0;
        EquipmentConfiguration config = null;
        foreach (var spawnRate in equipmentSpawnTable.spawnRates)
        {
            if(randomRoll >= curCount && randomRoll < (curCount + spawnRate.spawnWeight))
            {
                config = spawnRate.configuration;
                break;
            }
            curCount += spawnRate.spawnWeight;
        }

        _spawnedPickup = Instantiate(equipmentPickupPrefab);
        _spawnedPickup.transform.parent = transform;
        _spawnedPickup.transform.position = spawnPoint.transform.position;
        EquipmentPickup equipmentPickup = _spawnedPickup.GetComponent<EquipmentPickup>();
        equipmentPickup.configuration = config;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (_spawnedPickupLastFrame && _spawnedPickup == null)
        {
            // The last spawned item was destroyed. We should kick off a new timer
            // (this is inefficient, we should really use callbacks)
            CalculateNextSpawn();
        }

        if (_spawnedPickup == null && elapsedTime > targetElapsedTime)
        {
            elapsedTime = 0.0f;
            targetElapsedTime = 0.0f;
            SpawnPickup();
        }

        _spawnedPickupLastFrame = _spawnedPickup != null;

        if (_spawnedPickup)
        {
            var pct = elapsedTime / verticalMovementTime;
            _spawnedPickup.transform.position = new Vector3(_spawnedPickup.transform.position.x,
                transform.position.y + (verticalMovement.Evaluate(pct) * verticalMovementDistance) + verticalOffset,
                _spawnedPickup.transform.position.z);
        }
    }
}
