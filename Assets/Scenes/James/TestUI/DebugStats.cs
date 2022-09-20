using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugStats : MonoBehaviour
{
    public World world;
    public TextMeshProUGUI resourcesLabel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var str = "";
        var resourceStore = world.resourceStore;
        foreach (var r in resourceStore.Resources)
        {
            var amountString = "";

            if (r.Amount < 1000000)
            {
                amountString = r.Amount.ToString("G");
            }
            else
            {
                amountString = r.Amount.ToString("E3");
            }

            str += r.key.name + " : " + amountString + "\n";
        }
        resourcesLabel.text = str;
    }
}
