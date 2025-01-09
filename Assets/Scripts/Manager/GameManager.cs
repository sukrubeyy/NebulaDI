using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Inject]
    public RandomGuidGenerater randomGuidGenerater { get; set; }

    void Start()
    {
        Debug.Log(randomGuidGenerater.RandomGuid);
    }
}
