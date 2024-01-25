using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersTracker : NetworkBehaviour
{
    private static PlayersTracker _instance;
    public static PlayersTracker Instance { get => _instance; }
    public void Awake()
    {
        if (_instance != null)
        {
            Debug.Log("PlayersTracker already exists");
        }
        _instance = this;
        OnFirstSpanw.OnObjectServerSpawn += AddPlayer;
        OnDisconnect.OnObjectDisconnect += RemovePlayer;
    }

    public void OnDestroy()
    {
        OnFirstSpanw.OnObjectServerSpawn -= AddPlayer;
        OnDisconnect.OnObjectDisconnect -= RemovePlayer;

    }

    public List<Transform> PlayersTransforms = new();

    public void AddPlayer(Transform playersTransform)
    {
        if (!IsServer) return;
        PlayersTransforms.Add(playersTransform);
    }

    public void RemovePlayer(Transform playersTransform)
    {
        if (!IsServer) return;
        PlayersTransforms.Remove(playersTransform);
    }

    public Transform GetNearest(Transform other)
    {
        if (!IsServer) return null;
        return PlayersTransforms.MinBy(tr => (tr.position - other.position).magnitude);
    }
}
