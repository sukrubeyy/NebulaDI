using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class NebulaInstaller : MonoBehaviour
{
    NebulaServiceCollection serviceCollection = new NebulaServiceCollection();
    NebulaContainer Container;
    public GameObject containerTransform;

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
    void Start()
    {
        serviceCollection.RegisterSingleton<RandomGuidGenerater>();
        CheckReferanceExists();
        CreateContainer();

        // var s1 = Container.GetService<RandomGuidGenerater>();
        // var s2 = Container.GetService<RandomGuidGenerater>();
        // Debug.Log($"{s1.RandomGuid}");
        // Debug.Log($"{s2.RandomGuid}");


        FindInjectAttributesInAssembly();
    }

    void CreateContainer()
    {
        Container = serviceCollection.GenerateContainer(containerTransform);
    }

    void CheckReferanceExists()
    {
        if (containerTransform == null)
        {
            containerTransform = transform.GetChild(0).gameObject;
        }

        if (Container == null)
            CreateContainer();
    }

    public void FindInjectAttributesInAssembly()
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        var types = currentAssembly.GetTypes();

        foreach (var type in types)
        {
            var fieldsWithInject = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)));

            var propertiesWithInject = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(InjectAttribute)));

            if (fieldsWithInject.Any() || propertiesWithInject.Any())
            {
                Debug.Log($"Type: {type.Name}");

                var instance = FindObjectOfType(type) as MonoBehaviour ?? Activator.CreateInstance(type);

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
                    Debug.Log($"  Property: {property.Name}, Type: {property.PropertyType}");
                }
            }
        }
    }

}
