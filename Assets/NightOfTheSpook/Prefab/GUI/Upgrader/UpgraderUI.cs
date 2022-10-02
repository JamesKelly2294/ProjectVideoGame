using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgraderUI : MonoBehaviour
{

    public UpgradeButton first, second, third;
    private UpgradeManager upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GameObject.FindObjectOfType<UpgradeManager>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void PickNewUpgrades() {
        List<UpgradeOption> options = upgradeManager.GetRandomOptions(3);
        for (int i = 0; i < options.Count; i++) {
            if (i == 0) {           first.upgradeOption  = options[i]; 
            } else if (i == 1) {    second.upgradeOption = options[i]; 
            } else {                third.upgradeOption  = options[i]; }
        }
    }
}
