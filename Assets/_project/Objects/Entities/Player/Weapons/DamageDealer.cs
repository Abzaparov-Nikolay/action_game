using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : Dealer
{
    public Reference<float> BaseDamage;
    [SyncObject]
    public readonly Multiplier DamageMultiplier = new();
    [SyncVar]
    bool healing;

    public override void Deal(GameObject target)
    {
        if (!IsServer) return;
        if (target.TryGetComponentInParent<DamageReceiver>(out var damageReceiver))
        {
            damageReceiver.TakeDamage(BaseDamage * DamageMultiplier * (!healing == true ? 1 : -1));
        }
    }

    public void AddDamageBonus(float percentage)
    {
        if (!IsServer) return;
        DamageMultiplier.Add(percentage);
    }

    public void AddDamageBonus(float percentage, bool SetHealing)
    {
        if (!IsServer) return;
        DamageMultiplier.Add(percentage);
        healing = SetHealing;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //DamageMultiplier = 1;
    }
}
