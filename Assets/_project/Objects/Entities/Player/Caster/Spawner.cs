using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private SpellsList AllSpells;
    [SerializeField] private ElementsList AllElements;

    [SerializeField] private GameObject holdableSpell;

    public void Spawn(List<ElementType> order)
    {
        //get spell 
        var spell = GetSpellFromOrder(order
            .Select(type => AllElements
            .First(el => el.Type == type)).ToList(),
            out var modifiers);
        if (spell == null) return;


        //server spawn and modify
        SpawnServer(spell.prefab,
            spell.castType,
            modifiers.Select(m => m.Type).ToList(),
            SpawnPoint.position,
            SpawnPoint.rotation);
    }


    public void StopContinuousCast()
    {
        if (!IsOwner) return;
        DespawnSpellServer();
    }

    private Spell GetSpellFromOrder(List<Element> order, out List<Element> modifiers)
    {
        if (order.Count == 0)
        {
            modifiers = null;
            return null;
        }
        var max = order.MinBy(el => el.Power);
        var sameTypeSpells = AllSpells.Where(spell => spell.elementsRequired.MinBy(el => el.Power) == max).ToList();
        var neededSpell = sameTypeSpells.MaxBy(spell =>
        {
            int same = 0;
            foreach (var reqEl in spell.elementsRequired)
            {
                if (order.Contains(reqEl)) same++;
            }
            return same;
        });
        modifiers = order.ToList();

        if (neededSpell == null) return null;

        foreach (var el in neededSpell.elementsRequired)
        {
            modifiers.RemoveAt(modifiers.FindIndex(element => element.Type == el.Type));
        }
        return neededSpell;
    }

    [ServerRpc]
    private void SpawnServer(GameObject spell, CastType castType, List<ElementType> modifiers, Vector3 spawnPosition, Quaternion rotation)
    {
        if (holdableSpell != null) return;
        var spawned = castType == CastType.Tap
            ? Instantiate(spell, new Vector3(spawnPosition.x, 0, spawnPosition.z), rotation)
            : Instantiate(spell, this.transform);
        ServerManager.Spawn(spawned);
        spawned.GetComponent<SpellConfigurator>()
            .SetModifiers(modifiers);
        if (castType == CastType.Hold)
        {
            holdableSpell = spawned;
        }
    }

    [ServerRpc]
    private void DespawnSpellServer()
    {
        if (holdableSpell != null /*&& !holdableSpell.IsDestroyed()*/)
        {
            ServerManager.Despawn(holdableSpell);
        }
        holdableSpell = null;
    }
}
