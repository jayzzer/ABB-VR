using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableFixed : XRGrabInteractable
{
    #region Settings

    [SerializeField] private bool _remainParent;
    [SerializeField] private bool _useOffset;
    [SerializeField] private bool _attachToHands;
    [SerializeField] private bool _attachToObj;

    [SerializeField] private bool _hideHandOnGrab;

    #endregion

    private Vector3 _interactorPos = Vector3.zero;
    private Quaternion _interactorRot = Quaternion.identity;

    private HandPresence _handPresence;
    private Transform _handModel;

    private Transform _originalHandParent;
    private Vector3 _originalHandPosition;
    private Quaternion _originalHandRotation;

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        if (_remainParent)
        {
            _originalHandParent = transform.parent;
        }

        base.OnSelectEntering(interactor);

        if (_remainParent)
        {
            transform.parent = _originalHandParent;
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (_attachToHands)
        {
            SetParentToXRRig(interactor);
        }

        if (_attachToObj || _hideHandOnGrab)
        {
            StoreHand(interactor);
        }

        if (_attachToObj)
        {
            SetHandToObj(interactor);
        }

        if (_hideHandOnGrab)
        {
            _handModel.gameObject.SetActive(false);
        }

        if (_useOffset)
        {
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
        }
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

        if (_hideHandOnGrab)
        {
            _handModel.gameObject.SetActive(true);
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
        _handPresence = interactor.transform.GetComponentInChildren<HandPresence>();
        _handModel = _handPresence.spawnedHandModel.transform;
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
        _originalHandPosition = _handModel.localPosition;
        _originalHandRotation = _handModel.localRotation;

        _handModel.parent = transform;

        if (TryGetComponent(out PoseContainer poseContainer))
        {
            var handInfo = poseContainer.pose.GetHandInfo(_handPresence.HandType);
            _handModel.localPosition = handInfo.attachPosition;
            _handModel.localRotation = handInfo.attachRotation;
        }
    }

    private void ReturnHandToParent(XRBaseInteractor interactor)
    {
        _handModel.parent = interactor.transform;
        _handModel.localPosition = _originalHandPosition;
        _handModel.localRotation = _originalHandRotation;
    }
}