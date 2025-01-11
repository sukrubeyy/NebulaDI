using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Inject]
    public PopUpManager popUpManager;

    [Inject]
    public void Initialize(RandomGuidGenerater randomGuidGenerater)
    {
        Debug.Log($"{randomGuidGenerater.RandomGuid}");
        Debug.Log($"{gameObject.GetInstanceID()}");
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
