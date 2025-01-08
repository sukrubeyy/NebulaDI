using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NebulaContainer
{
    private List<ServiceDescriptor> services;
    GameObject containerTransform;
    public NebulaContainer(List<ServiceDescriptor> services, GameObject _containerTransform)
    {
        this.services = services;
        containerTransform = _containerTransform;
    }

    internal T GetService<T>()
    {
        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(T));

        if (descriptor == null)
            Debug.Log($"Services of type {typeof(T).Name} is not registered");

        if (descriptor.Implementetion != null)
            return (T)descriptor.Implementetion;

        if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
        {
            Debug.Log($"Creating MonoBehaviour instance for {typeof(T).Name}");
            var component = containerTransform.AddComponent(typeof(T));
            if (descriptor.LifeTime is ServiceLifeType.Singleton)
                descriptor.Implementetion = component;
            return (T)(object)component;
        }

        var implementation = (T)Activator.CreateInstance(descriptor.ImplementationType ?? descriptor.ServiceType);

        if (descriptor.LifeTime is ServiceLifeType.Singleton)
            descriptor.Implementetion = implementation;


        return implementation;
    }
}
