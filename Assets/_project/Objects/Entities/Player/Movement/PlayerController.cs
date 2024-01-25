using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private InputDirectionProvider inputDirection;
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private float speed;

    [SerializeField] private AnimationStateSetter stateSetter;


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            this.enabled = false;
    }


    private void FixedUpdate()
    {
        body.velocity = new Vector3(inputDirection.Get().x, 0, inputDirection.Get().y) * speed;

        if (body.velocity.magnitude > 0)
        {
            body.rotation = Quaternion.LookRotation(body.velocity, Vector3.up);
            body.constraints = (RigidbodyConstraints)80;
            if (stateSetter != null)
            {
                stateSetter.Set(PlayerState.Walking);

            }
        }
        else
        {
            body.constraints = (RigidbodyConstraints)112;
            if (stateSetter != null)
                stateSetter.Set(PlayerState.Idle);
        }
    }
}
