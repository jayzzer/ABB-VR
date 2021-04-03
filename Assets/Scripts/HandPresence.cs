using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresence : BaseHand
{
    public InputDeviceCharacteristics deviceCharacteristics;
    public GameObject spawnedHandModel;

    private InputDevice _targetDevice;
    private Animator _handAnimator;
    private XRBaseInteractor _interactor;

    private bool isGripActive = true;

    private void OnEnable()
    {
        _interactor.onSelectEntered.AddListener(TryApplyObjectPose);
        _interactor.onSelectExited.AddListener(TryApplyDefaultPose);
        
        _interactor.onHoverEntered.AddListener(DisableGrip);
        _interactor.onHoverExited.AddListener(EnableGrip);
    }

    private void OnDisable()
    {
        _interactor.onSelectEntered.RemoveListener(TryApplyObjectPose);
        _interactor.onSelectExited.RemoveListener(TryApplyDefaultPose);
        
        _interactor.onHoverEntered.RemoveListener(DisableGrip);
        _interactor.onHoverExited.RemoveListener(EnableGrip);
    }

    protected override void Awake()
    {
        base.Awake();
        spawnedHandModel = transform.GetChild(0).gameObject;
        _handAnimator = spawnedHandModel.GetComponent<Animator>();
        spawnedHandModel.SetActive(false);
    }

    private void Start()
    {
        TryInitializeHand();
    }

    private void TryInitializeHand()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        if (devices.Count <= 0) return;
        _targetDevice = devices[0];
        spawnedHandModel.SetActive(true);
    }

    private void Update()
    {
        if (!_targetDevice.isValid)
        {
            TryInitializeHand();
            return;
        }

        UpdateHandAnimation();
    }

    private void UpdateHandAnimation()
    {
        if (_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            _handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            _handAnimator.SetFloat("Trigger", 0);
        }

        if (isGripActive && _targetDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue))
        {
            _handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            _handAnimator.SetFloat("Grip", 0);
        }
    }

    private void TryApplyObjectPose(XRBaseInteractable interactable)
    {
        _handAnimator.enabled = false;
        if (interactable.TryGetComponent(out PoseContainer poseContainer))
        {
            ApplyPose(poseContainer.pose);
        }
    }

    private void TryApplyDefaultPose(XRBaseInteractable interactable)
    {
        if (interactable.TryGetComponent(out PoseContainer poseContainer))
        {
            ApplyDefaultPose();
        }
        _handAnimator.enabled = true;
    }

    private void EnableGrip(XRBaseInteractable interactable)
    {
        isGripActive = true;
    }
    
    private void DisableGrip(XRBaseInteractable interactable)
    {
        isGripActive = false;
    }

    public override void ApplyOffset(Vector3 position, Quaternion rotation)
    {
        var finalPosition = position * -1f;
        var finalRotation = Quaternion.Inverse(rotation);

        finalPosition = finalPosition.RotatePointAroundPivot(Vector3.zero, finalRotation.eulerAngles);

        _interactor.attachTransform.localPosition = finalPosition;
        _interactor.attachTransform.localRotation = finalRotation;
    }

    private void OnValidate()
    {
        if (!_interactor)
        {
            _interactor = GetComponentInParent<XRBaseInteractor>();
        }
    }
}