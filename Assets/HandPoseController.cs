using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseController : MonoBehaviour
{
    private ThumbPosing[] _thumbPosings;
    
    private void Awake()
    {
        _thumbPosings = GetComponentsInChildren<ThumbPosing>();
    }

    public void StartSqueeze()
    {
        foreach (var thumbPosing in _thumbPosings)
        {
            thumbPosing.StartSqueeze();
        }
    }
    
    public void StopSqueeze()
    {
        foreach (var thumbPosing in _thumbPosings)
        {
            thumbPosing.StopSqueeze();
        }
    }

    public void ResetHand()
    {
        foreach (var thumbPosing in _thumbPosings)
        {
            thumbPosing.ResetThumb();
        }
    }
}
