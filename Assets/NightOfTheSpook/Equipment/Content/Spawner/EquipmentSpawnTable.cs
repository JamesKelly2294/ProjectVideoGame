using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EquipmentSpawnRate
{
    public EquipmentConfiguration configuration;
    // Higher value == more likely to spawn
    public int spawnWeight;
}

[CreateAssetMenu(fileName = "EquipmentSpawnTable", menuName = "Equipment/Spawn Table")]
public class EquipmentSpawnTable : ScriptableObject
{
    public List<EquipmentSpawnRate> spawnRates = new List<EquipmentSpawnRate>();
}