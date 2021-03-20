﻿using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit;

public class EmptyActionBasedController : ActionBasedController
{
    protected override void UpdateTrackingInput(XRControllerState controllerState)
    {
        base.UpdateTrackingInput(controllerState);
        controllerState.poseDataFlags = PoseDataFlags.NoData;
    }
}