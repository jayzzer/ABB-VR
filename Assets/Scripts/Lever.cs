using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(HingeJoint))]
public class Lever : MonoBehaviour
{
    #region Settings

    [SerializeField] private float angleThreshold = 0.1f;

    [SerializeField] private bool useSwitchingAngle;
    [SerializeField] private float switchingAngle = 10f;
    [SerializeField] private float switchingDuration = 1f;

    [SerializeField] private bool useSpring;
    [SerializeField] private float springTargetAngle;
    [SerializeField] private float springReturnDuration = 1f;

    #endregion

    #region Events

    public UnityEvent onTurnedOn;
    public UnityEvent onTurnedOff;

    #endregion

    #region References

    private HingeJoint _joint;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _interactable;

    #endregion

    private bool _isTurned;
    private bool _wasTurnedOn;
    private Tween _currentTween;

    private void Awake()
    {
        _joint = GetComponent<HingeJoint>();
        _rigidbody = GetComponent<Rigidbody>();

        if (useSpring)
        {
            _interactable = GetComponent<XRGrabInteractable>();
            FreezeObject();
        }
    }

    private void OnEnable()
    {
        if (useSpring)
        {
            _interactable.onSelectEntered.AddListener(UnfreezeObject);
            _interactable.onSelectExited.AddListener(ReturnToStartRotation);
        }
    }

    private void OnDisable()
    {
        if (useSpring)
        {
            _interactable.onSelectEntered.RemoveListener(UnfreezeObject);
            _interactable.onSelectExited.RemoveListener(ReturnToStartRotation);
        }
    }

    private void Update()
    {
        CheckLimits();

        if (useSwitchingAngle)
        {
            CheckSwitchingAngle();
        }
    }

    private void CheckSwitchingAngle()
    {
        if (_currentTween != null || !(_joint.angle < _joint.limits.max - switchingAngle) ||
            !(_joint.angle > _joint.limits.min + switchingAngle)) return;
        var targetRotation = _wasTurnedOn
            ? Quaternion.Euler(_joint.axis * _joint.limits.min)
            : Quaternion.Euler(_joint.axis * _joint.limits.max);
        _currentTween = transform
            .DOLocalRotateQuaternion(targetRotation, switchingDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => _currentTween = null);
    }

    private void CheckLimits()
    {
        if (!_isTurned)
        {
            if (IsAchievedMax())
            {
                onTurnedOn.Invoke();
                _wasTurnedOn = _isTurned = true;
            }
            else if (IsAchievedMin())
            {
                onTurnedOff.Invoke();
                _wasTurnedOn = false;
                _isTurned = true;
            }
        }
        else if (!IsAchievedMax() && !IsAchievedMin())
        {
            _isTurned = false;
        }
    }

    private bool IsAchievedMax()
    {
        return _joint.angle >= _joint.limits.max - angleThreshold;
    }

    private bool IsAchievedMin()
    {
        return _joint.angle <= _joint.limits.min + angleThreshold;
    }

    private void ReturnToStartRotation(XRBaseInteractor interactor)
    {
        _currentTween = transform
            .DOLocalRotate(_joint.axis * springTargetAngle, springReturnDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                FreezeObject();
                _currentTween = null;
            });
    }

    private void FreezeObject()
    {
        _rigidbody.freezeRotation = true;
    }

    private void UnfreezeObject(XRBaseInteractor interactor)
    {
        _rigidbody.freezeRotation = false;
    }
}