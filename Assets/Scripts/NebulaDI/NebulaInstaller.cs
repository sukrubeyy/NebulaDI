using UnityEngine;

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
        serviceCollection.RegisterTransient<GameManager>(containerTransform);

        serviceCollection.RegisterTransient<ISomeThing, DoSomething>();
        CheckReferanceExists();

        CreateContainer();

        var p1 = Container.GetService<ISomeThing>();

        var manager1 = Container.GetService<GameManager>();
        // var manager2 = Container.GetService<GameManager>();
        Debug.Log($"{manager1.userId}");
        // Debug.Log($"{manager2.userId}");
    }

    void CreateContainer()
    {
        Container = serviceCollection.GenerateContainer();
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
}
