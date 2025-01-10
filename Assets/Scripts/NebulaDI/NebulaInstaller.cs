using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class NebulaInstaller : MonoBehaviour
{
    protected NebulaServiceCollection Servises = new NebulaServiceCollection();
    protected NebulaContainer Container;
    public GameObject containerTransform;
    List<int> InjectedIds = new List<int>();

    #region Editor
    void OnValidate()
    {
        if (transform.Find("Container") == null)
        {
            containerTransform = new GameObject("Container");
            containerTransform.transform.SetParent(this.transform);
            containerTransform.transform.localPosition = Vector3.zero;
            Debug.Log("NebulaInstaller eklendi: Container child objesi oluşturuldu.");
        }

        CheckReferanceExists();
    }
    void CheckReferanceExists()
    {
        if (containerTransform == null)
            containerTransform = transform.GetChild(0).gameObject;

        if (Container == null)
            CreateContainer();
    }
    #endregion

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    void Start()
    {
        CreateContainer();

        OverrideBindings();

        FindInjectAttributesInAssembly();
    }

    public virtual void OverrideBindings() { }

    void CreateContainer()
    {
        Container = Servises.GenerateContainer(containerTransform);
    }

    public void FindInjectAttributesInAssembly()
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        var types = currentAssembly.GetTypes();

        foreach (var type in types)
        {

            if (IsInject(type.GetHashCode()))
            {
                Debug.Log($"{type.Name} Inject Edilmiş... ID:{type.GetHashCode()}");
                continue;
            }


            var fieldsWithInject = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)));

            var propertiesWithInject = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(InjectAttribute)));


            if (fieldsWithInject.Any() || propertiesWithInject.Any())
            {
                Debug.Log($"Type: {type.Name}");
                var instance = typeof(MonoBehaviour).IsAssignableFrom(type) ? FindObjectOfType(type) : Activator.CreateInstance(type);

                if (instance == null)
                {
                    Debug.LogWarning($"Type {type.Name} için uygun bir instance bulunamadı.");
                    continue;
                }

                foreach (var field in fieldsWithInject)
                {
                    var dependency = Container.GetService(field.FieldType);
                    field.SetValue(instance, dependency);
                    Debug.Log($"  Field: {field.Name}, Type: {field.FieldType}");
                }

                foreach (var property in propertiesWithInject)
                {
                    var dependency = Container.GetService(property.PropertyType);
                    property.SetValue(instance, dependency);
                    Debug.Log($"  Property: {property.Name}, Type: {property.PropertyType}");
                }
            }



            var methodsInject = type.GetMethods().Where(method => Attribute.IsDefined(method, typeof(InjectAttribute)));

            if (methodsInject.Any())
            {
                foreach (var method in methodsInject)
                {
                    var instance = typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType) ? FindObjectOfType(method.DeclaringType) : Activator.CreateInstance(method.DeclaringType);

                    var parameters = method.GetParameters()
                                        .Select(x => Container.GetService(x.ParameterType))
                                        .ToArray();
                    method.Invoke(instance, parameters);
                }
            }



            var contructorsInject = type.GetConstructors().Where(c => Attribute.IsDefined(c, typeof(InjectAttribute)));
            if (contructorsInject.Any())
            {
                foreach (var constructor in contructorsInject)
                {


                    if (typeof(MonoBehaviour).IsAssignableFrom(constructor.DeclaringType))
                        continue;

                    var parameters = constructor.GetParameters()
               .Select(p => Container.GetService(p.ParameterType))
               .ToArray();


                    constructor.Invoke(parameters);
                }
            }
        }
    }

    bool IsInject(int _hasCode)
    {

        if (InjectedIds.Contains(_hasCode))
            return true;

        InjectedIds.Add(_hasCode);
        return false;
    }
}
