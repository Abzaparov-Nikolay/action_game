using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickaxe : Dealer
{

    [SerializeField] private Reference<float> DiggingDamageInit;
    [SyncVar] private float DiggingDamage;

    public override void Deal(GameObject target)
    {
        if (target.TryGetComponent<Destroyable>(out var destroyable))
        {
            destroyable.TakeDamage(DiggingDamage);
        }
    }

    public void ChangeDamage(float newValue)
    {
        if (!IsServer) return;
        RpcChangeDamage(newValue);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) { return; }
        RpcChangeDamage(DiggingDamageInit);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RpcChangeDamage(float newValue)
    {
        DiggingDamage = newValue;
    }
}
