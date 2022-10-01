using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryGUI : MonoBehaviour
{
    public GameObject slots;

    public GameObject inventorySlotPrefab;

    private PlayerInventory _playerInventory;

    private Color _deselectedSlotColor;
    public Color selectedSlotColor;

    private Color _filledChargeColor;
    public Color emptyChargeColor;

    // Start is called before the first frame update
    void Start()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _deselectedSlotColor = inventorySlotPrefab.GetComponent<PlayerInventorySlotGUI>().equipmentSelection.color;
        _filledChargeColor = inventorySlotPrefab.GetComponent<PlayerInventorySlotGUI>().chargeSlotPrefab.GetComponent<Image>().color;

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

            for (var j = 0; j < slotGUI.charges.transform.childCount; j++)
            {
                // very efficient omegalul
                Destroy(slotGUI.charges.transform.GetChild(j).gameObject);
            }

            if (inventorySlot.configuration == null)
            {
                slotGUI.equipmentIcon.enabled = false;
                slotGUI.equipmentIcon = null;
                slotGUI.equipmentSelection.color = _deselectedSlotColor;
            }
            else
            {
                slotGUI.equipmentIcon.enabled = true;
                slotGUI.equipmentIcon.sprite = inventorySlot.configuration.inventoryIcon;
                slotGUI.equipmentSelection.color = _playerInventory.SelectedSlot == inventorySlot ? selectedSlotColor : _deselectedSlotColor;

                for (var j = 0; j < inventorySlot.ChargeCapacity; j++)
                {
                    var chargeSlot = Instantiate(slotGUI.chargeSlotPrefab);
                    chargeSlot.transform.parent = slotGUI.charges.transform;
                    if (j >= inventorySlot.currentCharges)
                    {
                        chargeSlot.GetComponent<Image>().color = emptyChargeColor;
                    }
                }
            }

            go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
