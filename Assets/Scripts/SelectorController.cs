using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectorController : MonoBehaviour
{
    #region References

    [SerializeField] private XRBaseInteractor interactor;
    [SerializeField] private GameObject selectorContainer;
    [SerializeField] private InputActionProperty enableAction;

    #endregion

    private bool _isHovering;

    private void Awake()
    {
        selectorContainer.SetActive(false);
    }

    private void OnEnable()
    {
        interactor.onHoverEntered.AddListener(OnHoverEntered);
        interactor.onHoverExited.AddListener(OnHoverExited);

        interactor.onSelectEntered.AddListener(OnSelectEntered);

        enableAction.action.performed += OnActionPerformed;
        enableAction.action.canceled += OnActionCanceled;
    }

    private void OnDisable()
    {
        interactor.onHoverEntered.RemoveListener(OnHoverEntered);
        interactor.onHoverExited.RemoveListener(OnHoverExited);

        interactor.onSelectEntered.RemoveListener(OnSelectEntered);

        enableAction.action.performed -= OnActionPerformed;
        enableAction.action.canceled -= OnActionCanceled;
    }

    private void OnHoverEntered(XRBaseInteractable interactable)
    {
        _isHovering = true;
    }

    private void OnHoverExited(XRBaseInteractable interactable)
    {
        _isHovering = false;
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        selectorContainer.SetActive(!_isHovering);
    }

    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        DisableSelector();
    }

    private void OnSelectEntered(XRBaseInteractable interactable)
    {
        DisableSelector();
    }

    private void DisableSelector()
    {
        selectorContainer.SetActive(false);
    }
}