using System;
using System.Collections;
using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class SelectInteractable : XRBaseInteractable
{
    #region Settings

    [SerializeField] private Vector3 flyOutDirection;
    [SerializeField] private float flyOutDuration = 4f;
    [SerializeField] private float rotationSpeed = 10f;

    #endregion

    #region References

    private Rigidbody _rigidbody;

    #endregion

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _initialKinematic;

    private bool _isActivated;

    private Tween _moveTween;

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();

        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
        _initialKinematic = _rigidbody.isKinematic;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (!interactor.CompareTag("Selector")) return;

        StartMovement();
    }

    private void StartMovement()
    {
        _moveTween?.Kill();
        if (_isActivated)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    private void Activate()
    {
        _rigidbody.isKinematic = true;
        var target = _initialPosition + flyOutDirection;
        _moveTween = transform.DOLocalMove(target, flyOutDuration);

        _isActivated = true;
    }

    private void Deactivate()
    {
        _moveTween = transform
            .DOLocalMove(_initialPosition, flyOutDuration)
            .OnComplete(ReturnInitialProperties);

        _isActivated = false;
    }

    private void ReturnInitialProperties()
    {
        _rigidbody.isKinematic = _initialKinematic;
    }
}