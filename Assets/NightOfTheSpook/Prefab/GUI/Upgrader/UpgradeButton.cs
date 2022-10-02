using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{

    public TextMeshProUGUI title;
    public UpgradeOption upgradeOption;
    
    private UpgradeManager upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GameObject.FindObjectOfType<UpgradeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpgradeConfiguration configuration = upgradeManager.upgradeConfigurations.Find(x => x.equipmentConfiguration == upgradeOption.configuration);
        if (configuration != null && configuration.allowances != null && upgradeOption != null) {
            title.text = configuration.allowances.Find(x => x.type == upgradeOption.upgradeType).displayName;
        } else {
            title.text = "NULL";
        }
    }
}