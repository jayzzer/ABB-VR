using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    [SerializeField] private InputActionProperty enableAction;

    private void Awake()
    {
        controller.SetActive(false);
    }

    private void OnEnable()
    {
        enableAction.action.performed += EnableLocomotionController;
        enableAction.action.canceled += OnActionCanceled;
    }

    private void OnDisable()
    {
        enableAction.action.performed -= EnableLocomotionController;
        enableAction.action.canceled -= OnActionCanceled;
    }

    private void EnableLocomotionController(InputAction.CallbackContext context)
    {
        controller.SetActive(true);
    }

    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        StartCoroutine(DisableLocomotionController());
    }


    private IEnumerator DisableLocomotionController()
    {
        yield return new WaitForEndOfFrame();
        controller.SetActive(false);
    }
}