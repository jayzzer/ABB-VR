using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    [SerializeField] private ActionBasedController _controller;
    [SerializeField] private InputActionProperty _enableAction;
    
    private void Update()
    {
        if (!_controller) return;
        _controller.gameObject.SetActive(IsPressed(_enableAction.action));
    }

    private static bool IsPressed(InputAction action)
    {
        return action.triggered || action.phase == InputActionPhase.Performed;
    }
}
