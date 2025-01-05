using System;
using UnityEngine;

public class DoSomething : ISomeThing
{
    public Guid ID { get; set; } = Guid.NewGuid();

    public void Print()
    {
        Debug.Log(ID);
    }
}
