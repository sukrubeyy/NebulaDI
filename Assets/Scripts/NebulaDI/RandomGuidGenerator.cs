using System;
using UnityEngine;

[Serializable]
public class RandomGuidGenerater
{
    public Guid RandomGuid { get; set; } = Guid.NewGuid();
}