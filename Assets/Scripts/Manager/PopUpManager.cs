using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [Inject]
    [SerializeField] private RandomGuidGenerater randomGuidGenerater;

    void Start()
    {
        Debug.Log(randomGuidGenerater.RandomGuid);
    }
}
