using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusApplier : NetworkBehaviour
{
    [SyncVar]
    public StatusEffect Status;

    [SyncObject]
    public readonly Multiplier Power = new();

    public void SetStatusEffect(StatusEffect statusEffect)
    {
        if (!IsServer) return;
        Status = statusEffect;
    }

    public void Apply(GameObject target)
    {
        if (!IsServer) return;
        //getcomponent => setstatus
    }

    public void AddPowerBonus(float percentage)
    {
        if (!IsServer) return;
        Power.Add(percentage);
    }
}

