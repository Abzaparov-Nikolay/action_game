using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesSprayer : NetworkBehaviour
{
    [SerializeField] private GameObject leafPrefab;

    [SerializeField] private LeavesSprayDefaultValues defaultValues;

    public List<ElementType> leavesModificators;

    [SyncVar] private float fireRate;
    [SyncVar] private float firerateMultiplier;
    [SyncVar] private float degreesOfAttack;
    [SyncVar] private float degreesOfAttackMultiplier;
    [SyncVar] private float leafImpulse;
    [SyncVar] private float leafImpulseMultiplier;

    private float timeSinceLastAttack;

    private void Update()
    {
        if (!IsServer) return;

        var totalAttackInterval = 1 / (fireRate * firerateMultiplier);
        timeSinceLastAttack += Time.deltaTime;
        while (timeSinceLastAttack > totalAttackInterval)
        {
            timeSinceLastAttack -= totalAttackInterval;
            PerformAttack(timeSinceLastAttack);
        }
        //spawn leaves
    }
    private void PerformAttack(float timeSinceLastAttack)
    {
        if (!IsServer) return;

        SpawnLeaf(leafPrefab,
            leavesModificators,
            transform.position + timeSinceLastAttack * transform.forward,
            transform.rotation
            * Quaternion.AngleAxis(
                UnityEngine.Random.Range(-degreesOfAttack * degreesOfAttackMultiplier / 2
                , degreesOfAttack * degreesOfAttackMultiplier / 2)
                , Vector3.up),
            leafImpulse * leafImpulseMultiplier);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        fireRate = defaultValues.fireRate;
        firerateMultiplier = defaultValues.firerateMultiplier;
        degreesOfAttack = defaultValues.degreesOfAttack;
        degreesOfAttackMultiplier = defaultValues.degreesOfAttackMultiplier;
        leafImpulse = defaultValues.leafImpulse;
        leafImpulseMultiplier = defaultValues.leafImpulseMultiplier;
        //leavesModificators = elements.Where(mod => mod);
    }

    private void SpawnLeaf(GameObject leaf,
        List<ElementType> modifiers,
        Vector3 position,
        Quaternion rotation,
        float impulse)
    {
        var spawned = Instantiate(leaf, position, rotation);
        ServerManager.Spawn(spawned);
        spawned.GetComponent<LeafConfigurator>().SetModifiers(modifiers);
        spawned.GetComponent<Rigidbody>().AddForce(spawned.transform.forward * impulse, ForceMode.Impulse);
    }

    public void AddFirerateBonus(float percentage)
    {
        if (!IsServer) return;
        firerateMultiplier += percentage;
    }

    public void AddAccuracyBonus(float percentage)
    {
        if (!IsServer) return;
        degreesOfAttackMultiplier += percentage;
        if (degreesOfAttackMultiplier < 0)
            degreesOfAttackMultiplier = 0;
    }

    public void AddLeafImpulseBonus(float percentage)
    {
        if (!IsServer) return;
        leafImpulseMultiplier += percentage;
    }
}
