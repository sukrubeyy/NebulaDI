using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebula : NebulaInstaller
{
    public override void OverrideBindings()
    {
        Servises.AsTransient<RandomGuidGenerater>();
    }
}
