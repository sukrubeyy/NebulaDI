using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Inject]
    public RandomGuidGenerater r;
    [Inject]
    public RandomGuidGenerater MyProperty { get; set; }
    [Inject]
    public void Initialize(RandomGuidGenerater randomGuidGenerater)
    {
        Debug.Log($"{randomGuidGenerater.RandomGuid}");
    }
}
