using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class SelectInteractable : XRBaseInteractable
{
    #region Settings

    [SerializeField] private float flyOutSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    #endregion

    #region References

    private XRBaseInteractable _interactable;

    #endregion

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
    }

    private void OnEnable()
    {
        _interactable.onSelectEntered.AddListener(OnSelectEntered);
    }
    
    private void OnDisable()
    {
        _interactable.onSelectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (!interactor.CompareTag("Selector")) return;
        
        Debug.Log("YEs");
    }
}