using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    #region References

    private ConfigurableJoint _joint;

    #endregion

    private void Awake()
    {
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        
    }

    private bool IsAchievedMaxAngle()
    {
        // _joint.anglhighAngularXLimit.limit
    }
}
