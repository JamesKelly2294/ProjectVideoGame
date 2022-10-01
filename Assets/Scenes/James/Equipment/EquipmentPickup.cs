using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickup : MonoBehaviour
{
    public EquipmentConfiguration configuration;

    // Start is called before the first frame update
    void Start()
    {
        GameObject pickupPrefab = Instantiate(configuration.pickupPrefab);
        pickupPrefab.transform.localPosition = transform.position;
        pickupPrefab.transform.parent = transform;
        pickupPrefab.transform.name = configuration.displayName + " Pickup Visuals";
        transform.name = configuration.displayName + " Pickup";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
