using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryGUI : MonoBehaviour
{
    public GameObject slots;

    public GameObject inventorySlotPrefab;

    private PlayerInventory _playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();

        InventoryChanged();

    }

    public void InventoryChanged()
    {
        for (var i = 0; i < slots.transform.childCount; i++)
        {
            Destroy(slots.transform.GetChild(i).gameObject);
        }

        for (var i = 0; i < _playerInventory.InventoryCapacity; i++)
        {
            var inventorySlot = _playerInventory.slots[i];

            var go = Instantiate(inventorySlotPrefab);
            go.transform.SetParent(slots.transform);
            go.transform.name = "Slot " + i;
            go.SetActive(true);

            var slotGUI = go.GetComponent<PlayerInventorySlotGUI>();

            if(inventorySlot.configuration == null)
            {
                slotGUI.equipmentIcon.enabled = false;
                slotGUI.equipmentIcon = null;
                slotGUI.equipmentCountText.text = "";
                slotGUI.equipmentSelection.enabled = _playerInventory.SelectedSlot == inventorySlot;
            }
            else
            {
                slotGUI.equipmentIcon.enabled = true;
                slotGUI.equipmentIcon.sprite = inventorySlot.configuration.inventoryIcon;
                slotGUI.equipmentCountText.text = inventorySlot.currentCharges + "/" + inventorySlot.configuration.baseMaxCharges;
                slotGUI.equipmentSelection.enabled = _playerInventory.SelectedSlot == inventorySlot;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
