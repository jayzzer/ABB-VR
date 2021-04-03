using System.Collections;
using UnityEngine;

public class OpenState : State
{
    public OpenState(DoorStateSystem doorStateSystemRef) : base(doorStateSystemRef)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("???");
        doorStateSystem.door.GetComponent<Rigidbody>().isKinematic = false;

        yield break;
    }

    public override IEnumerator CloseDoor()
    {
        var doorJoint = doorStateSystem.door.GetComponent<HingeJoint>();
        if (doorJoint.angle <= doorJoint.limits.min + 0.1f)
            doorStateSystem.SetState(new CanOpenState(doorStateSystem));

        yield break;
    }
}