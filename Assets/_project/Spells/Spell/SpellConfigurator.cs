using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SpellConfigurator : NetworkBehaviour
{
    protected Dictionary<ElementType, Action<int, Modifier>> modifiers = new();

    protected List<ElementType> modifiersList = new();

    [SerializeField] private ModifierList buffs;
    public void SetModifiers(List<ElementType> elements)
    {
        modifiersList = elements;
        ApplyModifiers();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //if (!IsServer) { return; }
        //ApplyModifiers();
    }

    private void Awake()
    {
        modifiers.Add(ElementType.Roots, HandleRootModifier);
        modifiers.Add(ElementType.Fire, HandleFireModifier);
        modifiers.Add(ElementType.Void, HandleVoidModifier);
        modifiers.Add(ElementType.Tree, HandleTreeModifier);
        modifiers.Add(ElementType.Leaves, HandleLeavesModifier);
        modifiers.Add(ElementType.Moon, HandleMoonModifier);
        modifiers.Add(ElementType.Sun, HandleSunModifier);
        modifiers.Add(ElementType.Stars, HandleStarsModifier);
        modifiers.Add(ElementType.Harmony, HandleHarmonyModifier);
        //gameObject.SetActive(false);
    }

    protected void ApplyModifiers()
    {
        foreach (var (element, count) in modifiersList
            .Distinct()
            .Select(el => (el, modifiersList.FindAll(ele => ele == el).Count)))
        {
            if (!buffs.Any(buff => buff.ModifierType == element))
            {
                Debug.Log($"U forgor to assign a modifier for {element} :skull:");
                continue;
            }
            modifiers[element]?.Invoke(count, buffs.First(buff => buff.ModifierType == element));
        }
        //gameObject.SetActive(true);
    }


    protected virtual void HandleRootModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleFireModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleHarmonyModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleSunModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleMoonModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleVoidModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleTreeModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleStarsModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);

    }

    protected virtual void HandleLeavesModifier(int count, Modifier buff)
    {
        HandleModifier(count, buff);
    }

    private void HandleModifier(int count, Modifier buff)
    {

    }
}
