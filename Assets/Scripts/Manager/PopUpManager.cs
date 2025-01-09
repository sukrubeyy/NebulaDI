using UnityEngine;

public class PopUpManager
{

    [Inject]
    public PopUpManager(RandomGuidGenerater _ra)
    {
        Debug.Log($"{_ra.RandomGuid}");
    }
}
