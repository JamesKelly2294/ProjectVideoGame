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

    public Sprite inventoryIcon;

    public int baseMaxCharges = 3;
}
