using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableFixed : XRGrabInteractable
{
    [SerializeField] private bool _useOffset;
    [SerializeField] private bool _attachToHands;
    [SerializeField] private bool _attachToObj;

    private Vector3 _interactorPos = Vector3.zero;
    private Quaternion _interactorRot = Quaternion.identity;
    private Transform _hand;

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (_attachToHands)
        {
            SetParentToXRRig(interactor);
        }

        if (_attachToObj)
        {
            StoreHand(interactor);
            SetHandToObj(interactor);
        }

        if (_useOffset)
        {
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
        }
        
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        if (_useOffset)
        {
            ResetAttachmentPoint(interactor);
            ClearInteractor();
        }

        if (_attachToObj)
        {
            ReturnHandToParent(interactor);
        }
        
        base.OnSelectExited(interactor);
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        _interactorPos = interactor.attachTransform.localPosition;
        _interactorRot = interactor.attachTransform.localRotation;
    }

    private void StoreHand(XRBaseInteractor interactor)
    {
        _hand = interactor.transform.GetComponentInChildren<HandPresence>().transform;
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        var hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    private void ResetAttachmentPoint(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = _interactorPos;
        interactor.attachTransform.localRotation = _interactorRot;
    }

    private void ClearInteractor()
    {
        _interactorPos = Vector3.zero;
        _interactorRot = Quaternion.identity;
    }

    private void SetParentToXRRig(XRBaseInteractor interactor)
    {
        transform.SetParent(interactor.transform);
    }

    private void SetHandToObj(XRBaseInteractor interactor)
    {
        _hand.parent = transform;
    }

    private void ReturnHandToParent(XRBaseInteractor interactor)
    {
        _hand.parent = interactor.transform;
        _hand.localPosition = Vector3.zero;
        _hand.localRotation = Quaternion.identity;
    }
}