using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvincibilityChanger : NetworkBehaviour
{
    [SyncVar]
    private bool SettableFlag;
    [SerializeField] private Reference<float> InvincibilityDurationInitial;
     private float InvincibilityDuration;

    [SerializeField] public UnityEvent<float> OnInvincible;
    private float elapsed;

    private void OnInvincibleChange(bool prev, bool next, bool asServer)
    {
        //if (!asServer)
            SettableFlag = next;
        if (SettableFlag)
        {
            OnInvincible?.Invoke(InvincibilityDuration);
        }
    }

    private void ChangeValue(bool newValue)
    {
        if (!IsServer) { return; }
        SettableFlag = newValue;
        if (SettableFlag)
        {
            OnInvincible?.Invoke(InvincibilityDuration);
        }
    }

    private void RpcChangeDuration(float newValue)
    {
        if (!IsServer) { return; }
        InvincibilityDuration = newValue;
    }

    public void BecomeInvincible()
    {
        if (!base.IsServer) return;
        ChangeValue(true);
        elapsed = 0;
    }

    public void ChangeDuration(float duration)
    {
        if (!IsServer) return;
        RpcChangeDuration(duration);
    }

    private void Update()
    {
        if (!IsServer) return;
        elapsed += Time.deltaTime;
        if (elapsed >= InvincibilityDuration)
        {
            ChangeValue(false);
        }
    }

    public bool Get() => SettableFlag;

    public override void OnStartServer()
    {
        base.OnStartServer();
        InvincibilityDuration = InvincibilityDurationInitial;
    }
}
