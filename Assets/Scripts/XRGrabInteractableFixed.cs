using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableFixed : XRGrabInteractable
{
    [SerializeField] private bool _useOffset;
    [SerializeField] private bool _attachToHands;
    
    private Vector3 _interactorPos = Vector3.zero;
    private Quaternion _interactorRot = Quaternion.identity;
    
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (_attachToHands)
        {
            SetParentToXRRig();
        }

        if (_useOffset)
        {
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);

        if (_useOffset)
        {
            ResetAttachmentPoint(interactor);
            ClearInteractor();
        }
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        _interactorPos = interactor.attachTransform.localPosition;
        _interactorRot = interactor.attachTransform.localRotation;
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

    private void SetParentToXRRig()
    {
        transform.SetParent(selectingInteractor.transform);
    }
}
