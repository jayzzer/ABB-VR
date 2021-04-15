using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    [Serializable]
    public class ButtonEvent : UnityEvent { }

    public float pressLength;
    public bool pressed;
    public ButtonEvent downEvent;

    private Vector3 _startPos;
    private Rigidbody _rb;

    private void Awake()
    {
        _startPos = transform.localPosition;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If our distance is greater than what we specified as a press
        // set it to our max distance and register a press if we haven't already
        var distance = Mathf.Abs(transform.localPosition.z - _startPos.z);
        if (distance >= pressLength)
        {
            // Prevent the button from going past the pressLength
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _startPos.z - pressLength);
            if (!pressed)
            {
                pressed = true;
                // If we have an event, invoke it
                downEvent?.Invoke();
            }
        } else
        {
            // If we aren't all the way down, reset our press
            pressed = false;
        }
        // Prevent button from springing back up past its original position
        if (transform.localPosition.z > _startPos.z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _startPos.z);
        }
    }
}
    // private enum Axis
    // {
    //     X,
    //     Y,
    //     Z
    // }
    //
    // #region Settings
    //
    // [SerializeField] private Axis axis;
    // [SerializeField] private float maxDistance;
    //
    // #endregion
    //
    // private float _initialPos;
    //
    // private void Awake()
    // {
    //     switch (axis)
    //     {
    //         case Axis.X:
    //             _initialPos = transform.localPosition.x;
    //             break;
    //         case Axis.Y:
    //             _initialPos = transform.localPosition.y;
    //             break;
    //         case Axis.Z:
    //             _initialPos = transform.localPosition.z;
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException();
    //     }
    // }
    //
    // private void Update()
    // {
    //     
    // }
