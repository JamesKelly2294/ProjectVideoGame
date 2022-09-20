using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the state of the game world, and all of its resources.
// We pass around world instances instead of a singleton so we can simulate
// multiple worlds simultaneously (which might be useful for idle games).
public class World : MonoBehaviour
{
    public ResourceStore resourceStore = new ResourceStore();

    public World()
    {
        resourceStore.RegisterResource(new Resource(ResourceKeys.Silica, "Silica", 100));
        resourceStore.RegisterResource(new Resource(ResourceKeys.Iron, "Iron", 0));
        resourceStore.RegisterResource(new Resource(ResourceKeys.Platinum, "Platinum", 0));
    }
}
