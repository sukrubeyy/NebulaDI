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
        serviceCollection.RegisterSingleton<GameManager>();

        serviceCollection.RegisterSingleton<RandomGuidGenerater>();

        CheckReferanceExists();
        CreateContainer();
        var manager1 = Container.GetService<GameManager>();
        var manager2 = Container.GetService<GameManager>();
        Debug.Log($"{manager1.userId}");
        Debug.Log($"{manager2.userId}");

        Debug.Log($"--------------NONE MONOBEHAVIOUR--------------");

        var uid1 = Container.GetService<RandomGuidGenerater>();
        Debug.Log($"{uid1.RandomGuid}");
        var uid2 = Container.GetService<RandomGuidGenerater>();
        Debug.Log($"{uid2.RandomGuid}");
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
}
