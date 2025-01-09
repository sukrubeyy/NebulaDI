using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Inject]
    public void Initialize(RandomGuidGenerater randomGuidGenerater)
    {
        Debug.Log($"{randomGuidGenerater.RandomGuid}");
    }
}
