using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    public EquipmentConfiguration configuration = null;

    public int currentCharges = 0;
}

[RequireComponent(typeof(PubSubSender))]
public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> slots;
    private PubSubSender _pubSub;

    public int InventoryCapacity
    {
        get
        {
            return 3;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _pubSub = GetComponent<PubSubSender>();

        slots = new List<InventorySlot>(InventoryCapacity);
        for(var i = 0; i < InventoryCapacity; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private InventorySlot SlotForEquipment(EquipmentConfiguration configuration)
    {
        InventorySlot firstAvailableEmptySlot = null;
        foreach(InventorySlot slot in slots)
        {
            if (slot.configuration == configuration)
            {
                if (slot.configuration.baseMaxCharges >= slot.currentCharges)
                {
                    return null;
                }
                else
                {
                    return slot;
                }
            }
            if (firstAvailableEmptySlot == null && slot.configuration == null)
            {
                firstAvailableEmptySlot = slot;
            }
        }
        return firstAvailableEmptySlot;
    }

    private void PickUpEquipment(EquipmentPickup pickup)
    {
        var configuration = pickup.configuration;
        if (configuration == null) { return; }

        InventorySlot availableInventorySlot = SlotForEquipment(configuration);
        if (availableInventorySlot == null) { return; }

        availableInventorySlot.configuration = configuration;
        availableInventorySlot.currentCharges += 1;

        _pubSub.Publish("InventoryChanged");

        Destroy(pickup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerConstants.Pickup)
        {
            var equipmentPickup = other.gameObject.GetComponent<EquipmentPickup>();
            if (equipmentPickup == null) { return; }

            PickUpEquipment(equipmentPickup);
        }
    }
}
