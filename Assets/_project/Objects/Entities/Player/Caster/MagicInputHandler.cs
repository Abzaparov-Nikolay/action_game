using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MagicInputHandler : NetworkBehaviour
{
    public Action<List<Element>> OnOrderChange;

    [SerializeField] private InputMagicProvider inputMagicProvider;
    [SerializeField] private Spawner Spawner;
    public ElementsList Magics;

    [SyncObject]
    public readonly SyncList<ElementType> CurrentOrder = new();
    private List<Element> currentOrder => CurrentOrder
        .Select(type => Magics.First(magic => magic.Type == type))
        .ToList();

    public void MagicInput(ElementType type)
    {
        if (!IsOwner) return;
        var magic = Magics.First(m => m.Type == type);
        if (!CanOrder(magic)) return;
        HandleCombinationsAndChangeOrder(magic);
    }

    public void CastStarted(CastTarget target)
    {
        if (!IsOwner) return;
        Spawner.Spawn(CurrentOrder.ToList());
        ChangeOrder(new());
    }

    public void CastStopped()
    {
        if (!IsOwner) return;
        Spawner.StopContinuousCast();
    }

    private void Awake()
    {
        inputMagicProvider.CastStarted += CastStarted;
        inputMagicProvider.CastStopped += CastStopped;
        inputMagicProvider.ElementPressed += MagicInput;
    }

    private void HandleCombinationsAndChangeOrder(Element magic)
    {
        if (!magic.HasCombination(currentOrder))
            AddToOrder(magic.Type);
        else
        {
            var newOrder = magic.HandleCombination(currentOrder);
            ChangeOrder(newOrder.Select(m => m.Type).ToList());
        }

    }

    private bool CanOrder(Element type)
    {
        return type.CanOrder(currentOrder);
    }

    [ServerRpc]
    private void AddToOrder(ElementType type)
    {
        CurrentOrder.Add(type);
        Notify(CurrentOrder.ToList());
    }

    [ServerRpc]
    private void ChangeOrder(List<ElementType> type)
    {
        CurrentOrder.Clear();
        foreach (var magic in type)
        {
            CurrentOrder.Add(magic);
        }
        Notify(CurrentOrder.ToList());
    }

    [ObserversRpc]
    private void Notify(List<ElementType> elements)
    {
        OnOrderChange?.Invoke(elements
        .Select(type => Magics.First(magic => magic.Type == type))
        .ToList());
    }
}
