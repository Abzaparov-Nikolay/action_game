using FishNet.Object;
using FishNet.Object.Synchronizing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lifetime : NetworkBehaviour
{
    [SerializeField] public Reference<float> intervalBase;
    [SyncObject] public readonly Multiplier intervalMultiplier = new();
    [SerializeField] public bool autoRestart;
    [SerializeField] private bool active;
    [SerializeField] public UnityEvent elapsed;
    [SerializeField] private bool restartOnEnable;
    private float timeLeft;

    //void Awake()
    //{
    //    timeLeft = intervalBase;
    //}

    void Update()
    {
        if (!IsServer) return;
        if (active)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                elapsed?.Invoke();
                if (autoRestart)
                    Restart();
                else
                    active = false;
            }
        }
    }

    public void Restart()
    {
        if (!IsServer) return;
        timeLeft = intervalBase * intervalMultiplier;
        active = true;
    }

    public void AddTimeBonus(float percentage)
    {
        if (!IsServer) return;
        RpcAddTimeBonus(percentage);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        timeLeft = intervalBase * intervalMultiplier;
        //Restart();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RpcAddTimeBonus(float percentage)
    {
        intervalMultiplier.Add(percentage);
    }

    private void OnEnable()
    {
        if (restartOnEnable)
            Restart();
    }
}
