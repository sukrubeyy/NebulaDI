using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Guid userId { get; set; } = Guid.NewGuid();
}
