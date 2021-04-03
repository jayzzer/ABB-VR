using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HingeJoint))]
public class Lever : MonoBehaviour
{
    #region Settings

    [SerializeField] private float angleThreshold = 0.1f;

    [SerializeField] private bool useSwitchingAngle;
    [SerializeField] private float switchingAngle = 10f;
    [SerializeField] private float switchingSpeed = 100f;

    #endregion

    #region Events

    public UnityEvent onTurnedOn;
    public UnityEvent onTurnedOff;

    #endregion

    #region References

    private HingeJoint _joint;

    #endregion

    private bool _isTurned;
    private bool _wasTurnedOn;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _joint = GetComponent<HingeJoint>();
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
            _currentCoroutine = StartCoroutine(SmoothTurn(_wasTurnedOn
                ? Quaternion.Euler(_joint.axis * _joint.limits.min)
                : Quaternion.Euler(_joint.axis * _joint.limits.max)));
        }
    }

    private IEnumerator SmoothTurn(Quaternion target)
    {
        while (Quaternion.Angle(transform.localRotation, target) >= 0.1f)
        {
            transform.localRotation =
                Quaternion.RotateTowards(transform.localRotation, target, switchingSpeed * Time.deltaTime);
            yield return null;
        }

        _currentCoroutine = null;
    }

    private void CheckLimits()
    {
        if (!_isTurned)
        {
            if (IsAchievedMax())
            {
                onTurnedOn.Invoke();
                _wasTurnedOn = true;
                _isTurned = true;
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
}