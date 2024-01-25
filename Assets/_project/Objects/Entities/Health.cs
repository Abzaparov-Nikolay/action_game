using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public Reference<float> MaxInit;
    [SyncVar]
    private float Max;
    [SyncVar]
    private float Current;

    [SyncVar]
    private float Regeneration;

    //[SerializeField] private readonly Reference<bool> Invincible;

    [SerializeField] private UnityEvent<float> OnDamaged;
    [SerializeField] private UnityEvent<float> OnCurrentZero;


    //private void OnCurrentChange(float prev, float next, bool asServer)
    //{
    //    //if (!IsServer) return;
    //    if (!asServer)
    //        Current = next;
    //    var amount = prev - next;

    //    if (next <= 0)
    //    {
    //        OnCurrentZero?.Invoke(amount);
    //    }
    //    else
    //    {
    //        OnDamaged?.Invoke(amount);
    //    }
    //}

    //[ServerRpc(RequireOwnership = false)]
    private void ChangeCurrent(float newValue)
    {
        if (!IsServer) return;
        var amount = Current - newValue;
        Current = newValue;
        if (Current <= 0)
        {
            OnCurrentZero?.Invoke(amount);
        }
        else
        {
            OnDamaged?.Invoke(amount);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeMax(float newValue)
    {
        if (!IsServer) return;
        Max = newValue;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeRegen(float newValue)
    {
        if (!IsServer) return;
        Regeneration = newValue;
    }

    public void TakeDamage(float amount)
    {
        if (!base.IsServer) return;

        ChangeCurrent(Current - amount);
    }

    public float Get()
    {
        return Current;
    }

    private void Update()
    {
        if (IsServer) return;
        //ChangeCurrent(Get() + Regeneration * Time.deltaTime);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!IsServer) return;
        Max = MaxInit;
        Current = MaxInit;
    }

    public void AddBonus(float percentage)
    {
        if (!IsServer) return;
        var newMax = Max * (1 + percentage / 100);
        var newCurrent = Current < newMax ? newMax : Current * newMax / Max;
        ChangeMax(newMax);
        ChangeCurrent(newCurrent);
    }
}
