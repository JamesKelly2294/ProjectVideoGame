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

        for (var i = 0; i < _playerInventory.InventoryCapacity; i++)
        {
            var go = Instantiate(inventorySlotPrefab);
            go.transform.SetParent(slots.transform);
            go.transform.name = "Slot " + i;
            go.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
