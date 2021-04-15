using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectorController : MonoBehaviour
{
    #region References

    [SerializeField] private XRDirectInteractor directInteractor;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private GameObject selectorContainer;
    [SerializeField] private InputActionProperty enableAction;

    [SerializeField] private InputActionProperty rotateRightAction;
    [SerializeField] private InputActionProperty rotateLeftAction;

    #endregion

    private bool _isHovering;
    private SelectInteractable _selectedObject;

    private void Awake()
    {
        selectorContainer.SetActive(false);
    }

    private void OnEnable()
    {
        directInteractor.onHoverEntered.AddListener(OnHoverEntered);
        directInteractor.onHoverExited.AddListener(OnHoverExited);

        directInteractor.onSelectEntered.AddListener(OnSelectEntered);
        
        rayInteractor.onSelectEntered.AddListener(OnRaySelectEntered);

        enableAction.action.performed += OnEnableActionPerformed;
        enableAction.action.canceled += OnEnableActionCanceled;

        rotateLeftAction.action.started += OnRotateLeftActionStarted;
        rotateLeftAction.action.canceled += OnRotateActionCanceled;
        rotateRightAction.action.started += OnRotateRightActionStarted;
        rotateRightAction.action.canceled += OnRotateActionCanceled;
    }

    private void OnDisable()
    {
        directInteractor.onHoverEntered.RemoveListener(OnHoverEntered);
        directInteractor.onHoverExited.RemoveListener(OnHoverExited);

        directInteractor.onSelectEntered.RemoveListener(OnSelectEntered);
        
        rayInteractor.onSelectEntered.RemoveListener(OnRaySelectEntered);

        enableAction.action.performed -= OnEnableActionPerformed;
        enableAction.action.canceled -= OnEnableActionCanceled;
        
        rotateLeftAction.action.started -= OnRotateLeftActionStarted;
        rotateLeftAction.action.canceled -= OnRotateActionCanceled;
        rotateRightAction.action.started -= OnRotateRightActionStarted;
        rotateRightAction.action.canceled -= OnRotateActionCanceled;
    }

    private void OnHoverEntered(XRBaseInteractable interactable)
    {
        _isHovering = true;
    }

    private void OnHoverExited(XRBaseInteractable interactable)
    {
        _isHovering = false;
    }

    private void OnEnableActionPerformed(InputAction.CallbackContext context)
    {
        selectorContainer.SetActive(!_isHovering);
    }

    private void OnEnableActionCanceled(InputAction.CallbackContext context)
    {
        DisableSelector();
    }

    private void OnRotateLeftActionStarted(InputAction.CallbackContext context)
    {
        if (!_selectedObject) return;
        if (!_selectedObject.Rotate(SelectInteractable.Direction.Right)) _selectedObject = null;
    }
    
    private void OnRotateRightActionStarted(InputAction.CallbackContext context)
    {
        if (!_selectedObject) return;
        if (!_selectedObject.Rotate(SelectInteractable.Direction.Left)) _selectedObject = null;
    }

    private void OnRotateActionCanceled(InputAction.CallbackContext context)
    {
        if (!_selectedObject) return;
        _selectedObject.StopRotation();
    }

    private void OnSelectEntered(XRBaseInteractable interactable)
    {
        DisableSelector();
    }

    private void OnRaySelectEntered(XRBaseInteractable interactable)
    {
        if (_selectedObject && _selectedObject.name == interactable.name)
        {
            _selectedObject = null;
            return;
        }

        if (!interactable.TryGetComponent(out SelectInteractable selectInteractable)) return;
        if (_selectedObject) _selectedObject.Deactivate();
        _selectedObject = selectInteractable;
    }

    private void DisableSelector()
    {
        selectorContainer.SetActive(false);
    }
}