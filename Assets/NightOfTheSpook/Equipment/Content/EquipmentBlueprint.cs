using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBlueprint : MonoBehaviour
{
    public Material BlueprintValidPlacementMat;
    public Material BlueprintInvalidPlacementMat;

    public float PlacementRange;
    public GameObject PlacementAnchor; // must place be within placement range from this anchor for valid placement

    private int _collisions;

    private void UpdateVisuals()
    {
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = IsValidPlacement ? BlueprintValidPlacementMat : BlueprintInvalidPlacementMat;
        }
    }

    public bool IsValidPlacement
    {
        get
        {
            var isColliding = _collisions > 0;
            var isTooFar = Vector3.Distance(transform.position, PlacementAnchor.transform.position) > PlacementRange;

            return !isColliding && !isTooFar;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _collisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        _collisions--;
    }

    public void LateUpdate()
    {
        UpdateVisuals();
    }
}
