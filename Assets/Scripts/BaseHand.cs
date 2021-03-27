﻿using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHand : MonoBehaviour
{
    [SerializeField] protected Pose defaultPose;
    [SerializeField] protected List<Transform> fingerRoots = new List<Transform>();
    [SerializeField] protected HandType handType = HandType.None;

    public HandType HandType => handType;
    public List<Transform> Joints { get; protected set; } = new List<Transform>();

    protected virtual void Awake()
    {
        Joints = CollectJoints();
    }

    protected List<Transform> CollectJoints()
    {
        var joints = new List<Transform>();

        foreach (var fingerRoot in fingerRoots)
        {
            joints.AddRange(fingerRoot.GetComponentsInChildren<Transform>());
        }

        return joints;
    }

    public List<Quaternion> GetJointRotations()
    {
        var rotations = new List<Quaternion>();

        foreach (var joint in Joints)
        {
            rotations.Add(joint.localRotation);
        }

        return rotations;
    }

    public void ApplyDefaultPose()
    {
        ApplyPose(defaultPose);
    }

    public void ApplyPose(Pose pose)
    {
        var handInfo = pose.GetHandInfo(handType);
        ApplyFingerRotations(handInfo.fingerRotations);
        
        ApplyOffset(handInfo.attachPosition, handInfo.attachRotation);
    }

    public void ApplyFingerRotations(List<Quaternion> rotations)
    {
        if (!HasProperCount(rotations)) return;
        for (var i = 0; i < Joints.Count; i++)
        {
            Joints[i].localRotation = rotations[i];
        }
    }

    private bool HasProperCount(List<Quaternion> rotations)
    {
        return Joints.Count == rotations.Count;
    }
    
    public abstract void ApplyOffset(Vector3 position, Quaternion rotation);
}