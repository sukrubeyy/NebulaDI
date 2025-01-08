using System;
using UnityEngine;

[Serializable]
public class RandomGuidGenerater : MonoBehaviour
{
    public Guid RandomGuid { get; set; } = Guid.NewGuid();
}