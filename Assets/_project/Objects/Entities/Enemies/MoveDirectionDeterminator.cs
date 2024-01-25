using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirectionDeterminator : NetworkBehaviour
{
    HashSet<Transform> aggroSources = new();

    public Vector3 GetMoveDirection()
    {
        if (aggroSources.Count != 0)
        {
            return (aggroSources
                .MinBy(tr => (tr.position - transform.position).magnitude).position
                - transform.position).normalized;
        }
        var playerPosition = PlayersTracker.Instance.GetNearest(transform);
        if (playerPosition != null)
            return (playerPosition.position - transform.position).normalized;
        return Vector3.zero;
    }

    public void AddAggroSource(GameObject obj)
    {
        if (!IsServer) return;
        if (obj == null) return;
        if (!obj.TryGetComponentInParent<Death>(out var death)) return;
        death.OnDeathWithGameObject.AddListener(RemoveAggroSource);
        aggroSources.Add(death.transform);

    }

    private void RemoveAggroSource(GameObject obj)
    {
        aggroSources.Remove(obj.transform);
    }
}
