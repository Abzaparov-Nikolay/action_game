using FishNet.Connection;
using FishNet.Object;
using HeathenEngineering.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStater : NetworkBehaviour
{
    //public Action<NetworkConnection> PlayerDyied;
    private List<Transform> players = new();
    private int deadCount = 0;
    public static Action GameEnded;


    private void Start()
    {
        //OnFirstSpanw.OnObjectClienSpawn += PlayerSpawned;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        OnFirstSpanw.OnObjectServerSpawn += PlayerSpawned;
    }


    private void PlayerSpawned(Transform transform)
    {
        if (!IsServer) return;
        players.Add(transform);
        transform.GetComponent<Death>().OnDeath.AddListener(PlayerDied);
    }


    private void PlayerDied()
    {
        if (!IsServer)
            return;
        deadCount++;
        if(deadCount == players.Count)
        {
            GameLost();
        }
    }

    [ObserversRpc]
    public void GameLost()
    {
        GameEnded?.Invoke();
    }


}
