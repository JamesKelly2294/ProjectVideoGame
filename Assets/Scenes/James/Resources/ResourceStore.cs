using BreakInfinity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceKeys
{
    public static Resource.Key Silica = new Resource.Key("silica");
    public static Resource.Key Iron = new Resource.Key("iron");
    public static Resource.Key Platinum = new Resource.Key("platinum");
}

public class Resource
{
    public struct Key
    {
        public string name;

        public Key(string name)
        {
            this.name = name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("<ResourceKey {0}>", name);
        }
    }

    public Key key;
    public string name;
    public BigDouble Amount { get; protected set; }

    public void ApplyClickProduction()
    {
        Amount += ProductionPerClick;
    }

    public BigDouble ProductionPerClick
    {
        get { return 1; }
    }

    public BigDouble ProductionPerSecond
    {
        get { return 0; }
    }

    public Resource(Key key, string name, BigDouble startingAmount)
    {
        this.key = key;
        this.name = name;
    }

    public override string ToString()
    {
        return string.Format("<Resource {0} ({1})>", key.name, name);
    }
}

public class ResourceStore
{
    Dictionary<Resource.Key, Resource> _resources = new Dictionary<Resource.Key, Resource>();

    public void RegisterResource(Resource resource)
    {
        RegisterResource(resource, new BigDouble(0.0));
    }

    public void RegisterResource(Resource resource, BigDouble startingAmount)
    {
        if (_resources.ContainsKey(resource.key))
        {
            Debug.LogError("Unable to register a resource that is already registered!");
            return;
        }

        _resources[resource.key] = resource;
    }

    public List<Resource> Resources
    {
        get
        {
            return new List<Resource>(_resources.Values);
        }
    }

    public Resource GetResource(string resourceKey)
    {
        return _resources[new Resource.Key(resourceKey)];
    }

    public Resource GetResource(Resource.Key resource)
    {
        return _resources[resource];
    }

    public BigDouble GetResourceAmount(Resource.Key resource)
    {
        return _resources[resource].Amount;
    }
}
