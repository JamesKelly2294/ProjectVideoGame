using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{

    public List<UpgradeConfiguration> upgradeConfigurations;

    private List<Upgrade> upgrades = new List<Upgrade>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<UpgradeOption> GetRandomOptions(int n) {
        List<UpgradeOption> options = new List<UpgradeOption>();
        foreach (var upgradeConfig in upgradeConfigurations) {
            Upgrade u = Upgrade(upgradeConfig.equipmentConfiguration);
            int uPower = 0, uSpeed = 0, uSpecial = 0, uLifetime = 0;
            if (u != null) {
                uPower = u.power;
                uSpeed = u.speed;
                uSpecial = u.special;
                uLifetime = u.lifetime;
            }

            UpgradeAllowance powerAllowance = upgradeConfig.allowances.Find(x => x.type == UpgradeType.power);
            if (powerAllowance != null && (powerAllowance.max - uPower) > 0) {
                for (int i = 0; i < powerAllowance.weight; i++) { options.Add(new UpgradeOption(upgradeConfig.equipmentConfiguration, UpgradeType.power)); }
            }

            UpgradeAllowance speedAllowance = upgradeConfig.allowances.Find(x => x.type == UpgradeType.speed);
            if (speedAllowance != null && (speedAllowance.max - uSpeed) > 0) {
                for (int i = 0; i < speedAllowance.weight; i++) { options.Add(new UpgradeOption(upgradeConfig.equipmentConfiguration, UpgradeType.speed)); }
            }

            UpgradeAllowance specialAllowance = upgradeConfig.allowances.Find(x => x.type == UpgradeType.special);
            if (specialAllowance != null && (specialAllowance.max - uSpecial) > 0) {
                for (int i = 0; i < specialAllowance.weight; i++) { options.Add(new UpgradeOption(upgradeConfig.equipmentConfiguration, UpgradeType.special)); }
            }

            UpgradeAllowance lifetimeAllowance = upgradeConfig.allowances.Find(x => x.type == UpgradeType.lifetime);
            if (lifetimeAllowance != null && (lifetimeAllowance.max - uLifetime) > 0) {
                for (int i = 0; i < lifetimeAllowance.weight; i++) { options.Add(new UpgradeOption(upgradeConfig.equipmentConfiguration, UpgradeType.lifetime)); }
            }
        }

        // Take n options
        List<UpgradeOption> found = new List<UpgradeOption>();
        for (int i = 0; i < n; i++) {
            if (options.Count < 1) { return found; }

            UpgradeOption chosen = options[UnityEngine.Random.Range(0, options.Count - 1)];
            found.Add(chosen);
            for (int j = options.Count - 1; j >= 0 ; j--) {
                if (options[j].configuration == chosen.configuration && options[j].upgradeType == chosen.upgradeType) {
                    options.RemoveAt(j);
                }
            }
        }

        return found;
    }

    public void PurchaseUpgrade(UpgradeOption option) {
        Upgrade u = Upgrade(option.configuration);
        if (u == null) {
            u = new Upgrade();
            u.equipmentConfiguration = option.configuration;
            upgrades.Add(u);
        }

        switch (option.upgradeType) {
            case UpgradeType.speed:     u.speed += 1;   break;
            case UpgradeType.power:     u.power += 1;   break;
            case UpgradeType.special:   u.special += 1; break;
            case UpgradeType.lifetime:  u.lifetime += 1; break;
        }

        Debug.Log("Purchasing... " + u.equipmentConfiguration.displayName + " now " + u.speed + ", " + u.power + ", " + u.special + ", " + u.lifetime);
    }

    public Upgrade Upgrade(EquipmentConfiguration configuration) {
        return upgrades.Find(x => x.equipmentConfiguration == configuration);
    }

    public Upgrade UpgradeOrZero(EquipmentConfiguration configuration) {
        Upgrade real = Upgrade(configuration);
        if (real == null) {
            Upgrade fake = new Upgrade();
            fake.equipmentConfiguration = configuration;
            return fake;
        } else {
            return real;
        }
    }
}

[Serializable]
public class UpgradeOption {
    public EquipmentConfiguration configuration;
    public UpgradeType upgradeType;

    public UpgradeOption(EquipmentConfiguration configuration, UpgradeType upgradeType) {
        this.configuration = configuration;
        this.upgradeType = upgradeType;
    }
}