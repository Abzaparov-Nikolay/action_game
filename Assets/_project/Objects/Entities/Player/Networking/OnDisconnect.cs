using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisconnect : NetworkBehaviour
{
    public static Action<Transform> OnObjectDisconnect;

    public override void OnStopServer()
    {
        base.OnStopServer();
        OnObjectDisconnect?.Invoke(transform);
    }
}
