using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using UnityEngine;

public class ExperienceManager : NetworkBehaviour
{
    [SyncVar(OnChange = nameof(OnChange))]
    public float Amount;

    public Action<float> OnAmountChange;

    private static Action<float> AddAmountAll;

    public static void AddAll(float amount)
    {
        AddAmountAll?.Invoke(amount);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        AddAmountAll += AddAmount;
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        AddAmountAll += AddAmount;
    }

    public void AddAmount(float amount)
    {
        RpcSetAmount(amount + Amount);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RpcSetAmount(float newValue)
    {
        Amount = newValue;
    }

    private void OnChange(float prev, float next, bool asServer)
    {
        if (!asServer)
        {
            Amount = next;
        }
        OnAmountChange?.Invoke(Amount);
        //update ui
    }

    //public void Test()
    //{
    //    AddAmount(1);
    //}
}
