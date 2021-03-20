using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ActionBasedController))]
public class PhysicsPoser : MonoBehaviour
{
    #region PhysicValues

    public float physicsRange = .1f;
    public LayerMask physicsMask = 0;

    [Range(0, 1)] public float slowDownVelocity = .75f;
    [Range(0, 1)] public float slowDownAngularVelocity = .75f;

    [Range(0, 100)] public float maxPositionChange = 75f;
    [Range(0, 100)] public float maxRotationChange = 75f;

    #endregion

    #region References

    private Rigidbody _rigidbody;
    private XRBaseInteractor _interactor;
    private ActionBasedController _controller;

    #endregion

    #region ControllerTransformInfo

    private Vector3 _targetPos = Vector3.zero;
    private Quaternion _targetRot = Quaternion.identity;

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactor = GetComponent<XRBaseInteractor>();
        _controller = GetComponent<ActionBasedController>();
    }

    private void OnEnable()
    {
        _interactor.onSelectEntered.AddListener(DisableHandColliders);
        _interactor.onSelectExited.AddListener(EnableHandColliders);
    }
    
    private void OnDisable()
    {
        _interactor.onSelectEntered.RemoveListener(DisableHandColliders);
        _interactor.onSelectExited.RemoveListener(EnableHandColliders);
    }

    private void Start()
    {
        UpdateTracking();
        MoveUsingTransform();
        RotateUsingTransform();
    }

    private void Update()
    {
        UpdateTracking();
    }

    private void FixedUpdate()
    {
        if (IsHoldingObject() || !WithinPhysicsRange())
        {
            MoveUsingTransform();
            RotateUsingTransform();
        }
        else
        {
            MoveUsingPhysics();
            RotateUsingPhysics();
        }
    }

    private void UpdateTracking()
    {
        _targetPos = _controller.positionAction.action.ReadValue<Vector3>();
        _targetRot = _controller.rotationAction.action.ReadValue<Quaternion>();
    }

    private void MoveUsingTransform()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.localPosition = _targetPos;
    }

    private void RotateUsingTransform()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = _targetRot;
    }

    private void MoveUsingPhysics()
    {
        _rigidbody.velocity *= slowDownVelocity;

        var newVelocity = FindNewVelocity();
        if (!IsValidVelocity(newVelocity.x)) return;

        var maxChange = maxPositionChange * Time.deltaTime;
        _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, newVelocity, maxChange);
    }

    private Vector3 FindNewVelocity()
    {
        var worldPosition = transform.root.TransformPoint(_targetPos);
        return (worldPosition - _rigidbody.position) / Time.deltaTime;
    }

    private void RotateUsingPhysics()
    {
        _rigidbody.angularVelocity *= slowDownAngularVelocity;

        var newVelocity = FindNewAngularVelocity();
        if (!IsValidVelocity(newVelocity.x)) return;

        var maxChange = maxRotationChange * Time.deltaTime;
        _rigidbody.angularVelocity = Vector3.MoveTowards(_rigidbody.angularVelocity, newVelocity, maxChange);
    }

    private Vector3 FindNewAngularVelocity()
    {
        var worldRotation = transform.root.rotation * _targetRot;
        var difference = worldRotation * Quaternion.Inverse(_rigidbody.rotation);
        difference.ToAngleAxis(out var angleInDegrees, out var rotationAxis);

        angleInDegrees = angleInDegrees > 180 ? angleInDegrees - 360 : angleInDegrees;

        return rotationAxis * (angleInDegrees * Mathf.Deg2Rad) / Time.deltaTime;
    }

    private bool IsValidVelocity(float velocity)
    {
        return !float.IsNaN(velocity) && !float.IsInfinity(velocity);
    }

    private bool IsHoldingObject()
    {
        return _interactor.selectTarget;
    }

    private bool WithinPhysicsRange()
    {
        return Physics.CheckSphere(transform.position, physicsRange, physicsMask, QueryTriggerInteraction.Ignore);
    }

    private void DisableHandColliders(XRBaseInteractable interactable)
    {
        var handColliders = transform.GetComponentInChildren<HandPresence>().GetComponentsInChildren<Collider>();
        foreach (var handCollider in handColliders)
        {
            handCollider.enabled = false;
        }
    }

    private void EnableHandColliders(XRBaseInteractable interactable)
    {
        StartCoroutine(WaitForRange());
    }

    private IEnumerator WaitForRange()
    {
        yield return new WaitWhile(WithinPhysicsRange);
        var handColliders = transform.GetComponentInChildren<HandPresence>().GetComponentsInChildren<Collider>();
        foreach (var handCollider in handColliders)
        {
            handCollider.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, physicsRange);
    }

    private void OnValidate()
    {
        if (TryGetComponent(out Rigidbody rigidbodyComp))
        {
            rigidbodyComp.useGravity = false;
        }
    }
}