using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeAttacker : NetworkBehaviour
{

    [SerializeField] private Reference<float> BaseAttackRate;
    [SyncObject]
    private readonly Multiplier AttackRateMultiplier = new();
    [SerializeField] private Dealer dealer;
    [SerializeField] private TargetSelector targetSelector;
    [SerializeField] private StatusApplier statusApplier;
    private float timeSinceLastAttack;

    private void Update()
    {
        if (!IsServer) return;
        if (targetSelector.TryGetTarget(out var targets))
        {
            var totalAttackInterval = 1 / (BaseAttackRate * AttackRateMultiplier);
            timeSinceLastAttack += Time.deltaTime;
            while (timeSinceLastAttack > totalAttackInterval)
            {
                timeSinceLastAttack -= totalAttackInterval;
                PerformAttack(timeSinceLastAttack, targets);
            }
        }
    }

    private void PerformAttack(float elapsedTime, Transform[] targets)
    {
        if (!IsServer) return;
        foreach (var target in targets.Where(t => t != null))
        {
            if (dealer != null)
            {
                dealer.Deal(target.gameObject);

            }
            if (statusApplier != null)
            {
                statusApplier.Apply(target.gameObject);
            }
        }
    }

    public void AddAttackRateBonus(float percentage)
    {
        if (!IsServer) return;
        AttackRateMultiplier.Add(percentage);
    }
}
