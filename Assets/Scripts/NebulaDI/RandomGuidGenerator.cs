using System;

[Serializable]
public class RandomGuidGenerater
{
    public Guid RandomGuid { get; set; } = Guid.NewGuid();
}