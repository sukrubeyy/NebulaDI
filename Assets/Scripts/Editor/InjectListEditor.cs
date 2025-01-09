using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class InjectListEditor : EditorWindow
{
    private static InjectListEditor Window;

    private List<FieldInfo> fieldsList = new();
    private List<PropertyInfo> propertiesList = new();
    private List<MethodInfo> methodsList = new();
    private List<ConstructorInfo> constructorsList = new();

    private ShowInjectType showInjectType = ShowInjectType.All;
    private Vector2 scrollPosition;

    [MenuItem("Nebula/Open Inject List Window")]
    public static void OpenWindow()
    {
        Window = GetWindow<InjectListEditor>("Inject List");
    }

    private void OnEnable()
    {
        PrepareAssemblyData();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.LabelField("Inject List Editor", EditorStyles.boldLabel);

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Filter by Inject Type:", GUILayout.Width(150));
                showInjectType = (ShowInjectType)EditorGUILayout.EnumPopup(showInjectType);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            DrawHorizontalLine();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                switch (showInjectType)
                {
                    case ShowInjectType.All:
                        DrawConstructors();
                        DrawMethods();
                        DrawFields();
                        DrawProperties();
                        break;
                    case ShowInjectType.Fields:
                        DrawFields();
                        break;
                    case ShowInjectType.Properties:
                        DrawProperties();
                        break;
                    case ShowInjectType.Methods:
                        DrawMethods();
                        break;
                    case ShowInjectType.Constructor:
                        DrawConstructors();
                        break;
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    public void PrepareAssemblyData()
    {
        Assembly currentAssembly = typeof(InjectAttribute).Assembly;
        var types = currentAssembly.GetTypes();

        foreach (var type in types)
        {
            fieldsList.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute))));

            propertiesList.AddRange(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(InjectAttribute))));

            methodsList.AddRange(type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(method => Attribute.IsDefined(method, typeof(InjectAttribute))));

            constructorsList.AddRange(type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(c => Attribute.IsDefined(c, typeof(InjectAttribute))));
        }
    }

    void DrawConstructors()
    {
        DrawSection("Constructors", constructorsList.Count, () =>
        {
            foreach (var item in constructorsList)
            {
                DrawItem($"Constructor of {item.DeclaringType.Name}");
            }
        });
    }

    void DrawMethods()
    {
        DrawSection("Methods", methodsList.Count, () =>
        {
            foreach (var item in methodsList)
            {
                DrawItem($"Method: {item.Name} ({item.DeclaringType.Name})");
            }
        });
    }

    void DrawFields()
    {
        DrawSection("Fields", fieldsList.Count, () =>
        {
            foreach (var item in fieldsList)
            {
                DrawItem($"Field: {item.Name} ({item.DeclaringType.Name})");
            }
        });
    }

    void DrawProperties()
    {
        DrawSection("Properties", propertiesList.Count, () =>
        {
            foreach (var item in propertiesList)
            {
                DrawItem($"Property: {item.Name} ({item.DeclaringType.Name})");
            }
        });
    }

    void DrawSection(string title, int count, Action drawContent)
    {
        EditorGUILayout.LabelField($"{title} ({count})", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        {
            drawContent();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
    }

    void DrawItem(string content)
    {
        EditorGUILayout.BeginHorizontal("box");
        {
            EditorGUILayout.LabelField(content, EditorStyles.label);
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawHorizontalLine(float thickness = 1f)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, thickness);
        EditorGUI.DrawRect(rect, new Color(0.6f, 0.6f, 0.6f, 1));
    }
}

public enum ShowInjectType
{
    All,
    Fields,
    Properties,
    Methods,
    Constructor
}
