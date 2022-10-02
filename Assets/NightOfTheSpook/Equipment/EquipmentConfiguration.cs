using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EquipmentConfiguration", menuName = "Equipment/Configuration", order = 1)]
public class EquipmentConfiguration : ScriptableObject
{
    public enum EquipmentType
    {
        Deployable = 0,
        Throwable = 1,
    }


    public string displayName = "Equipment";

    public EquipmentType type = EquipmentType.Deployable;

    public GameObject pickupPrefab;
    public GameObject deployedPrefab;

    // would prefer a programatic way of doing this
    public GameObject blueprintPrefab;

    public Sprite inventoryIcon;

    public int baseMaxCharges = 3;
}
