using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    public EquipmentConfiguration configuration = null;

    public int ChargeCapacity
    {
        get
        {
            return configuration.baseMaxCharges;
        }
    }
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
            return _selectedSlotIndex >= 0 ? slots[_selectedSlotIndex] : null;
        }
    }
    public List<InventorySlot> slots;

    private PubSubSender _pubSub;

    public GameObject PlacementRangeIndicator;
    [Range(0, 10.0f)]
    public float PlacementRange = 5.0f;
    public Material BlueprintValidPlacementMat;
    public Material BlueprintInvalidPlacementMat;

    [Range(0, 20.0f)]
    public float ThrowRange = 10.0f;
    public GameObject TargetingReticlePrefab;

    private EquipmentBlueprint _deploymentBlueprint;
    private GameObject _targetingReticle;

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

    private void Start()
    {

    }

    private void SetSelectedSlotIndex(int index)
    {
        if (index < 0 || index >= InventoryCapacity)
        {
            _selectedSlotIndex = -1;
        }
        else
        {
            if (index == _selectedSlotIndex)
            {
                _selectedSlotIndex = -1;
            }
            else
            {
                _selectedSlotIndex = index;
                if (SelectedSlot == null)
                {
                    _selectedSlotIndex = -1;
                }
            }
        }

        ResetTracking();

        _pubSub.Publish("InventoryChanged");
    }

    private void ResetTracking()
    {
        if (_deploymentBlueprint != null)
        {
            Destroy(_deploymentBlueprint.gameObject);
            PlacementRangeIndicator.SetActive(false);
        }

        if (_targetingReticle != null)
        {
            Destroy(_targetingReticle);
        }

        if (SelectedSlot != null &&
            SelectedSlot.configuration != null &&
            SelectedSlot.configuration.type == EquipmentConfiguration.EquipmentType.Deployable)
        {
            PlacementRangeIndicator.SetActive(true);
            PlacementRangeIndicator.transform.localScale = new Vector3(PlacementRange * 2, 0, PlacementRange * 2);

            var go = Instantiate(SelectedSlot.configuration.blueprintPrefab);
            _deploymentBlueprint = go.AddComponent<EquipmentBlueprint>();
            _deploymentBlueprint.BlueprintValidPlacementMat = BlueprintValidPlacementMat;
            _deploymentBlueprint.BlueprintInvalidPlacementMat = BlueprintInvalidPlacementMat;
            _deploymentBlueprint.PlacementAnchor = gameObject;
            _deploymentBlueprint.PlacementRange = PlacementRange;
        }

        if (SelectedSlot != null &&
            SelectedSlot.configuration != null &&
            SelectedSlot.configuration.type == EquipmentConfiguration.EquipmentType.Throwable)
        {
            _targetingReticle = Instantiate(TargetingReticlePrefab);
        }
    }

    private bool CanPollUserInput
    {
        get
        {
            return Time.timeScale > 0.01;
        }
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

        if (Input.GetMouseButtonDown(1))
        {
            SetSelectedSlotIndex(-1);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                var newIndex = _selectedSlotIndex + 1;
                if (newIndex >= InventoryCapacity) { newIndex = 0; }
                SetSelectedSlotIndex(newIndex);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                var newIndex = _selectedSlotIndex - 1;
                if (newIndex < 0) { newIndex = InventoryCapacity - 1; }
                SetSelectedSlotIndex(newIndex);
            }
        }

        if (CanPollUserInput && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DeployEquipment();
        }

        UpdateTracking();
    }

    private void UpdateTracking()
    {
        GameObject trackingCursor = null;

        if (_deploymentBlueprint != null) { trackingCursor = _deploymentBlueprint.gameObject; }
        if (_targetingReticle != null) { trackingCursor = _targetingReticle; }

        if (trackingCursor != null)
        {
            var plane = new Plane(Vector3.up, new Vector3(0.0f, 0.0f, 0.0f));
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float distance))
            {
                var hitPoint = ray.GetPoint(distance);

                trackingCursor.transform.position = hitPoint;

                if (_targetingReticle != null && Vector3.Distance(trackingCursor.transform.position, transform.position) > ThrowRange)
                {
                    var direction = (trackingCursor.transform.position - transform.position).normalized;
                    var offset = direction * ThrowRange;
                    trackingCursor.transform.position = transform.position + offset;
                }
            }
        }
    }

    private void DeployEquipment()
    {
        InventorySlot selectedSlot = SelectedSlot;
        if (selectedSlot == null)
        {
            return;
        }

        EquipmentConfiguration equipmentConfig = selectedSlot.configuration;
        if (equipmentConfig == null)
        {
            return;
        }

        // lol tightly coupled
        var headingGO = transform.Find("Inner");
        var placementLocation = transform.position + (headingGO.forward * 1);
        if (equipmentConfig.type == EquipmentConfiguration.EquipmentType.Deployable)
        {
            if (_deploymentBlueprint == null || _deploymentBlueprint.IsValidPlacement == false)
            {
                return;
            }
            else
            {
                placementLocation = _deploymentBlueprint.transform.position;
            }
        }

        var deployedEquipment = Instantiate(equipmentConfig.deployedPrefab);

        if (equipmentConfig.type == EquipmentConfiguration.EquipmentType.Throwable)
        {
            var throwableTarget = deployedEquipment.GetComponent<ThrowableTarget>();

            if (throwableTarget != null)
            {
                float maxParabolaDuration = 0.60f;
                float minParabolaDuration = 0.25f;
                float durationRange = maxParabolaDuration - minParabolaDuration;

                float maxParabolaHeight = 3.0f;
                float minParabolaHeight = 1.0f;
                float heightRange = maxParabolaHeight - minParabolaHeight;

                var distance = Vector3.Distance(transform.position, _targetingReticle.transform.position);

                throwableTarget.target = _targetingReticle;
                throwableTarget.ParabolaDuration = (minParabolaDuration + (distance / ThrowRange) * durationRange);
                throwableTarget.ParabolaHeight = (minParabolaHeight + (distance / ThrowRange) * heightRange);
            }
        }

        deployedEquipment.transform.position = placementLocation;
        selectedSlot.currentCharges -= 1;
        if (selectedSlot.currentCharges == 0)
        {
            selectedSlot.configuration = null;
        }

        ResetTracking();

        _pubSub.Publish("InventoryChanged");
        AudioManager.Instance.Play("SFX/EquipmentPlace");
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

        AudioManager.Instance.Play("SFX/EquipmentPickup");

        ResetTracking();

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
