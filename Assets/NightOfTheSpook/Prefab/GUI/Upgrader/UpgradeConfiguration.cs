using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UpgradeConfiguration", menuName = "Equipment/UpgradeConfiguration", order = 1)]
public class UpgradeConfiguration : ScriptableObject
{
    public string displayName = "Upgrade";
    public EquipmentConfiguration equipmentConfiguration;

    public List<UpgradeAllowance> allowances;
}

[Serializable]
public class UpgradeAllowance {

    public UpgradeType type;
    public string displayName;
    public string displayDescription;
    public int max;
    public float weight;
}

public enum UpgradeType {
    power, speed, special, lifetime
}