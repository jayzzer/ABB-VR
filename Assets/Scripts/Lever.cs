using System;
using System.Collections;
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
    [SerializeField] private float switchingSpeed = 100f;

    [SerializeField] private bool useSpring;
    [SerializeField] private float springTargetAngle = 0f;
    [SerializeField] private float springSpeed = 100f;

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
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _joint = GetComponent<HingeJoint>();
        _rigidbody = GetComponent<Rigidbody>();

        if (useSpring)
        {
            _interactable = GetComponent<XRGrabInteractable>();
        }
    }

    private void OnEnable()
    {
        if (useSpring)
        {
            _interactable.onSelectExited.AddListener(ReturnToStartRotation);
        }
    }

    private void OnDisable()
    {
        if (useSpring)
        {
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
        if (_currentCoroutine == null && _joint.angle < _joint.limits.max - switchingAngle &&
            _joint.angle > _joint.limits.min + switchingAngle)
        {
            var targetRotation = _wasTurnedOn
                ? Quaternion.Euler(_joint.axis * _joint.limits.min)
                : Quaternion.Euler(_joint.axis * _joint.limits.max);
            _currentCoroutine = StartCoroutine(SmoothTurn(targetRotation, switchingSpeed));
        }
    }

    private IEnumerator SmoothTurn(Quaternion target, float speed, Action endMethod = null)
    {
        while (Quaternion.Angle(transform.localRotation, target) >= 0.1f)
        {
            transform.localRotation =
                Quaternion.RotateTowards(transform.localRotation, target, speed * Time.deltaTime);
            yield return null;
        }

        endMethod?.Invoke();

        _currentCoroutine = null;
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
        _currentCoroutine = StartCoroutine(
            SmoothTurn(
                Quaternion.Euler(_joint.axis * springTargetAngle),
                springSpeed,
                FreezeObject
            )
        );
    }

    private void FreezeObject()
    {
        _rigidbody.isKinematic = true;
    }
}