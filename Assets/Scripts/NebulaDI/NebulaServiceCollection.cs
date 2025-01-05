using System.Collections.Generic;
using UnityEngine;

public class NebulaServiceCollection
{
    private readonly List<ServiceDescriptor> _services = new List<ServiceDescriptor>();
    public NebulaContainer GenerateContainer()
    {
        var container = new NebulaContainer(_services);
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

    internal void RegisterSingleton<TService>(GameObject gameObject) where TService : MonoBehaviour
    {
        var component = gameObject.AddComponent<TService>();
        _services.Add(new ServiceDescriptor(component, ServiceLifeType.Singleton));
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

    internal void RegisterTransient<TService>(GameObject parent) where TService : MonoBehaviour
    {
        _services.Add(new ServiceDescriptor(typeof(TService), ServiceLifeType.Transient)
        {
            Implementetion = parent
        });
    }
    #endregion

}

public enum ServiceLifeType
{
    Singleton,
    Transient
}
