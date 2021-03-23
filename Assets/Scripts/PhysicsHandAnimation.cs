using UnityEngine;

public class PhysicsHandAnimation : MonoBehaviour
{
    private Animator _handAnimator;
    private ThumbPosing[] _thumbPosings;

    private void Awake()
    {
        _handAnimator = GetComponentInChildren<Animator>();
        _thumbPosings = GetComponentsInChildren<ThumbPosing>();
    }

    public void StartSqueeze()
    {
        _handAnimator.enabled = false;
        foreach (var thumbPosing in _thumbPosings)
        {
            thumbPosing.StartSqueeze();
        }
    }

    public void StopSqueeze()
    {
        _handAnimator.enabled = true;
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