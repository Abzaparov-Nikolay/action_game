using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateSetter : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SyncVar(OnChange = nameof(OnUpdate))]
    private PlayerState CurrentState;

    public void Set(PlayerState state)
    {
        
        SetServer(state);
    }

    [ServerRpc]
    private void SetServer(PlayerState state)
    {
        CurrentState = state;
        UpdateAnimator();
    }

    private void OnUpdate(PlayerState prev, PlayerState next, bool asServer)
    {
        if (!asServer)
            CurrentState = next;
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Walking", CurrentState == PlayerState.Walking);
    }
}

public enum PlayerState
{
    Idle,
    Walking
}
