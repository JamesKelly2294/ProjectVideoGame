using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies offscreen.
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    /// <summary>
    /// Locations in the current scene where enemies can spawn
    /// </summary>
    public Transform[] ViableSpawnLocations;

    /// <summary>
    /// The camera to use for scene visability purposes.
    /// </summary>
    public Camera PlayerCamera;

    /// <summary>
    /// The attackable entity the spawn manager will preferentially hit?
    /// </summary>
    public Attackable EnemyTarget;

    //public IEnumerable OffscreenSpawnLocations
    //{
    //    get
    //    {
    //        // todo
    //        return null;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
