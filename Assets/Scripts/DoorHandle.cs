using System;
using UnityEngine;
using UnityEngine.Events;


public class DoorHandle : MonoBehaviour
{
    #region References

    private ConfigurableJoint _joint;

    #endregion

    #region Events

    public UnityEvent onReachedEnd;
    public UnityEvent onUnreachedEnd;

    #endregion

    private bool _doorCanBeOpened;

    private void Awake()
    {
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void FixedUpdate()
    {
        CheckLimit();
    }

    private void CheckLimit()
    {
        if (IsAchievedVerticalMax() && IsAchievedRotationForHorizontal())
        {
            // Запрещаем поворот ручки вверх/вниз
            _joint.lowAngularXLimit = new SoftJointLimit {limit = 14};
            // Увеличиваем поворот ручки вправо/влево
            _joint.angularZLimit = new SoftJointLimit {limit = 90};
        }

        if (IsAchievedRotationForVertical())
        {
            // Разрешаем поворот ручки вверх/вниз
            _joint.lowAngularXLimit = new SoftJointLimit {limit = 0};
            // Уменьшаем поворот ручки по вправо/влево
            _joint.angularZLimit = new SoftJointLimit {limit = 2};
        }

        if (!_doorCanBeOpened && IsAchievedHorizontalMax())
        {
            _doorCanBeOpened = true;
            onReachedEnd.Invoke();
        }
        else if (_doorCanBeOpened && !IsAchievedHorizontalMax())
        {
            _doorCanBeOpened = false;
            onUnreachedEnd.Invoke();
        }
    }

    private bool IsAchievedVerticalMax()
    {
        return transform.localRotation.eulerAngles.x > 0 && transform.localRotation.eulerAngles.x <= 345;
    }

    private bool IsAchievedRotationForHorizontal()
    {
        return transform.localRotation.eulerAngles.z > 180 && transform.localRotation.eulerAngles.z <= 358;
    }

    private bool IsAchievedRotationForVertical()
    {
        return transform.localRotation.eulerAngles.z < 180 && transform.localRotation.eulerAngles.z >= 0;
    }

    private bool IsAchievedHorizontalMax()
    {
        return transform.localRotation.eulerAngles.z > 180 && transform.localRotation.eulerAngles.z <= 275;
    }
}