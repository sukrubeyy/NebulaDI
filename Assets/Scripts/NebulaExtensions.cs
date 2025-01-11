using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NebulaExtentions : MonoBehaviour
{
    static HashSet<int> InjectedObjects = new HashSet<int>();

    public static void FindInjectAttributesInScene(NebulaContainer Container)
    {
        Debug.Log("Finding Inject Attributes in Scene...");

        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects().ToList();

        foreach (var item in GetDontDestroyOnLoadObjects())
        {
            rootObjects.Add(item);
        }

        foreach (var root in rootObjects)
        {
            // var components = root.GetComponentsInChildren<MonoBehaviour>(true);

            var componentsWithInject = root.GetComponentsInChildren<MonoBehaviour>(true)
            .Where(component => HasInjectAttribute(component))
            .ToList();

            foreach (var component in componentsWithInject)
            {
                if (IsInjected(component))
                {
                    Debug.LogWarning($"This Object Injected... Name : {component.name} -- ID : {component.GetInstanceID()}");
                    continue;
                }
                var type = component.GetType();

                var fieldsWithInject = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)));

                foreach (var field in fieldsWithInject)
                {
                    var dependency = ResolveDependency(field.FieldType, Container);
                    field.SetValue(component, dependency);
                }

                var propertiesWithInject = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => Attribute.IsDefined(prop, typeof(InjectAttribute)));

                foreach (var property in propertiesWithInject)
                {
                    var dependency = ResolveDependency(property.PropertyType, Container);
                    property.SetValue(component, dependency);
                }

                var methodsInject = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(method => Attribute.IsDefined(method, typeof(InjectAttribute)));

                foreach (var method in methodsInject)
                {
                    var parameters = method.GetParameters()
                        .Select(param => Container.GetService(param.ParameterType))
                        .ToArray();

                    method.Invoke(component, parameters);
                }
            }
        }
    }

    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            DontDestroyOnLoad(temp);
            Scene dontDestroyOnLoad = temp.scene;
            DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                DestroyImmediate(temp);
        }
    }

    private static object ResolveDependency(Type type, NebulaContainer Container)
    {
        if (typeof(MonoBehaviour).IsAssignableFrom(type))
        {
            return Container.GetService(type);
        }

        var constructor = type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (constructor != null)
        {
            var parameters = constructor.GetParameters()
                .Select(param => Container.GetService(param.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters);
        }

        return Container.GetService(type);
    }


    private static bool HasInjectAttribute(MonoBehaviour component)
    {
        var type = component.GetType();
        return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                   .Any(field => Attribute.IsDefined(field, typeof(InjectAttribute)))
               || type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                   .Any(prop => Attribute.IsDefined(prop, typeof(InjectAttribute)))
               || type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                   .Any(method => Attribute.IsDefined(method, typeof(InjectAttribute)));
    }


    static bool IsInjected(MonoBehaviour monoBehaviour)
    {
        int instanceId = monoBehaviour.GetInstanceID();
        if (InjectedObjects.Contains(instanceId))
            return true;

        InjectedObjects.Add(instanceId);
        return false;
    }
}