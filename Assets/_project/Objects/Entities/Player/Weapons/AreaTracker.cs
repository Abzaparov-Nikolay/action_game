using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AreaTracker : NetworkBehaviour, IEnumerable<Transform>
{
    [SerializeField] private new Collider collider;
    [SerializeField] private Reference<float> radiusBase;
    [SyncObject]
    private readonly Multiplier radiusMultiplier = new();
    [SerializeField] private List<Team> teamsToTrack;
    private readonly HashSet<Transform> entitiesInRadius = new();

    private void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider>();
        if (!collider.isTrigger)
            collider.isTrigger = true;
    }

    private void Awake()
    {
        radiusMultiplier.OnChange += OnMultiplierChange;
    }

    public void AddBonus(float percentage)
    {
        if (!IsServer) return;
        radiusMultiplier.Add(percentage);
    }

    private void OnMultiplierChange()
    {
        //if (!asServer) radiusMultiplier = next;
        if (collider is SphereCollider)
        {
            (collider as SphereCollider).radius = radiusBase * radiusMultiplier;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.activeInHierarchy
            && other.gameObject.TryGetComponentInParent<TeamMember>(out var otherTeam)
            //&& otherTeam.isHostileTo(team)
            && teamsToTrack.Contains(otherTeam.Team))
        {
            entitiesInRadius.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        entitiesInRadius.Remove(other.gameObject.transform);
    }

    public IEnumerator<Transform> GetEnumerator()
    {
        return entitiesInRadius.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
