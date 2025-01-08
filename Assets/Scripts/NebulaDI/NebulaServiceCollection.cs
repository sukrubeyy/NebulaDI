using System.Collections.Generic;
using UnityEngine;

public class NebulaServiceCollection
{
    private readonly List<ServiceDescriptor> _services = new List<ServiceDescriptor>();
    public NebulaContainer GenerateContainer(GameObject containerObjTransform)
    {
        var container = new NebulaContainer(_services, containerObjTransform);
        return container;
    }

    #region Singleton
    internal void RegisterSingleton<TService>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), ServiceLifeType.Singleton));
    }
    public void RegisterSingleton<TService>(TService implementation)
    {
        _services.Add(new ServiceDescriptor(implementation, ServiceLifeType.Singleton));
    }
    #endregion

    #region Transient

    internal void RegisterTransient<TService>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), ServiceLifeType.Transient));
    }

    internal void RegisterTransient<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifeType.Transient));
    }
    #endregion
}

public enum ServiceLifeType
{
    Singleton,
    Transient
}
