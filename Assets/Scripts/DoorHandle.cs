using System;
using UnityEngine;
using UnityEngine.Events;


public class DoorHandle : MonoBehaviour
{
    #region References

    public GameObject door;
    private HingeJoint _joint;
    private HingeJoint _doorJoint;
    private Rigidbody _doorRigidbody;

    #endregion

    #region Events

    public UnityEvent onReachedEnd;

    #endregion

    private const float DoorAngleThreshold = 0.2f;

    private enum DoorHandleState
    {
        Vertical,
        Horizontal
    }

    private enum DoorOpenState
    {
        Closed,
        InProgress,
        Opened,
    }

    private DoorHandleState _currentState = DoorHandleState.Vertical;
    private DoorOpenState _openState = DoorOpenState.Closed;
    private bool _canGoBack = false;
    private bool _canGoForward = true;
    
    private void Awake()
    {
        _joint = GetComponent<HingeJoint>();
        _doorJoint = door.GetComponent<HingeJoint>();
        _doorRigidbody = door.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckLimit();
    }

    private void CheckLimit()
    {
        Debug.Log(_currentState + " " + _canGoBack + " " + _canGoForward + " " + _joint.angle);
        switch (_currentState)
        {
            case DoorHandleState.Vertical:
                if (_canGoForward && IsAchievedMaxAngle())
                {
                    _joint.axis = new Vector3(0, 0, -1);
                    _joint.limits = new JointLimits {min = 0, max = 90};
                    _canGoBack = false;
                    _canGoForward = false;
                    _currentState = DoorHandleState.Horizontal;
                }

                if (IsAchievedMinAngle())
                {
                    Debug.Log("Undone");
                }

                if (_joint.angle < -5) _canGoForward = true;

                // if (_joint.angle < 10) _canGoBack = true;
                // if (_openState == DoorOpenState.Closed && IsAchievedMaxAngle())
                // {
                //     Debug.Log("Achieved max vertical");
                //     _joint.axis = new Vector3(0, 0, -1);
                //     _joint.limits = new JointLimits {min = 0, max = 90};
                //     _currentState = DoorHandleState.Horizontal;
                // }
                //
                // if (_openState == DoorOpenState.InProgress && IsAchievedMinAngle())
                // {
                //     Debug.Log("Fully closed door");
                //     _joint.axis = new Vector3(-1, 0, 0);
                //     _openState = DoorOpenState.Closed;
                // }

                break;
            case DoorHandleState.Horizontal:
                // if ((_openState == DoorOpenState.Closed || _openState == DoorOpenState.InProgress) &&
                //     IsAchievedMaxAngle())
                // {
                //     Debug.Log("Achieved max horizontal");
                //     // transform.localEulerAngles = new Vector3(-15, 0, -90);
                //     _doorRigidbody.isKinematic = false;
                //     _openState = DoorOpenState.Opened;
                // }
                //
                // if (_openState == DoorOpenState.Opened && IsDoorClosed() && !IsAchievedMaxAngle())
                // {
                //     Debug.Log("Close door");
                //     door.transform.localEulerAngles = new Vector3(0, 0, 0);
                //     _doorRigidbody.isKinematic = true;
                //     _openState = DoorOpenState.InProgress;
                // }
                //
                // if (_openState == DoorOpenState.InProgress && IsAchievedMinAngle())
                // {
                //     Debug.Log("Door closed: switch to vertical");
                //     // transform.localEulerAngles = new Vector3(-15, 0, 0);
                //     _joint.axis = new Vector3(1, 0, 0);
                //     _joint.limits = new JointLimits {min = 0, max = 15};
                //     _currentState = DoorHandleState.Vertical;
                // }

                if (IsAchievedMaxAngle())
                {
                    Debug.Log("Done");
                }

                if (_canGoBack && IsAchievedMinAngle())
                {
                    transform.localEulerAngles = new Vector3(-15, 0, 0);
                    _joint.axis = new Vector3(-1, 0, 0);
                    _joint.limits = new JointLimits {min = 0, max = 15};
                    _currentState = DoorHandleState.Vertical;
                    _canGoForward = false;
                }

                if (_joint.angle > 30) _canGoBack = true;
                
                break;
        }
    }

    private bool IsAchievedMinAngle()
    {
        return _joint.angle <= _joint.limits.min;
    }

    private bool IsAchievedMaxAngle()
    {
        return _joint.angle >= _joint.limits.max;
    }

    private bool IsDoorClosed()
    {
        return _doorJoint.angle - DoorAngleThreshold <= _doorJoint.limits.min;
    }
}