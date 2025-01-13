using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NebulaContainer
{
    private List<ServiceDescriptor> services;
    Transform ContainerObj;
    public NebulaContainer(List<ServiceDescriptor> services, Transform _containerObj)
    {
        this.services = services;
        ContainerObj = _containerObj;
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

        var actualType = descriptor.ImplementationType ?? descriptor.ServiceType;

        if (actualType.IsInterface)
        {
            throw new Exception($"You cannot create interface. {actualType.Name}");
        }

        if (actualType.IsAbstract)
        {
            throw new Exception($"You cannot create abstract class. {actualType.Name}");
        }

        if (descriptor.Implementetion != null)
            return descriptor.Implementetion;

        if (typeof(MonoBehaviour).IsAssignableFrom(serviceType))
        {
            Debug.Log($"Creating MonoBehaviour instance for {serviceType.Name}");
            var component = ContainerObj.gameObject.AddComponent(serviceType);
            if (descriptor.LifeTime == ServiceLifeType.Singleton)
                descriptor.Implementetion = component;
            return component;
        }

        if (typeof(ScriptableObject).IsAssignableFrom(serviceType))
        {
            Debug.Log($"Creating ScriptableObject instance for {serviceType.Name}");
            var method = typeof(NebulaExtentions).GetMethod("LoadScriptableObject");
            var genericMethod = method.MakeGenericMethod(serviceType);
            var component = genericMethod.Invoke(null, null) as ScriptableObject;

            if (component == null)
                Debug.LogWarning($"{serviceType} is not found");

            if (descriptor.LifeTime == ServiceLifeType.Singleton)
                descriptor.Implementetion = component;
            return component;
        }

        var implementation = Activator.CreateInstance(actualType);

        if (descriptor.LifeTime == ServiceLifeType.Singleton)
            descriptor.Implementetion = implementation;

        return implementation;
    }

}
