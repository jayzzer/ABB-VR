using System;
using System.Collections;
using DG.Tweening;
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

    public enum Direction
    {
        Left,
        Right
    }

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _initialKinematic;

    private bool _isActivated;

    private Sequence _moveTween;
    private Coroutine _rotationCoroutine;

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
        _moveTween?.Kill();

        _rigidbody.isKinematic = true;
        var target = _initialPosition + flyOutDirection;
        _moveTween = DOTween.Sequence()
            .Append(transform.DOLocalMove(target, flyOutDuration));

        _isActivated = true;
    }

    public void Deactivate()
    {
        _moveTween?.Kill();

        StopRotation();

        _moveTween = DOTween.Sequence()
            .Append(transform.DOLocalMove(_initialPosition, flyOutDuration))
            .Join(transform.DOLocalRotateQuaternion(_initialRotation, flyOutDuration))
            .OnComplete(ReturnInitialProperties);

        _isActivated = false;
    }

    public bool Rotate(Direction direction)
    {
        if (!_isActivated) return false;
        if (_rotationCoroutine != null) return true;

        _rotationCoroutine = StartCoroutine(Rotation(direction));

        return true;
    }

    public void StopRotation()
    {
        if (_rotationCoroutine == null) return;

        StopCoroutine(_rotationCoroutine);
        _rotationCoroutine = null;
    }

    private IEnumerator Rotation(Direction direction)
    {
        var angle = direction == Direction.Left ? -rotationSpeed : rotationSpeed;

        while (true)
        {
            transform.Rotate(Vector3.up, angle * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    private void ReturnInitialProperties()
    {
        _rigidbody.isKinematic = _initialKinematic;
    }
}