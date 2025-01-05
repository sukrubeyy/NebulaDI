using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NebulaContainer
{
    private List<ServiceDescriptor> services;

    public NebulaContainer(List<ServiceDescriptor> services)
    {
        this.services = services;
    }

    internal T GetService<T>()
    {
        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(T));

        if (descriptor == null)
            Debug.Log($"Services of type {typeof(T).Name} is not registered");

        // if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
        // {
        //     Debug.LogWarning($"MONOBEHAVIOUR CLASS VAR");
        // }

        if (descriptor.Implementetion != null)
            return (T)descriptor.Implementetion;

        var implementation = (T)Activator.CreateInstance(descriptor.ImplementationType ?? descriptor.ServiceType);

        if (descriptor.LifeTime is ServiceLifeType.Singleton)
            descriptor.Implementetion = implementation;


        return implementation;
    }

    private T CreateMonoBehaviourInstance<T>(ServiceDescriptor descriptor) where T : MonoBehaviour
    {
        var parentObject = descriptor.Implementetion as GameObject;

        if (parentObject == null)
        {
            Debug.LogError("Parent GameObject is not provided for transient MonoBehaviour.");
            return null;
        }

        return parentObject.AddComponent<T>();
    }
}
