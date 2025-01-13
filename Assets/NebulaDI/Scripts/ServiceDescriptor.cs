using System;
public class ServiceDescriptor
{
    public Type ServiceType { get; set; }
    public Type ImplementationType { get; set; }
    public object Implementetion { get; set; }
    public ServiceLifeType LifeTime { get; set; }
    public ServiceDescriptor(object implementation, ServiceLifeType _lifeType)
    {
        ServiceType = implementation.GetType();
        Implementetion = implementation;
        LifeTime = _lifeType;
    }

    public ServiceDescriptor(Type _serviceType, ServiceLifeType _lifeType)
    {
        ServiceType = _serviceType;
        LifeTime = _lifeType;
    }

    public ServiceDescriptor(Type _serviceType, Type _implementationType, ServiceLifeType _lifeType)
    {
        ServiceType = _serviceType;
        ImplementationType = _implementationType;
        LifeTime = _lifeType;
    }
}
