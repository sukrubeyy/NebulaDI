using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [Inject]
    private RandomGuidGenerater randomGuidGenerater;

    void Start()
    {
        Debug.Log(randomGuidGenerater.RandomGuid);
    }
}
