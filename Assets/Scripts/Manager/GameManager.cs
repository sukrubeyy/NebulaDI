using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Inject]
    public RandomGuidGenerater randomGuidGenerater;

    void Start()
    {
        Debug.Log(randomGuidGenerater.RandomGuid);
    }
}
