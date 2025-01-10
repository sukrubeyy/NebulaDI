using UnityEngine;

public class PopUpManager
{
    [Inject]
    public RandomGuidGenerater r;
    [Inject]
    public PopUpManager(RandomGuidGenerater _ra)
    {
        Debug.Log($"{_ra.RandomGuid}");
    }

    public PopUpManager()
    {

    }
}
