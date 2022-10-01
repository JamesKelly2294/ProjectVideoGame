using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EquipmentConfiguration", menuName = "Equipment/Configuration", order = 1)]
public class EquipmentConfiguration : ScriptableObject
{
    public string displayName = "Equipment";

    public GameObject pickupPrefab;
    public GameObject deployedPrefab;

    public Image inventoryIcon;

    // Higher value == more likely to spawn
    public int baseSpawnWeight = 1;

    public int baseMaxCharges = 3;
}
