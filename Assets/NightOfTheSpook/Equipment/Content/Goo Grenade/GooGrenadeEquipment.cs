using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - Look into using physics and detecting collisions, rather than just exploding when parabola is done
[RequireComponent(typeof(ThrowableTarget))]
public class GooGrenadeEquipment : Equipment
{
    public ThrowableTarget target;

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<ThrowableTarget>();
    }

    private void Update()
    {
        if (target.ThrowLanded) {
            Destroy(gameObject);
        }
    }
}
