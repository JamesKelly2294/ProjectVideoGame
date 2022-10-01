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
    private int _selectedSlotIndex = 0;
    public InventorySlot SelectedSlot
    {
        get
        {
            return slots[_selectedSlotIndex];
        }
    }
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
    void Awake()
    {
        _pubSub = GetComponent<PubSubSender>();

        slots = new List<InventorySlot>(InventoryCapacity);
        for(var i = 0; i < InventoryCapacity; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    private void SetSelectedSlotIndex(int index)
    {
        if (index < 0 || index >= InventoryCapacity)
        {
            return;
        }

        _selectedSlotIndex = index;
        _pubSub.Publish("InventoryChanged");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SetSelectedSlotIndex(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { SetSelectedSlotIndex(1); }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) { SetSelectedSlotIndex(2); }
        else if(Input.GetKeyDown(KeyCode.Alpha4)) { SetSelectedSlotIndex(3); }
        else if(Input.GetKeyDown(KeyCode.Alpha5)) { SetSelectedSlotIndex(4); }
        else if(Input.GetKeyDown(KeyCode.Alpha6)) { SetSelectedSlotIndex(5); }
        else if(Input.GetKeyDown(KeyCode.Alpha7)) { SetSelectedSlotIndex(6); }
        else if(Input.GetKeyDown(KeyCode.Alpha8)) { SetSelectedSlotIndex(7); }
        else if(Input.GetKeyDown(KeyCode.Alpha9)) { SetSelectedSlotIndex(8); }
    }

    private InventorySlot SlotForEquipment(EquipmentConfiguration configuration)
    {
        InventorySlot firstAvailableEmptySlot = null;
        foreach(InventorySlot slot in slots)
        {
            if (slot.configuration == configuration)
            {
                if (slot.currentCharges < slot.configuration.baseMaxCharges)
                {
                    return slot;
                }
                else
                {
                    return null;
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

        Destroy(pickup.gameObject);
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
