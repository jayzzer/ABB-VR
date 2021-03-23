using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPoseController : MonoBehaviour
{
    #region References

    private HandPresence _handPresence;
    private XRDirectInteractor _interactor;
    private PhysicsHandAnimation _physicsHandAnimation;

    #endregion

    private void Awake()
    {
        _handPresence = GetComponentInChildren<HandPresence>();
        _physicsHandAnimation = GetComponentInChildren<PhysicsHandAnimation>();
        _interactor = GetComponent<XRDirectInteractor>();
    }

    private void OnEnable()
    {
        _interactor.onHoverEntered.AddListener(DisableGripAnimation);
        _interactor.onHoverExited.AddListener(EnableGripAnimation);
        
        _interactor.onSelectEntered.AddListener(StartHandSqueeze);
        _interactor.onSelectExited.AddListener(StopHandSqueeze);
    }

    private void OnDisable()
    {
        _interactor.onHoverEntered.RemoveListener(DisableGripAnimation);
        _interactor.onHoverExited.RemoveListener(EnableGripAnimation);
    }

    private void EnableGripAnimation(XRBaseInteractable interactable)
    {
        _handPresence.isGripAnimEnabled = true;
    }

    private void DisableGripAnimation(XRBaseInteractable interactable)
    {
        _handPresence.isGripAnimEnabled = false;
    }

    private void StartHandSqueeze(XRBaseInteractable interactable)
    {
        _physicsHandAnimation.StartSqueeze();
    }
    
    private void StopHandSqueeze(XRBaseInteractable interactable)
    {
        _physicsHandAnimation.StopSqueeze();
    }
}