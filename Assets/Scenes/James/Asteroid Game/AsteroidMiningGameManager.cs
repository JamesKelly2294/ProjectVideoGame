using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(World))]
public class AsteroidMiningGameManager : MonoBehaviour
{
    private World _world;

    // Start is called before the first frame update
    void Start()
    {
        _world = GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
