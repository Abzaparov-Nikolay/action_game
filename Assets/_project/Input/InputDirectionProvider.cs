using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDirectionProvider : NetworkBehaviour
{
    [SerializeField] private Variable<Transform> cameraTransform;
    //private Transform cameraTransform;
    //SerializeField] private Variable<Vector2> output;
    /*[SyncVar]*/ public  Vector2 output;


    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        if (!base.IsOwner) return;
        //Debug.Log("PIPI");
        var wasd = callbackContext.ReadValue<Vector2>();
        var cameraRotation = cameraTransform.Value.eulerAngles;


        var worldInputDirection = wasd
            .normalized
            .Rotate(-cameraRotation.y);
        output = (worldInputDirection);
    }

    //public override void OnStartClient()
    //{
    //    base.OnStartNetwork();
    //    if (base.IsOwner)
    //    {
    //    }
    //    else
    //    {
    //        this.enabled = false;
    //    }
    //}

    public Vector2 Get()
    {
        return output;
    }
}
