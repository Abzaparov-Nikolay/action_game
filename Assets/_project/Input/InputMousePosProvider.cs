using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMousePosProvider : MonoBehaviour
{
    [SerializeField] Variable<Vector2> mousePos;

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePos.Set(context.ReadValue<Vector2>());
    }
}
