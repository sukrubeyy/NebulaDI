using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        return (T)GetService(typeof(T));
    }


    public object GetService(Type serviceType)
    {
        var descriptor = services.SingleOrDefault(x => x.ServiceType == serviceType);

        if (descriptor == null)
            throw new Exception($"Service of type {serviceType.Name} is not registered");

        if (descriptor.Implementetion != null)
            return descriptor.Implementetion;

        if (typeof(MonoBehaviour).IsAssignableFrom(serviceType))
        {
            Debug.Log($"Creating MonoBehaviour instance for {serviceType.Name}");
            var component = containerTransform.AddComponent(serviceType);
            if (descriptor.LifeTime == ServiceLifeType.Singleton)
                descriptor.Implementetion = component;
            return component;
        }

        var implementation = Activator.CreateInstance(descriptor.ImplementationType ?? descriptor.ServiceType);

        if (descriptor.LifeTime == ServiceLifeType.Singleton)
            descriptor.Implementetion = implementation;

        return implementation;
    }

}
