using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFirstSpanw : NetworkBehaviour
{
    public static event Action<Transform> OnObjectClienSpawn;
    public static event Action<Transform> OnObjectServerSpawn;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            //var nobj = base.LocalConnection.FirstObject;
            //if (nobj == NetworkObject)
            //{
                OnObjectClienSpawn?.Invoke(transform);
            //}
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (!IsServer) return;


        OnObjectServerSpawn?.Invoke(transform);


    }
}
